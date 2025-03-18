using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;
using backend.DTOs;

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
    public async Task<ActionResult<Password>> PostPassword(PasswordCreateDto dto)
    {
        var user = await _userService.GetAuthenticatedUser(User);
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        var password = new Password {
            UserId = user.Id,
            User = user,
            ServiceName = dto.ServiceName,
            HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
        };

        _context.Passwords.Add(password);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password created sucessfully" });
    }

    // PUT: api/passwords/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<Password>> UpdatePassword(int id, PasswordCreateDto dto)
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

        if (!string.IsNullOrEmpty(dto.ServiceName)) 
        {
            passwordFound.ServiceName = dto.ServiceName;
        }
        if (!string.IsNullOrEmpty(dto.Password))
        {
            passwordFound.HashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
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