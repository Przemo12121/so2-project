using Microsoft.AspNetCore.SignalR;

namespace BlazorApp.Hubs;

public class MapSyncingHub : Hub
{
    public const string HubUrl = "/map";

    // public async Task MapChanged(int index, string marker)
    // {
    //     await Clients.All.SendAsync("MapChanged", index, marker);
    // }
    
    public async Task MapChanged()
    {
        await Clients.All.SendAsync("MapChanged");
    }
}