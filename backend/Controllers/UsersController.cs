using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using backend.Data;
using backend.Models;
using Microsoft.AspNetCore.Authorization;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserService _userService;

    public UsersController(ApplicationDbContext context, IConfiguration configuration, IUserService userService)
    {
        _context = context;
        _configuration = configuration;
        _userService = userService;
    }

    // GET: api/users/auth
    [HttpGet("auth")]
    [Authorize]
    public async Task<ActionResult<User>> GetUserAuth()
    {
        var user = await _userService.GetAuthenticatedUser(User);
        if (user == null)
        {
            return Unauthorized(new { message = "User not authorized" });
        }

        return Ok(new { message = "User authorized" });
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

        return Ok(new { message = "User created sucessfully" });
    }

    // POST: api/users/login
    [HttpPost("login")]
    public async Task<IActionResult> Login(User user)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
        if (existingUser == null)
            return Unauthorized(new { message = "Email or password is invalid" });

        if (!BCrypt.Net.BCrypt.Verify(user.HashedPassword, existingUser.HashedPassword))
            return Unauthorized(new { message = "Email or password is invalid" });

        var token = GenerateJwtToken(existingUser);
        return Ok(new { token = token, userName = existingUser.Name });
    }

    private string GenerateJwtToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"] 
            ?? throw new InvalidOperationException("JWT Key is missing in configuration.");
        var jwtIssuer = _configuration["Jwt:Issuer"] 
            ?? throw new InvalidOperationException("JWT Issuer is missing in configuration.");
        var jwtAudience = _configuration["Jwt:Audience"] 
            ?? throw new InvalidOperationException("JWT Audience is missing in configuration.");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            jwtIssuer,
            jwtAudience,
            claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}