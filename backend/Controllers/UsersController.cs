using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net;
using backend.Data;
using backend.Models;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/users
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    // GET: api/users/5
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        return user;
    }

    // POST: api/users/register
    [HttpPost("register")]
    public async Task<ActionResult<User>> Register(User user)
    {
        if (_context.Users.Any(u => u.Name == user.Name))
        {
            return BadRequest(new { message = "User already exists" });
        }

        user.HashedPassword = BCrypt.Net.BCrypt.HashPassword(user.HashedPassword);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Name == user.Name);
        if (existingUser == null)
            return Unauthorized(new { message = "User or password is invalid" });

        if (!BCrypt.Net.BCrypt.Verify(user.HashedPassword, existingUser.HashedPassword))
            return Unauthorized(new { message = "User or password is invalid" });

        return Ok(new { message = "Sucessful login" });
    }
}