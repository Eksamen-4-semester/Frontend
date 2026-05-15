using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontend.Models.Membership;

public class MemberAddOns
{
    [BsonId]
    public int MemberAddOnId { get; set; }
    public List<int> AddOnIds { get; set; } = new List<int>();
    public int MemberSubscriptionId { get; set; }
}