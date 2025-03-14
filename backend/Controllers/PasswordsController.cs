using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Data;
using backend.Models;

[Route("api/[controller]")]
[ApiController]
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
        return await _context.Passwords.ToListAsync();
    }

    // GET: api/passwords/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Password>> GetPassword(int id)
    {
        var password = await _context.Passwords.FindAsync(id);

        if (password == null)
        {
            return NotFound();
        }

        return password;
    }

    // POST: api/passwords
    [HttpPost]
    public async Task<ActionResult<Password>> PostPassword(Password password)
    {
        password.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password.HashedPassword);

        _context.Passwords.Add(password);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPassword), new { id = password.Id }, password);
    }
}