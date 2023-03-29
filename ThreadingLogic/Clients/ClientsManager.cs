using ThreadingLogic.Buffers;
using ThreadingLogic.Map;

namespace ThreadingLogic.Clients;

public class ClientsManager
{
    private int NextId { get; set; } = 0;
    private readonly int _delay;
    private readonly Random _random;
    private readonly IBuffer<IRouteAccessor> _goCartsBuffer;
    private readonly Counter _waitingClientsCounter;
    
    public ClientsManager(int delay, IBuffer<IRouteAccessor> goCartsBuffer, CancellationToken token)
    {
        _delay = delay;
        _goCartsBuffer = goCartsBuffer;
        _random = new Random();
        _waitingClientsCounter = new Counter(0);
        
        Thread thread = new Thread(() => DoWork(token));
        thread.Start();
    }

    private void DoWork(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(_delay);
            if (_waitingClientsCounter.Value >= 5)
            {
                continue;
            }

            lock (_waitingClientsCounter)
            {
                _waitingClientsCounter.Increase();
            }
            
            NextId = (NextId + 1) % 10;
            new Client(NextId.ToString(), (_random.Next() % 5) * 500, _goCartsBuffer, OnClientEnter, token);
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