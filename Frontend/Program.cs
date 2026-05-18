using System;
using Frontend;
using Frontend.Components;
using Blazored.LocalStorage;
using Frontend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
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
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<OnlineService>();
builder.Services.AddScoped<TrainerBookingService>();
builder.Services.AddScoped<MembershipService>();

var userClientUrl = Environment.GetEnvironmentVariable("USERSERVICE_URL");
if (string.IsNullOrEmpty(userClientUrl))
{
    Console.WriteLine("UserClientUrl is not set");
    userClientUrl = "http://localhost:5034/api/";
}
var sessionClientUrl = Environment.GetEnvironmentVariable("SESSIONSERVICE_URL");
if (string.IsNullOrEmpty(sessionClientUrl))
{
    Console.WriteLine("UserClientUrl is not set");
    sessionClientUrl = "http://localhost:5003/api/";
}
var authClientUrl = Environment.GetEnvironmentVariable("AUTHSERVICE_URL");
if (string.IsNullOrEmpty(authClientUrl))
{
    Console.WriteLine("UserClientUrl is not set");
    authClientUrl = "http://localhost:5028/api/";
}
var membershipClientUrl = Environment.GetEnvironmentVariable("MEMBERSHIPSERVICE_URL");
if (string.IsNullOrEmpty(membershipClientUrl))
{
    Console.WriteLine("UserClientUrl is not set");
    membershipClientUrl = "http://localhost:5050/api/";
}

// HttpClientFactory
builder.Services.AddHttpClient("UserClient", client =>
{
    client.BaseAddress = new Uri(userClientUrl);
});

builder.Services.AddHttpClient("SessionClient", client =>
{
    client.BaseAddress = new Uri(sessionClientUrl);
});

builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri(authClientUrl);
});

builder.Services.AddHttpClient("MembershipClient", client =>
{
    client.BaseAddress = new Uri(membershipClientUrl);
});

// using Microsoft.AspNetCore.DataProtection;

// Gemmer til antiforgery tokens
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/root/.aspnet/DataProtection-Keys"));

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