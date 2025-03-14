using BCrypt.Net;

namespace backend.Models;

public class Password
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Hashes the password before storing
    public void SetPassword(string password)
    {
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verifies if the given password matches the stored hash
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
    }
}