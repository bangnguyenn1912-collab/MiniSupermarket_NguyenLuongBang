
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MiniSupermarket.API.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniSupermarket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DemoAccountStore _accountStore;

        // Nhận cấu hình JWT và dịch vụ tài khoản
        public AuthController(
            IConfiguration configuration,
            DemoAccountStore accountStore)
        {
            _configuration = configuration;
            _accountStore = accountStore;
        }

        // POST /api/auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestDto request)
        {
            // Kiểm tra dữ liệu đăng nhập
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Vui lòng nhập tài khoản và mật khẩu."
                });
            }

            // Xác thực bằng dịch vụ dùng chung
            var account = _accountStore.Authenticate(
                request.Username.Trim(),
                request.Password);

            if (account == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Sai tài khoản, mật khẩu hoặc tài khoản đã bị khóa."
                });
            }

            // Tạo JWT
            string token = GenerateJwtToken(
                account.Username,
                account.Role);

            // Giữ nguyên cấu trúc JSON cho WinForms
            return Ok(new
            {
                success = true,
                token,
                username = account.Username,
                fullName = account.FullName,
                role = account.Role
            });
        }

        // Tạo JWT với thời hạn 2 giờ
        private string GenerateJwtToken(string username, string role)
        {
            string secret = _configuration["JwtSettings:Secret"]
                ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!";

            byte[] key = Encoding.ASCII.GetBytes(secret);

            var handler = new JwtSecurityTokenHandler();

            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            return handler.WriteToken(handler.CreateToken(descriptor));
        }
    }

    // DTO nhận thông tin đăng nhập
    public class LoginRequestDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
