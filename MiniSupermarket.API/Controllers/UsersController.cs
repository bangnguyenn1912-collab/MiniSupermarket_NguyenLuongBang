
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniSupermarket.API.Services;
using System.Net.Mail;
using System.Security.Claims;

namespace MiniSupermarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")] // Chỉ Admin quản lý tài khoản
    public class UsersController : ControllerBase
    {
        private readonly DemoAccountStore _store;

        public UsersController(DemoAccountStore store)
        {
            _store = store;
        }

        // Kiểm tra người gọi vẫn là Admin đang hoạt động
        private bool IsCurrentAdmin()
        {
            string username = User.FindFirstValue(ClaimTypes.Name) ?? "";
            return _store.IsActiveWithRole(username, "Admin");
        }

        // GET /api/Users
        [HttpGet]
        public IActionResult GetAll()
        {
            if (!IsCurrentAdmin()) return Forbid();

            // Không trả mật khẩu hoặc hash
            return Ok(_store.GetAll());
        }

        // POST /api/Users
        [HttpPost]
        public IActionResult Create([FromBody] CreateUserRequest request)
        {
            if (!IsCurrentAdmin()) return Forbid();

            if (request == null ||
                string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Vui lòng nhập đầy đủ thông tin.");
            }

            string username = request.Username.Trim();
            string fullName = request.FullName.Trim();
            string email = string.IsNullOrWhiteSpace(request.Email)
                ? BuildDefaultEmail(username)
                : request.Email.Trim();

            if (username.Length > 50 || fullName.Length > 150 ||
                username.Any(char.IsWhiteSpace))
            {
                return BadRequest("Tên đăng nhập hoặc họ tên không hợp lệ.");
            }

            if (request.Password.Length < 8)
            {
                return BadRequest("Mật khẩu phải có ít nhất 8 ký tự.");
            }

            if (!IsValidEmail(email))
                return BadRequest("Email không hợp lệ.");

            string? role = NormalizeRole(request.Role);

            if (role == null)
                return BadRequest("Vai trò không hợp lệ.");

            var user = _store.Add(
                username,
                request.Password,
                fullName,
                email,
                role);

            if (user == null)
                return Conflict("Tên đăng nhập đã tồn tại.");

            return StatusCode(201, user);
        }

        // PUT /api/Users/{id}
        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] UpdateUserRequest request)
        {
            if (!IsCurrentAdmin()) return Forbid();

            if (request == null ||
                string.IsNullOrWhiteSpace(request.FullName))
            {
                return BadRequest("Họ tên không được để trống.");
            }

            string fullName = request.FullName.Trim();

            if (fullName.Length > 150)
                return BadRequest("Họ tên quá dài.");

            string? role = NormalizeRole(request.Role);

            if (role == null)
                return BadRequest("Vai trò không hợp lệ.");

            var target = _store.GetAll().FirstOrDefault(x => x.Id == id);

            if (target == null)
                return NotFound("Không tìm thấy tài khoản.");

            string email = string.IsNullOrWhiteSpace(request.Email)
                ? target.Email
                : request.Email.Trim();

            if (!IsValidEmail(email))
                return BadRequest("Email không hợp lệ.");

            // Không cho Admin tự hạ quyền của chính mình
            string currentUsername =
                User.FindFirstValue(ClaimTypes.Name) ?? "";

            if (target.Username.Equals(
                    currentUsername,
                    StringComparison.OrdinalIgnoreCase) &&
                role != "Admin")
            {
                return BadRequest("Không thể tự hạ quyền Admin.");
            }

            // Không cho hạ quyền Admin cuối cùng
            if (target.Role == "Admin" && role != "Admin" &&
                _store.GetAll().Count(x =>
                    x.Role == "Admin" && x.IsActive) <= 1)
            {
                return BadRequest("Phải giữ ít nhất một Admin hoạt động.");
            }

            _store.Update(id, fullName, email, role);
            return NoContent();
        }

        // PUT /api/Users/{id}/password
        [HttpPut("{id:int}/password")]
        public IActionResult ResetPassword(
            int id,
            [FromBody] ResetPasswordRequest request)
        {
            if (!IsCurrentAdmin()) return Forbid();

            if (request == null ||
                string.IsNullOrWhiteSpace(request.NewPassword) ||
                request.NewPassword.Length < 8)
            {
                return BadRequest("Mật khẩu mới phải có ít nhất 8 ký tự.");
            }

            if (!_store.ResetPassword(id, request.NewPassword))
                return NotFound("Không tìm thấy tài khoản.");

            return NoContent();
        }

        // PUT /api/Users/{id}/status
        [HttpPut("{id:int}/status")]
        public IActionResult ChangeStatus(
            int id,
            [FromBody] ChangeUserStatusRequest request)
        {
            if (!IsCurrentAdmin()) return Forbid();

            if (request == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            var target = _store.GetAll().FirstOrDefault(x => x.Id == id);

            if (target == null)
                return NotFound("Không tìm thấy tài khoản.");

            string currentUsername =
                User.FindFirstValue(ClaimTypes.Name) ?? "";

            // Không tự khóa tài khoản đang sử dụng
            if (!request.IsActive &&
                target.Username.Equals(
                    currentUsername,
                    StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Không thể tự khóa tài khoản đang đăng nhập.");
            }

            // Phải giữ ít nhất một Admin hoạt động
            if (!request.IsActive && target.Role == "Admin" &&
                _store.GetAll().Count(x =>
                    x.Role == "Admin" && x.IsActive) <= 1)
            {
                return BadRequest("Không thể khóa Admin cuối cùng.");
            }

            _store.SetActive(id, request.IsActive);
            return NoContent();
        }

        // Chuẩn hóa vai trò
        private static string? NormalizeRole(string? role)
        {
            return role?.Trim().ToUpperInvariant() switch
            {
                "ADMIN" => "Admin",
                "CASHIER" => "Cashier",
                "WAREHOUSE" => "Warehouse",
                _ => null
            };
        }

        // Kiểm tra email ở API để không phụ thuộc riêng vào giao diện WinForms.
        private static bool IsValidEmail(string email)
        {
            return email.Length <= 150 &&
                   MailAddress.TryCreate(email, out var parsed) &&
                   parsed.Address.Equals(
                       email,
                       StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildDefaultEmail(string username)
        {
            return $"{username.ToLowerInvariant()}@minisupermarket.local";
        }
    }

    public class CreateUserRequest
    {
        public string Username { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class UpdateUserRequest
    {
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
    }

    public class ResetPasswordRequest
    {
        public string NewPassword { get; set; } = "";
    }

    public class ChangeUserStatusRequest
    {
        public bool IsActive { get; set; }
    }
}
