using System;
using Frontend;
using Frontend.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Local storage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddLocalStorageServices();

// Custom services
builder.Services.AddScoped<Frontend.Services.SessionService>();
builder.Services.AddScoped<Frontend.Services.UserService>();

// HttpClientFactory
builder.Services.AddHttpClient("UserClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:4000/userservices/");
});

builder.Services.AddHttpClient("SessionClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:4000/sessionservices/");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();