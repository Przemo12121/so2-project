using ThreadingLogic.Map;
using ThreadingLogic.Utils;

namespace ThreadingLogic.Clients;

public class ClientsManager<T> : IWaitingClientsCounter, IThreadable
    where T : IRouteAccessor
{
    private readonly Random _random;
    private readonly IClientsFactory<T> _clientsFactory;
    private readonly ICounter _waitingClientsCounter;
    private readonly Thread _thread;
    
    public int WaitingClients
    {
        get => _waitingClientsCounter.Value;
    }


    public ClientsManager(IClientsFactory<T> clientsFactory, CancellationToken cancellationToken)
    {
        _clientsFactory = clientsFactory;
        _random = new();
        
        _waitingClientsCounter = new Counter(0);
        _thread = new(() => DoWork(cancellationToken));
    }

    public void StartThread() => _thread.Start();

    private void DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Thread.Sleep(1500);
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
            
            Thread.Sleep((_random.Next() % 10 + 1) * 1000);
        }
    }

    private void OnClientEnter()
    {
        lock (_waitingClientsCounter)
        {
            _waitingClientsCounter.Decrease();
        }
    }
}