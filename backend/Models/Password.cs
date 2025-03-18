using System.Text.Json.Serialization;

namespace backend.Models;

public class Password
{
    [JsonIgnore]
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    [JsonIgnore]
    public string HashedPassword { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // User table relationship
    [JsonIgnore]
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}