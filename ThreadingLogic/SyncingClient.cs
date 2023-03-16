using System;
using System.Threading;

namespace ThreadingLogic;

public class SyncingClient : Client
{
    private readonly SyncResource _resource;
    
    public SyncingClient(int id, int delay, SyncResource resource, CancellationTokenSource tokenSource) : base(id, delay, tokenSource)
    {
        _resource = resource;
    }

    protected override void DoWork(CancellationToken token)
    {
        lock (_resource)
        {
            Console.WriteLine($"Worker #{_id} occupies the resource!");
            while (!token.IsCancellationRequested)
            { 
                _resource.Ping(_id);
                Thread.Sleep(_delay);
            }
            Console.WriteLine($"Worker #{_id} frees the resource :P");
        }
    }
}