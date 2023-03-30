using BlazorApp;
using BlazorApp.Hubs;
using ThreadingLogic.Buffers;
using ThreadingLogic.Clients;
using ThreadingLogic.Map;
using Route = Microsoft.AspNetCore.Routing.Route;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddServerSideBlazor();

builder.AddThreadingLogic();

var app = builder.Build();
// app.UseResponseCompression();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapHub<MapSyncingHub>(MapSyncingHub.HubUrl);
app.MapFallbackToPage("/_Host");

app.StartThreads();

app.Run();
