using ThreadingLogic.Buffers;
using ThreadingLogic.Map;

namespace ThreadingLogic.Clients;

public class Client<T> : IThreadable
    where T : IRouteAccessor
{
    private readonly int _delay;
    private readonly string _id;
    private readonly IRouteAccessorsBuffer<T> _goCartsRouteAccessorsBuffer;
    private readonly Action? _onEnter;
    private readonly Thread _thread;

    public Client(string id, int delay, IRouteAccessorsBuffer<T> goCartsRouteAccessorsBuffer, Action? onEnter, CancellationToken cancellationToken)
    {
        _delay = delay;
        _id = id;
        _goCartsRouteAccessorsBuffer = goCartsRouteAccessorsBuffer;
        _onEnter = onEnter;
        
        _thread = new(() => DoWork(cancellationToken));
    }

    public void StartThread() => _thread.Start();
    
    private void DoWork(CancellationToken token)
    {
        var goCart = GetGoCart(token);
        _onEnter?.Invoke();
        goCart.DoRounds(new Random().Next() % 9 + 1, token);
        LeaveGoCart(goCart);
    }

    private T GetGoCart(CancellationToken token)
    {
        T? goCart;
        do
        {
            Thread.Sleep(_delay);
            goCart = _goCartsRouteAccessorsBuffer.Get();
        } while (goCart is null || token.IsCancellationRequested);

        goCart.Marker = _id;
        goCart.Delay = _delay;

        return goCart;
    }

    private void LeaveGoCart(T goCart)
    {
        Thread.Sleep(_delay);
        _goCartsRouteAccessorsBuffer.Put(goCart);
    }
}