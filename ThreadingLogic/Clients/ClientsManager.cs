using ThreadingLogic.Map;
using ThreadingLogic.Utils;

namespace ThreadingLogic.Clients;

public class ClientsManager<T> : IWaitingClientsCounter, IThreadable
    where T : IRouteAccessor
{
    private readonly int _delay;
    private readonly IClientsFactory<T> _clientsFactory;
    private readonly ICounter _waitingClientsCounter;
    private readonly Thread _thread;
    
    public ClientsManager(int delay, IClientsFactory<T> clientsFactory, CancellationToken cancellationToken)
    {
        _delay = delay;
        _clientsFactory = clientsFactory;
        
        _waitingClientsCounter = new Counter(0);
        _thread = new(() => DoWork(cancellationToken));
    }

    public void StartThread() => _thread.Start();

    private void DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(_delay);
            if (_waitingClientsCounter.Value >= 15)
            {
                continue;
            }

            lock (_waitingClientsCounter)
            {
                _waitingClientsCounter.Increase();
            }
            
            var client = _clientsFactory.Create(OnClientEnter, cancellationToken);
            client.StartThread();
        }
    }

    private void OnClientEnter()
    {
        lock (_waitingClientsCounter)
        {
            _waitingClientsCounter.Decrease();
        }
    }

    public int CountWaitingClients() => _waitingClientsCounter.Value;
}