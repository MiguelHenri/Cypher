using System.Security.Claims;
using backend.Data;
using backend.Models;

public interface IUserService
{
    Task<User?> GetAuthenticatedUser(ClaimsPrincipal user);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetAuthenticatedUser(ClaimsPrincipal userClaims)
    {
        var userIdClaim = userClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return null;
        }

        var user = await _context.Users.FindAsync(userId);
        return user;
    }
}
