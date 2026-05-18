using System.Net.Http.Headers;
using Frontend.Models.Membership;

namespace Frontend.Services;

public class MembershipService
{
    
    private readonly HttpClient _httpClient;
    public MembershipService(IHttpClientFactory httpClient)
    {
        _httpClient = httpClient.CreateClient("MembershipClient");
    }

    public async Task<MembershipUserMembershipDto?> GetMemberSubscriptionById(int memberId, string jwt)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        
        var result = await _httpClient.GetAsync($"MemberSubscription/{memberId}");
        if (result.IsSuccessStatusCode)
        {
            var userMemberShip = await result.Content.ReadFromJsonAsync<MemberSubscription>();
            if (userMemberShip == null)
                return null;
            
            var subscriptionResult = await _httpClient.GetAsync($"subscription/{userMemberShip.MemberId}");
            if (subscriptionResult.IsSuccessStatusCode)
            {
                var subscription =
                    await subscriptionResult.Content.ReadFromJsonAsync<Subscription>();
                if (subscription == null)
                    return null;

                var dto = new MembershipUserMembershipDto()
                {
                    Subscription = subscription,
                    MemberSubscription = userMemberShip
                };
                
                return dto;
            }
        }
        return null;
    }

    public async Task<List<Subscription>?> GetAllSubscriptions()
    {
        var result = await _httpClient.GetAsync("subscription");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<List<Subscription>>();
        }
        return null;
    }

    public async Task<List<AddOn>?> GetAllAddOns()
    {
        var result = await _httpClient.GetAsync("addon");
        if (result.IsSuccessStatusCode)
        {
            return await result.Content.ReadFromJsonAsync<List<AddOn>>();
        }
        return null;
    }
    
}