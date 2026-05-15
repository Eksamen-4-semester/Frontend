using MongoDB.Bson.Serialization.Attributes;

namespace Frontend.Models.Membership;

public class Subscription
{
    [BsonId]
    public int SubscriptionId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
}