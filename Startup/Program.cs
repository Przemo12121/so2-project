using ThreadingLogic.Map;

var map = new Route(10);

CancellationTokenSource tokenSource = new();
Console.CursorVisible = false;
var mapPrint = new Thread(() =>
{
    while (!tokenSource.Token.IsCancellationRequested)
    {
        Thread.Sleep(100);
        Console.Clear();
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

var cart1 = new GoCart(map, "1");
cart1.Delay = 500;
var cart2 = new GoCart(map, "2");
cart2.Delay = 2000;
var cart3 = new GoCart(map, "3");
cart3.Delay = 1000;

var t1 = new Thread(() => cart1.DoRounds(10));
var t2 = new Thread(() => cart2.DoRounds(1));
var t3 = new Thread(() => cart3.DoRounds(3));

t1.Start();
Thread.Sleep(10);
t2.Start();
Thread.Sleep(6000);
t3.Start();