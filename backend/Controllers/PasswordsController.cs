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
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrEmpty(userIdClaim)) {
            return Unauthorized("User not authorized");
        }

        int userId = int.Parse(userIdClaim);

        return await _context.Passwords
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    // POST: api/passwords
    [HttpPost]
    public async Task<ActionResult<Password>> PostPassword(Password password)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(string.IsNullOrEmpty(userIdClaim)) {
            return Unauthorized("User not authorized");
        }

        int userId = int.Parse(userIdClaim);

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        password.UserId = userId;
        password.User = user;
        password.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password.HashedPassword); // todo change

        _context.Passwords.Add(password);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Password created sucessfully" });
    }
}