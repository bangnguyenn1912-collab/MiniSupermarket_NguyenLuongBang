
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Tài khoản người dùng được lưu trong SQL Server.
    [Table("Users")]
    public class User
    {
        // Khóa chính, tự động tăng.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        // Tên đăng nhập, không được để trống.
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        // Họ và tên người dùng.
        [Required]
        [StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        // Địa chỉ email.
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        // Vai trò: Admin, Cashier hoặc Warehouse.
        [Required]
        [StringLength(30)]
        public string Role { get; set; } = string.Empty;

        // Trạng thái hoạt động của tài khoản.
        public bool IsActive { get; set; } = true;

        // Chỉ lưu mật khẩu đã được băm, không lưu mật khẩu gốc.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
