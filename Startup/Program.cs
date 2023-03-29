using ThreadingLogic.Map;
using ThreadingLogic.Buffers;
using ThreadingLogic.Clients;

var map = new Route(10);
GoCart[] goCarts = Enumerable
    .Range(0, 3)
    .Select(_ => new GoCart(map))
    .ToArray();
RouteAccessorsBuffer<IRouteAccessor> goCartsBuffer = new(goCarts);
CancellationTokenSource tokenSource = new();
ClientsManager manager = new(1000, goCartsBuffer, tokenSource.Token);

Console.CursorVisible = false;
var mapPrint = new Thread(() =>
{
    while (!tokenSource.Token.IsCancellationRequested)
    {
        Thread.Sleep(100);
        Console.Clear();
        
        Console.WriteLine($"Free GoCarts: {goCartsBuffer.Count()}");
        Console.WriteLine($"Waiting clients: {manager.WaitingClientsCount()}\n");
        
        Console.Write("> ");
        Console.Write(
            map.Map
                .Select(section => String.IsNullOrEmpty(section.Occupant) ? "_" : section.Occupant)
                .Aggregate((a, b) => $"{a}{b}")
        );
        Console.Write(" <");
    }
});
mapPrint.Start();
