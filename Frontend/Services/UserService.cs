using System;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using Frontend.Models.User;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FitLife.Models.User;

namespace Frontend.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public UserService(
        IHttpClientFactory factory,
        ILocalStorageService localStorage)
    {
        _httpClient = factory.CreateClient("UserClient");
        _localStorage = localStorage;
    }

    public async Task<bool> LoginMember(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "Member/login",
            loginDto);

        if (!response.IsSuccessStatusCode)
            return false;

        var token = await response.Content.ReadAsStringAsync();

        await _localStorage.SetItemAsync("jwt", token);
        await _localStorage.SetItemAsync("CurrentRole", "Member");
        return true;
    }

    public async Task<bool> LoginAdmin(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "Admin/login",
            loginDto);

        if (!response.IsSuccessStatusCode)
            return false;

        var token = await response.Content.ReadAsStringAsync();

        await _localStorage.SetItemAsync("jwt", token);
        await _localStorage.SetItemAsync("CurrentRole", "Admin");
        return true;
    }

    public async Task<bool> LoginTrainer(LoginDto loginDto)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "PersonalTrainer/login",
            loginDto);

        if (!response.IsSuccessStatusCode)
            return false;

        var token = await response.Content.ReadAsStringAsync();

        await _localStorage.SetItemAsync("jwt", token);
        await _localStorage.SetItemAsync("CurrentRole", "Trainer");
        return true;
    }

    public async Task<Member?> GetMemberById(int memberId)
    {
        var response = await _httpClient.GetFromJsonAsync<Member?>($"{memberId}");
        return response;
    }

    public async Task<Member?> GetMemberByJwt(string jwt)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", jwt);
        
            var response = await _httpClient.GetAsync("member/own");
        
            if (!response.IsSuccessStatusCode)
                return null;
            
            return await response.Content.ReadFromJsonAsync<Member>();
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<string?> GetCurrentRole() //kritisk for at det hele fungere. MÅ IKKE SLETTES
    {
        return await _localStorage.GetItemAsync<string>("CurrentRole");
    }
    
    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("jwt");
    }

    public async Task<bool> IsLoggedIn() //kontrollere om der er en token i local storage, hvis der er det så er brugeren logget ind, hvis ikke så er brugeren ikke logget ind
    {
        var token = await _localStorage.GetItemAsync<string>("jwt");

        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>("jwt");
    }
}