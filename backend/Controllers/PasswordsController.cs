using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using backend.Data;
using backend.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class PasswordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PasswordsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/passwords
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
    {
        var user = await GetAuthenticatedUser();
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        return await _context.Passwords
            .Where(p => p.UserId == user.Id)
            .ToListAsync();
    }

    // POST: api/passwords
    [HttpPost]
    public async Task<ActionResult<Password>> PostPassword(Password password)
    {
        var user = await GetAuthenticatedUser();
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        password.UserId = user.Id;
        password.User = user;
        password.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password.HashedPassword); // todo change

        _context.Passwords.Add(password);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password created sucessfully" });
    }

    // UPDATE: api/passwords/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Password>> UpdatePassword(int id, Password password)
    {
        var user = await GetAuthenticatedUser();
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        var passwordFound = await GetPasswordFromIds(id, user.Id);
        if (passwordFound == null)
        {
            return NotFound(new { message = "Password not found" });
        }

        if (!string.IsNullOrEmpty(password.ServiceName)) 
        {
            passwordFound.ServiceName = password.ServiceName;
        }
        if (!string.IsNullOrEmpty(password.HashedPassword))
        {
            passwordFound.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password.HashedPassword);
        }
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password updated sucessfully" });
    }

    // DELETE: api/passwords/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePassword(int id)
    {
        var user = await GetAuthenticatedUser();
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        var passwordFound = await GetPasswordFromIds(id, user.Id);
        if (passwordFound == null)
        {
            return NotFound(new { message = "Password not found" });
        }

        _context.Passwords.Remove(passwordFound);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password deleted successfully" });
    }

    private async Task<User?> GetAuthenticatedUser()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return null;
        }

        if (!int.TryParse(userIdClaim, out int userId))
        {
            return null;
        }

        var user = await _context.Users.FindAsync(userId);
        return user;
    }

    private async Task<Password?> GetPasswordFromIds(int pwdId, int userId)
    {
        var password = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == pwdId && p.UserId == userId);
        return password;
    }
}