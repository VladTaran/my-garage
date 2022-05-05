using MongoDB.Bson.Serialization.Attributes;

namespace MyGarage.Models;

[BsonIgnoreExtraElements]
public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }
    
    public string Password { get; set; }
}