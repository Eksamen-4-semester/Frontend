using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace Frontend.Models.User;

public class PersonalTrainer
{
    [Required]
    [BsonId]
    public int PersonalTrainerId { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Username { get; set; }
    public string Password { get; set; }
}