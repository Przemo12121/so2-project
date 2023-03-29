using ThreadingLogic.Map;

namespace ThreadingLogic.Buffers;

public class RouteAccessorsBuffer<T> : IBuffer<T>
    where T : class, IRouteAccessor
{
    private readonly Queue<T> _goCarts;

    public RouteAccessorsBuffer(IEnumerable<T> goCarts)
    {
        _goCarts = new Queue<T>(goCarts);
    }

    public int Count() => _goCarts.Count;
    
    public T? Get()
    {
        lock (_goCarts)
        {
            return _goCarts.Any() ? _goCarts.Dequeue() : default;
        }
    }

    public void Put(T goCart)
    {
        lock (_goCarts)
        {
            _goCarts.Enqueue(goCart);
        }
    }
}