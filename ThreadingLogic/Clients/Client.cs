using ThreadingLogic.Buffers;
using ThreadingLogic.Map;

namespace ThreadingLogic.Clients;

public class Client
{
    private readonly int _delay;
    private readonly string _id;
    private readonly IBuffer<IRouteAccessor> _goCartsBuffer;
    private readonly Action? _onEnter;

    public Client(string id, int delay, IBuffer<IRouteAccessor> goCartsBuffer, Action? onEnter, CancellationToken cancellationToken)
    {
        _delay = delay;
        _id = id;
        _goCartsBuffer = goCartsBuffer;
        _onEnter = onEnter;
        
        Thread thread = new(() => DoWork(cancellationToken));
        thread.Start();
    }

    private void DoWork(CancellationToken token)
    {
        var goCart = GetGoCart(token);
        _onEnter?.Invoke();
        goCart.DoRounds(new Random().Next() % 9 + 1, token);
        LeaveGoCart(goCart);
    }

    private IRouteAccessor GetGoCart(CancellationToken token)
    {
        IRouteAccessor? goCart;
        do
        {
            Thread.Sleep(_delay);
            goCart = _goCartsBuffer.Get();
        } while (goCart is null || token.IsCancellationRequested);

        goCart.Marker = _id;
        goCart.Delay = _delay;

        return goCart;
    }

    private void LeaveGoCart(IRouteAccessor goCart)
    {
        Thread.Sleep(_delay);
        _goCartsBuffer.Put(goCart);
    }
}