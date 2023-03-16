using System;

namespace ThreadingLogic;

public class SyncResource
{
    public void Ping(int id)
    {
        Console.WriteLine($"Pinged by {id}");
    }
}