namespace backend.Models;

public class Password
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string HashedPassword { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // User table relationship
    public int UserId { get; set; }
    public User? User { get; set; }
}