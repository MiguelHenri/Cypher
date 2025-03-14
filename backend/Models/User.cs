using BCrypt.Net;

namespace backend.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string HashedPassword { get; private set; } = string.Empty;

    // Hash the password before saving
    public void SetPassword(string password)
    {
        HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    }

    // Verify password
    public bool VerifyPassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, HashedPassword);
    }
}
