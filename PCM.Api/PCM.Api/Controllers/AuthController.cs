using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PCM.Api.Data;
using PCM.Api.DTOs;
using PCM.Api.Models;

namespace PCM.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IConfiguration config,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _config = config;
            _context = context;
        }

        // ================= LOGIN =================
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // Validate input
            if (dto == null ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and password are required");
            }

            IdentityUser? user;

            // Find user
            if (dto.Email.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(dto.Email);
            }
            else
            {
                user = await _userManager.FindByNameAsync(dto.Email);
            }

            if (user == null)
            {
                return Unauthorized("Invalid username/email or password");
            }

            // Check password
            var result = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!result)
            {
                return Unauthorized("Invalid username/email or password");
            }

            // JWT config
            var jwtKey = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(jwtKey))
            {
                jwtKey = "PCM_DEFAULT_SECRET_KEY_123456789_ABC";
            }

            var issuer = _config["Jwt:Issuer"] ?? "PCM.Api";
            var audience = _config["Jwt:Audience"] ?? "PCM.Client";

            // Claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? user.UserName ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create token
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        // ================= REGISTER =================
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Email and Password are required");
            }

            var existedUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existedUser != null)
            {
                return BadRequest("Email already exists");
            }

            var user = new IdentityUser
            {
                UserName = dto.Email,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var member = new Member
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                DateOfBirth = dto.DateOfBirth,
                UserId = user.Id,
                JoinDate = DateTime.Now,
                IsActive = true,
                RankLevel = 0,
                TotalMatches = 0,
                WinMatches = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return Ok("Register success");
        }

        // ================= CURRENT USER =================
        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName
            });
        }
    }
}
