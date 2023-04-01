using BlazorApp.Hubs;
using BlazorApp.Services;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorApp;

public static class ProgramExtenstionMethods
{
    public static void AddThreadingLogic(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<MapSyncingHub>();
        builder.Services.AddSingleton<IControllable, ThreadingLogicService>();
    }
    
    public static void UsePageRefreshingThread(this WebApplication app)
    {
        var control = app.Services.GetRequiredService<IControllable>();
        var hubUrl = app.Configuration["URLS"] + MapSyncingHub.HubUrl;
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl)
            .Build();
        
        CancellationTokenSource tokenSource = new();
        app.Lifetime.ApplicationStopped.Register
        (
            async () => 
            {
                tokenSource.Cancel();
                await hubConnection.StopAsync();
            }
        );
        
        Thread refreshView = new Thread(async () =>
        {
            await hubConnection.StartAsync();
            
            while (!tokenSource.Token.IsCancellationRequested)
            {
                if (control.IsRunning)
                {
                    await hubConnection.InvokeAsync("MapChanged");
                }
                
                Thread.Sleep(50);
            }
        });
        refreshView.Start();
    }
}