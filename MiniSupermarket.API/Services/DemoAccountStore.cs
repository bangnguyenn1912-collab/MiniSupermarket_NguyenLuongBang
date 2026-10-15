
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Services
{
    // ==========================================
    // THÔNG TIN TÀI KHOẢN TRẢ VỀ API
    // KHÔNG TRẢ VỀ PASSWORD HASH
    // ==========================================

    public class DemoUserInfo
    {
        public int Id { get; set; }

        public string Username { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Email { get; set; } = "";

        public string Role { get; set; } = "";

        public bool IsActive { get; set; }
    }

    // ==========================================
    // THÔNG TIN ĐĂNG NHẬP
    // ==========================================

    public class DemoLoginInfo
    {
        public string Username { get; set; } = "";

        public string FullName { get; set; } = "";

        public string Role { get; set; } = "";
    }

    // ==========================================
    // DỊCH VỤ QUẢN LÝ TÀI KHOẢN SQL SERVER
    // ==========================================

    public class DemoAccountStore
    {
        private readonly SupermarketDbContext _context;

        // Dependency Injection DbContext.
        public DemoAccountStore(
            SupermarketDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // CHUYỂN USER ENTITY SANG USER INFO
        // ==========================================

        private static DemoUserInfo ToInfo(User account)
        {
            return new DemoUserInfo
            {
                Id = account.Id,
                Username = account.Username,
                FullName = account.FullName,
                Email = account.Email,
                Role = account.Role,
                IsActive = account.IsActive
            };
        }

        // ==========================================
        // XÁC THỰC TÀI KHOẢN
        // ==========================================

        public DemoLoginInfo? Authenticate(
            string username,
            string password)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrEmpty(password))
            {
                return null;
            }

            username = username.Trim();

            var account = _context.Users
                .AsNoTracking()
                .FirstOrDefault(u =>
                    u.Username == username);

            // Không tìm thấy hoặc tài khoản bị khóa.
            if (account == null || !account.IsActive)
            {
                return null;
            }

            // Kiểm tra mật khẩu BCrypt.
            if (!BCrypt.Net.BCrypt.Verify(
                password,
                account.PasswordHash))
            {
                return null;
            }

            return new DemoLoginInfo
            {
                Username = account.Username,
                FullName = account.FullName,
                Role = account.Role
            };
        }

        // ==========================================
        // LẤY DANH SÁCH TÀI KHOẢN
        // ==========================================

        public List<DemoUserInfo> GetAll()
        {
            return _context.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .Select(u => new DemoUserInfo
                {
                    Id = u.Id,
                    Username = u.Username,
                    FullName = u.FullName,
                    Email = u.Email,
                    Role = u.Role,
                    IsActive = u.IsActive
                })
                .ToList();
        }

        // ==========================================
        // KIỂM TRA TÀI KHOẢN VÀ VAI TRÒ
        // PHIÊN BẢN ĐỒNG BỘ
        // ==========================================

        public bool IsActiveWithRole(
            string username,
            string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return _context.Users
                .AsNoTracking()
                .Any(u =>
                    u.Username == username &&
                    u.IsActive &&
                    u.Role == role);
        }

        // ==========================================
        // KIỂM TRA TÀI KHOẢN VÀ VAI TRÒ
        // PHIÊN BẢN ASYNC DÙNG CHO JWT
        // ==========================================

        public async Task<bool> IsActiveWithRoleAsync(
            string username,
            string role)
        {
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u =>
                    u.Username == username &&
                    u.IsActive &&
                    u.Role == role);
        }

        // ==========================================
        // THÊM TÀI KHOẢN MỚI
        // ==========================================

        public DemoUserInfo? Add(
            string username,
            string password,
            string fullName,
            string email,
            string role)
        {
            username = username?.Trim() ?? "";
            fullName = fullName?.Trim() ?? "";
            email = email?.Trim() ?? "";
            role = role?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(role))
            {
                return null;
            }

            // Giới hạn độ dài theo cấu trúc bảng Users.
            if (username.Length > 50 ||
                fullName.Length > 150 ||
                email.Length > 255 ||
                role.Length > 30)
            {
                return null;
            }

            // Kiểm tra trùng tên đăng nhập.
            bool exists = _context.Users.Any(u =>
                u.Username == username);

            if (exists)
            {
                return null;
            }

            var account = new User
            {
                Username = username,
                FullName = fullName,
                Email = email,
                Role = role,
                IsActive = true,

                // Không lưu mật khẩu gốc.
                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(account);

            // Lưu vào SQL Server.
            _context.SaveChanges();

            return ToInfo(account);
        }

        // ==========================================
        // ĐẶT LẠI MẬT KHẨU
        // ==========================================

        public bool ResetPassword(
            int id,
            string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }

            var account = _context.Users
                .FirstOrDefault(u => u.Id == id);

            if (account == null)
            {
                return false;
            }

            account.PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(newPassword);

            _context.SaveChanges();

            return true;
        }

        // ==========================================
        // KHÓA / MỞ KHÓA TÀI KHOẢN
        // ==========================================

        public bool SetActive(
            int id,
            bool isActive)
        {
            var account = _context.Users
                .FirstOrDefault(u => u.Id == id);

            if (account == null)
            {
                return false;
            }

            account.IsActive = isActive;

            _context.SaveChanges();

            return true;
        }

        // ==========================================
        // CẬP NHẬT THÔNG TIN TÀI KHOẢN
        // ==========================================

        public bool Update(
            int id,
            string fullName,
            string email,
            string role)
        {
            fullName = fullName?.Trim() ?? "";
            email = email?.Trim() ?? "";
            role = role?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            if (fullName.Length > 150 ||
                email.Length > 255 ||
                role.Length > 30)
            {
                return false;
            }

            var account = _context.Users
                .FirstOrDefault(u => u.Id == id);

            if (account == null)
            {
                return false;
            }

            account.FullName = fullName;
            account.Email = email;
            account.Role = role;

            _context.SaveChanges();

            return true;
        }

        // ==========================================
        // EMAIL MẶC ĐỊNH CHO TÀI KHOẢN MẪU
        // SẼ DÙNG Ở BƯỚC 4.4
        // ==========================================

        public static string BuildDefaultEmail(
            string username)
        {
            return $"{username.ToLowerInvariant()}@minisupermarket.local";
        }
    }
}
