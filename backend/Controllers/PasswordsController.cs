using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize] 
public class PasswordsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public PasswordsController(ApplicationDbContext context, IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    // GET: api/passwords
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Password>>> GetPasswords()
    {
        var user = await _userService.GetAuthenticatedUser(User);
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
        var user = await _userService.GetAuthenticatedUser(User);
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

    // PUT: api/passwords/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Password>> UpdatePassword(int id, Password password)
    {
        var user = await _userService.GetAuthenticatedUser(User);
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
        var user = await _userService.GetAuthenticatedUser(User);
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

    private async Task<Password?> GetPasswordFromIds(int pwdId, int userId)
    {
        var password = await _context.Passwords.FirstOrDefaultAsync(p => p.Id == pwdId && p.UserId == userId);
        return password;
    }
}