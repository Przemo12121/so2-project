using BlazorApp.Hubs;
using Microsoft.AspNetCore.SignalR.Client;
using ThreadingLogic.Buffers;
using ThreadingLogic.Clients;
using ThreadingLogic.Map;

namespace BlazorApp;

public static class ProgramExtenstionMethods
{
    public static void AddThreadingLogic(this WebApplicationBuilder builder)
    {
        string[] colors = new[] { "#fe6efe", "#a5fe6e", "#bc93ec", "#8fc9ff", "#92ec3e", "#fa5594", "#ff9a6b", "#d9e069", "#57ff85", "#4ccffc" };
        
        ThreadingLogic.Map.Route map = new(28);
        GoCart[] goCarts = Enumerable
            .Range(0, 10)
            .Select(i => new GoCart(map, colors[i]))
            .ToArray();
        
        builder.Services.AddSingleton<IRoute, ThreadingLogic.Map.Route>(service => map);
        builder.Services.AddSingleton<RouteAccessorsBuffer<IRouteAccessor>>(service => new (goCarts));
        builder.Services.AddSingleton<MapSyncingHub>();
    }
    
    public static void StartThreads(this WebApplication app)
    {
        var hubUrl = app.Configuration["URLS"] + MapSyncingHub.HubUrl;
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
        
        var buffer = app.Services.GetService<RouteAccessorsBuffer<IRouteAccessor>>();
        
        CancellationTokenSource tokenSource = new();
        new ClientsManager(1000, buffer!, tokenSource.Token);
        
        app.Lifetime.ApplicationStopped.Register(async () =>
        {
            await hubConnection.StopAsync();
            tokenSource.Cancel();
        });
        
        var mapPrint = new Thread(async () =>
        {
            await hubConnection.StartAsync();
            
            while (!tokenSource.Token.IsCancellationRequested)
            {
                Thread.Sleep(50);
                await hubConnection.InvokeAsync("MapChanged");
            }
        });
        mapPrint.Start();
    }
}