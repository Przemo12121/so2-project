using System;
using System.Threading;

namespace ThreadingLogic;

public class Client
{
    protected readonly int _delay;
    protected readonly int  _id;
    private readonly Thread _thread;
    
    public Client(int id, int delay, CancellationTokenSource tokenSource)
    {
        _delay = delay;
        _id = id;
        
        _thread = new Thread(() => DoWork(tokenSource.Token));
        _thread.Start();
    }

    protected virtual void DoWork(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            Thread.Sleep(_delay);
            Console.WriteLine($"Worker #{_id}");
        }
    }
}