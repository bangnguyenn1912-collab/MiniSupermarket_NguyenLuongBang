using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            string role;

            if (request.Username == "admin" && request.Password == "123456")
            {
                role = "Admin";
            }
            else if (request.Username == "cashier" && request.Password == "123456")
            {
                role = "Cashier";
            }
            else
            {
                return Unauthorized("Sai tài khoản hoặc mật khẩu");
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var secret = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()["JwtSettings:Secret"];

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                username = request.Username,
                role = role
            });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}