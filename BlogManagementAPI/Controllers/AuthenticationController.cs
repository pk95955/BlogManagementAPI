 
using BlogManagementAPI.DTOs.Request;
using BlogManagementAPI.Models;
using BlogManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        // For simplicity, using in-memory user list. Replace with DB in production
        private static readonly List<User> _users = new();

        public AuthController(IConfiguration configuration, IUserService userServices)
        {
            _configuration = configuration;
            _userService = userServices;
        }
        

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDTO loginDto)
        {
            var user = _userService.ValidateUser(loginDto.Username, loginDto.Password).Result;
            if (user == null)
                return Unauthorized(new { status = 401, title = "Unauthorized", detail = "Invalid credentials." });
            else
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["Secret"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expiryMinutes = Convert.ToInt32(jwtSettings["ExpiryMinutes"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username!),
                new Claim(ClaimTypes.Name, user.Username!),
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

