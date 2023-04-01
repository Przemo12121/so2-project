using ThreadingLogic.Map;

namespace ThreadingLogic.Buffers;

public class GoCartsBuffer : IRouteAccessorsBuffer<GoCart>
{
    private readonly Queue<GoCart> _goCarts;

    public GoCartsBuffer(IEnumerable<GoCart> goCarts)
    {
        _goCarts = new Queue<GoCart>(goCarts);
    }

    public int Count() => _goCarts.Count;
    
    public GoCart? Get()
    {
        lock (_goCarts)
        {
            return _goCarts.Any() ? _goCarts.Dequeue() : default;
        }
    }

    public void Put(GoCart goCart)
    {
        lock (_goCarts)
        {
            _goCarts.Enqueue(goCart);
        }
    }

    public IReadOnlyList<string> Preview() 
        => _goCarts
            .Select(cart => cart.HexColor)
            .ToList()
            .AsReadOnly();
}