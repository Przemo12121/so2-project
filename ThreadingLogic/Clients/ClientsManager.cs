using ThreadingLogic.Map;
using ThreadingLogic.Utils;
using ThreadingLogic.Buffers;

namespace ThreadingLogic.Clients;

public class ClientsManager
{
    private int NextId { get; set; } = 0;
    private readonly int _delay;
    private readonly Random _random;
    private readonly IBuffer<IRouteAccessor> _goCartsBuffer;
    private readonly ICounter _waitingClientsCounter;
    
    public ClientsManager(int delay, IBuffer<IRouteAccessor> goCartsBuffer, CancellationToken cancellationToken)
    {
        _delay = delay;
        _goCartsBuffer = goCartsBuffer;
        
        _random = new();
        _waitingClientsCounter = new Counter(0);
        
        Thread thread = new(() => DoWork(cancellationToken));
        thread.Start();
    }

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
            
            NextId = (NextId + 1) % 99;
            new Client(
                id: NextId.ToString(), 
                delay: (_random.Next() % 10) * 100 + 100, 
                goCartsBuffer: _goCartsBuffer, 
                onEnter: OnClientEnter, 
                cancellationToken: cancellationToken
            );
        }
    }

    private void OnClientEnter()
    {
        lock (_waitingClientsCounter)
        {
            _waitingClientsCounter.Decrease();
        }
    }

    public int WaitingClientsCount() => _waitingClientsCounter.Value;
}