using BlazorApp.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using ThreadingLogic.Buffers;
using ThreadingLogic.Clients;
using ThreadingLogic.Map;

namespace BlazorApp;

public static class ProgramExtenstionMethods
{
    private static CancellationTokenSource TokenSource = new();
    private static string[] Colors = { "#fe6efe", "#a5fe6e", "#bc93ec", "#8fc9ff", "#92ec3e", "#fa5594", "#ff9a6b", "#d9e069", "#57ff85", "#4ccffc" };
    
    public static void AddThreadingLogic(this WebApplicationBuilder builder)
    {
        ThreadingLogic.Map.Route map = new(28);
        GoCart[] goCarts = Enumerable
            .Range(0, 10)
            .Select(i => new GoCart(map, Colors[i]))
            .ToArray();
        GoCartsBuffer buffer = new GoCartsBuffer(goCarts);
        ClientsManager<GoCart> manager = new(
            delay: 1000,
            clientsFactory: new GoCartClientsFactory(buffer),
            cancellationToken: TokenSource.Token
        );
        
        builder.Services.AddSingleton<IWaitingClientsCounter, ClientsManager<GoCart>>(_ => manager);
        builder.Services.AddSingleton<IThreadable, ClientsManager<GoCart>>(_ => manager);
        builder.Services.AddSingleton<IRoute, ThreadingLogic.Map.Route>(_ => map);
        builder.Services.AddSingleton<GoCartsBuffer>(_ => buffer);
        builder.Services.AddSingleton<MapSyncingHub>();
    }
    
    public static void StartThreads(this WebApplication app)
    {
        var hubUrl = app.Configuration["URLS"] + MapSyncingHub.HubUrl;
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
        
        Console.WriteLine(app.Configuration["URLS"]);
        
        var manager = app.Services.GetService<IThreadable>();
        manager!.StartThread();
        
        app.Lifetime.ApplicationStopped.Register(async () =>
        {
            await hubConnection.StopAsync();
            TokenSource.Cancel();
        });
        
        var mapPrint = new Thread(async () =>
        {
            await hubConnection.StartAsync();
            
            while (!TokenSource.Token.IsCancellationRequested)
            {
                Thread.Sleep(50);
                await hubConnection.InvokeAsync("MapChanged");
            }
        });
        mapPrint.Start();
    }
}