using ThreadingLogic.Buffers;
using ThreadingLogic.Map;

namespace ThreadingLogic.Clients;

public class GoCartClientsFactory : IClientsFactory<GoCart>
{
    private int NextId { get; set; } = 0;
    private readonly Random _random ;
    private readonly IRouteAccessorsBuffer<GoCart> _goCartsBuffer;

    public GoCartClientsFactory(IRouteAccessorsBuffer<GoCart> goCartsBuffer)
    {
        _goCartsBuffer = goCartsBuffer;
        _random = new();
    }
    
    public Client<GoCart> Create(Action onEnter, CancellationToken cancellationToken)
    {
        NextId = (NextId + 1) % 99;
        
        return new (
            id: NextId.ToString(), 
            delay: (_random.Next() % 10 + 1) * 50, 
            goCartsRouteAccessorsBuffer: _goCartsBuffer, 
            onEnter: onEnter, 
            cancellationToken: cancellationToken
        );
    }
}