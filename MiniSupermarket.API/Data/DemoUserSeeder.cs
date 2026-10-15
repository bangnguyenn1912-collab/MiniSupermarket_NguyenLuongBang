
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    // Khởi tạo tài khoản mẫu vào SQL Server.
    // Không xóa hoặc cập nhật tài khoản đã tồn tại.
    public static class DemoUserSeeder
    {
        public static void Seed(SupermarketDbContext context)
        {
            // ==========================================
            // DANH SÁCH 17 TÀI KHOẢN MẪU
            // ==========================================

            var accounts = new List<(
                string Username,
                string Role,
                string FullName)>
            {
                ("admin", "Admin", "Quản trị viên"),
                ("cashier", "Cashier", "Thu ngân"),

                ("admin01", "Admin", "Nguyễn Quản Trị"),
                ("admin02", "Admin", "Trần Giám Đốc"),

                ("cashier01", "Cashier", "Lê Thu Ngân"),
                ("cashier02", "Cashier", "Phạm Bán Hàng"),
                ("cashier03", "Cashier", "Hoàng Thu Ngân"),
                ("cashier04", "Cashier", "Vũ Thị Quầy"),
                ("cashier05", "Cashier", "Đỗ Bán Lẻ"),

                ("ware01", "Warehouse", "Ngô Quản Kho"),
                ("ware02", "Warehouse", "Bùi Kiểm Kê"),
                ("ware03", "Warehouse", "Dương Thủ Kho"),
                ("ware04", "Warehouse", "Lý Nhập Hàng"),

                ("admin_backup", "Admin", "Đặng Hỗ Trợ"),
                ("cashier06", "Cashier", "Hồ Ca Chiều"),
                ("ware05", "Warehouse", "Trương Vận Chuyển"),
                ("supervisor", "Admin", "Mai Giám Sát")
            };

            // Mật khẩu mẫu chỉ dành cho môi trường học tập.
            const string demoPassword = "123456";

            // ==========================================
            // LẤY TÀI KHOẢN ĐÃ CÓ TRONG SQL SERVER
            // ==========================================

            var existingUsernames = context.Users
                .AsNoTracking()
                .Select(u => u.Username)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            int added = 0;

            // ==========================================
            // CHỈ THÊM TÀI KHOẢN CHƯA TỒN TẠI
            // ==========================================

            foreach (var item in accounts)
            {
                if (existingUsernames.Contains(item.Username))
                {
                    continue;
                }

                var user = new User
                {
                    Username = item.Username,
                    FullName = item.FullName,
                    Email =
                        $"{item.Username.ToLowerInvariant()}@minisupermarket.local",
                    Role = item.Role,
                    IsActive = true,

                    // Chỉ lưu BCrypt hash, không lưu mật khẩu gốc.
                    PasswordHash =
                        BCrypt.Net.BCrypt.HashPassword(demoPassword)
                };

                context.Users.Add(user);

                existingUsernames.Add(item.Username);

                added++;
            }

            // ==========================================
            // LƯU VÀO SQL SERVER
            // ==========================================

            if (added > 0)
            {
                context.SaveChanges();
            }

            Console.WriteLine(
                $"DemoUserSeeder: Đã thêm {added} tài khoản mẫu.");
        }
    }
}
