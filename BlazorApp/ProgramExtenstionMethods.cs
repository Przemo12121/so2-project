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
        RouteAccessorsBuffer<IRouteAccessor> goCartsBuffer = new(goCarts);
        
        
        builder.Services.AddSingleton<IRoute, ThreadingLogic.Map.Route>(service => map);
        builder.Services.AddSingleton<RouteAccessorsBuffer<IRouteAccessor>>(service => goCartsBuffer);

        builder.Services.AddSingleton<MapSyncingHub>();
    }
    
    public static void StartThreads(this WebApplication app)
    {
        var hubUrl = "http://localhost:5224" + MapSyncingHub.HubUrl;
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
        
        hubConnection.On("Abc", () => { });
        
        var buffer = app.Services.GetService<RouteAccessorsBuffer<IRouteAccessor>>();
        var map = app.Services.GetService<IRoute>();
        
        
        CancellationTokenSource tokenSource = new();
        ClientsManager manager = new(1000, buffer!, tokenSource.Token);
        
        app.Lifetime.ApplicationStopped.Register(async () =>
        {
            await hubConnection.StopAsync();
            tokenSource.Cancel();
        });
        
        Console.CursorVisible = false;
        var mapPrint = new Thread(async () =>
        {
            await hubConnection.StartAsync();
            
            while (!tokenSource.Token.IsCancellationRequested)
            {
                Thread.Sleep(50);
                // Console.Clear();
                //
                // Console.WriteLine($"Free GoCarts: {buffer.Count()}");
                // Console.WriteLine($"Waiting clients: {manager.WaitingClientsCount()}\n");
                //
                // Console.Write("> ");
                // Console.Write(
                //     map!.Map
                //         .Select(section => section.Occupant?.Marker ?? "_")
                //         .Aggregate((a, b) => $"{a}{b}")
                // );
                // Console.Write(" <");

                await hubConnection.InvokeAsync("MapChanged");
            }
        });
        mapPrint.Start();
    }
}