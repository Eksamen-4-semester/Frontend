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
builder.Services.AddBlazorBootstrap();

// Custom services
builder.Services.AddScoped<SessionService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AnalyticsService>();
builder.Services.AddScoped<OnlineService>();
builder.Services.AddScoped<TrainerBookingService>();
builder.Services.AddScoped<MembershipService>();

var userClientUrl = Environment.GetEnvironmentVariable("USERSERVICE_URL");
if (string.IsNullOrEmpty(userClientUrl))
    throw new Exception("UserClientUrl is not set");
var sessionClientUrl = Environment.GetEnvironmentVariable("SESSIONSERVICE_URL");
if (string.IsNullOrEmpty(sessionClientUrl))
    throw new Exception("SessionClientUrl is not set");
var authClientUrl = Environment.GetEnvironmentVariable("AUTHSERVICE_URL");
if (string.IsNullOrEmpty(authClientUrl))
    throw new Exception("AuthClientUrl is not set");
var membershipClientUrl = Environment.GetEnvironmentVariable("MEMBERSHIPSERVICE_URL");
if (string.IsNullOrEmpty(membershipClientUrl))
    throw new Exception("MembershipClientUrl is not set");
var analyticsClientUrl = Environment.GetEnvironmentVariable("ANALYTICSSERVICE_URL");
if (string.IsNullOrEmpty(analyticsClientUrl))
    throw new Exception("AnalyticsClientUrl is not set");

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

builder.Services.AddHttpClient("AnalyticsClient", client =>
{
    client.BaseAddress = new Uri(analyticsClientUrl);
});

// using Microsoft.AspNetCore.DataProtection;

// Gemmer til antiforgery tokens
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("DataProtection-Keys"));

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