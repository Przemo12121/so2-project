using ThreadingLogic;

Console.WriteLine("Hello, World!");

CancellationTokenSource source1 = new();
CancellationTokenSource source2 = new();

Client c1 = new (1, 1000, source1);
Client c2 = new (2, 3000, source2);

source1.CancelAfter(5000);
source2.CancelAfter(5000);

Thread.Sleep(7000);
Console.WriteLine("\nPart2\n");

SyncResource resource = new();
CancellationTokenSource source3 = new();
CancellationTokenSource source4 = new();

var sc1 = new SyncingClient(3, 3500, resource, source3);
var sc2 = new SyncingClient(4, 1500, resource, source4);

source3.CancelAfter(7000);
source4.CancelAfter(10000);
Thread.Sleep(11000);

Console.WriteLine("\nPart3\n");

SyncResource resource2 = new();
SyncResource resource3 = new();
CancellationTokenSource source5 = new();
CancellationTokenSource source6 = new();

var sc3 = new SyncingClient(3, 3500, resource2, source5);
var sc4 = new SyncingClient(4, 1500, resource3, source6);

source5.CancelAfter(7000);
source6.CancelAfter(10000);