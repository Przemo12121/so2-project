using ThreadingLogic.Buffers;
using ThreadingLogic.Clients;
using ThreadingLogic.Map;

namespace BlazorApp.Services;

public class ThreadingLogicService : IControllable
{
    public bool IsRunning { get; private set; }
    
    private readonly string[] _colors = { "#fe6efe", "#a5fe6e", "#bc93ec", "#8fc9ff", "#92ec3e", "#fa5594", "#ff9a6b", "#d9e069", "#57ff85", "#4ccffc" };
    
    private CancellationTokenSource TokenSource { get; set; }
    private ClientsManager<GoCart>? Manager { get; set; }
    private GoCartsBuffer? GoCartsBuffer { get; set; }
    private IRoute<Section>? RaceRoute { get; set; }
    
    public void Start()
    {
        Reset();
        Manager!.StartThread();
        IsRunning = true;
    }
    
    public void Stop()
    {
        TokenSource.Cancel();
        Reset();
        IsRunning = false;
    }

    private void Reset()
    {
        TokenSource = new();
        RaceRoute = new RaceRoute(28);
        
        var goCarts = Enumerable
            .Range(0, 10)
            .Select(i => new GoCart(RaceRoute, _colors[i]))
            .ToArray();
        
        GoCartsBuffer = new(goCarts);
        
        Manager = new(
            clientsFactory: new GoCartClientsFactory(GoCartsBuffer),
            cancellationToken: TokenSource.Token
        );
    }
    
    public ThreadingLogicStatus Status()
        => new 
        (
            WaitingClientsCount: Manager?.WaitingClients ?? 0,
            GoCartsBuffer?.Preview() ?? Array.Empty<string>(),
            Map: RaceRoute?.Map ?? Enumerable.Empty<Section>().ToList().AsReadOnly()
        );
}