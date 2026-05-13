using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using Frontend.Models;
using Frontend.Models.Session;

namespace Frontend.Services;

public class SessionService
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;

    public SessionService(
        IHttpClientFactory factory,
        ILocalStorageService localStorage)
    {
        _httpClient =
            factory.CreateClient("SessionClient");

        _localStorage = localStorage;
    }

    private async Task AddJwtToken()
    {
        var token =
            await _localStorage.GetItemAsync<string>(
                "jwt");

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    token);
        }
    }

    // =========================================
    // SESSIONS
    // =========================================

    // HENTER ALLE HOLDTRÆNINGER

    public async Task<List<Session>> GetAllSessions()
    {
        await AddJwtToken();

        return await _httpClient
                   .GetFromJsonAsync<List<Session>>(
                       "api/session")
               ?? new List<Session>();
    }

    // HENTER ÉN SESSION UD FRA ID

    public async Task<Session?> GetSessionById(
        int sessionId)
    {
        await AddJwtToken();

        return await _httpClient
            .GetFromJsonAsync<Session>(
                $"api/session/{sessionId}");
    }

    // HENTER ALLE SESSIONS SOM ET MEDLEM ER TILMELDT

    public async Task<List<Session>> GetMemberSessions(
        int memberId)
    {
        await AddJwtToken();

        return await _httpClient
                   .GetFromJsonAsync<List<Session>>(
                       $"api/members/{memberId}/sessions")
               ?? new List<Session>();
    }

    // HENTER ALLE SESSIONS FOR EN SPECIFIK TRÆNER
    // LAVET I FRONTEND INDTIL BACKEND ENDPOINT FINDES

    public async Task<List<Session>> GetTrainerSessions(
        int trainerId)
    {
        var allSessions =
            await GetAllSessions();

        return allSessions
            .Where(x =>
                x.InstructorId == trainerId)
            .ToList();
    }

    // OPRETTER NY SESSION

    public async Task<bool> CreateSession(Session session)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PostAsJsonAsync(
                "api/session",
                session);

        return response.IsSuccessStatusCode;
    }

    // OPDATERER SESSION

    public async Task<bool> UpdateSession(
        int sessionId,
        Session session)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PutAsJsonAsync(
                $"api/session/{sessionId}",
                session);

        return response.IsSuccessStatusCode;
    }

    // SLETTER SESSION

    public async Task<bool> DeleteSession(
        int sessionId)
    {
        await AddJwtToken();

        var response =
            await _httpClient.DeleteAsync(
                $"api/session/{sessionId}");

        return response.IsSuccessStatusCode;
    }

    // =========================================
    // BOOKINGS
    // =========================================

    // MEDLEM TILMELDER SIG EN SESSION

    public async Task<bool> CreateBooking(
        int memberId,
        int sessionId)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PostAsync(
                $"api/members/{memberId}/bookings/{sessionId}",
                null);

        return response.IsSuccessStatusCode;
    }

    // MEDLEM AFBESTILLER EN BOOKING
    // KRÆVER BOOKING ID

    public async Task<bool> CancelBooking(
        int memberId,
        int bookingId)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PutAsync(
                $"api/members/{memberId}/bookings/{bookingId}/cancel",
                null);

        return response.IsSuccessStatusCode;
    }

    // TJEKKER OM ET MEDLEM ALLEREDE ER TILMELDT
    // BRUGES TIL UI KNAPPER

    public async Task<bool> IsMemberBooked(
        int memberId,
        int sessionId)
    {
        var sessions =
            await GetMemberSessions(memberId);

        return sessions.Any(x =>
            x.SessionId == sessionId);
    }

    // =========================================
    // ROOMS
    // =========================================

    // HENTER ALLE ROOMS

    public async Task<List<Room>> GetRooms()
    {
        await AddJwtToken();

        return await _httpClient
                   .GetFromJsonAsync<List<Room>>(
                       "api/rooms")
               ?? new List<Room>();
    }

    // HENTER ROOM UD FRA ID

    public async Task<Room?> GetRoomById(
        int roomId)
    {
        await AddJwtToken();

        return await _httpClient
            .GetFromJsonAsync<Room>(
                $"api/rooms/{roomId}");
    }

    // OPRETTER ROOM
    // KUN ADMIN

    public async Task<bool> CreateRoom(
        Room room)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PostAsJsonAsync(
                "api/rooms",
                room);

        return response.IsSuccessStatusCode;
    }

    // =========================================
    // FITNESS CENTERS
    // =========================================

    // HENTER ALLE FITNESS CENTERS

    public async Task<List<FitnessCenter>>
        GetFitnessCenters()
    {
        await AddJwtToken();

        return await _httpClient
                   .GetFromJsonAsync<List<FitnessCenter>>(
                       "api/fitnesscenters")
               ?? new List<FitnessCenter>();
    }

    // HENTER FITNESS CENTER UD FRA ID

    public async Task<FitnessCenter?>
        GetFitnessCenterById(
            int centerId)
    {
        await AddJwtToken();

        return await _httpClient
            .GetFromJsonAsync<FitnessCenter>(
                $"api/fitnesscenters/{centerId}");
    }

    // OPRETTER FITNESS CENTER
    // KUN ADMIN

    public async Task<bool> CreateCenter(
        FitnessCenter center)
    {
        await AddJwtToken();

        var response =
            await _httpClient.PostAsJsonAsync(
                "api/fitnesscenters",
                center);

        return response.IsSuccessStatusCode;
    }
}