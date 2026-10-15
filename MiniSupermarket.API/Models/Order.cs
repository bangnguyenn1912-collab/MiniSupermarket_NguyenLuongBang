
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Bảng lưu thông tin hóa đơn bán hàng
    [Table("Orders")]
    public class Order
    {
        // Mã hóa đơn tự tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        // Ngày giờ tạo hóa đơn (UTC)
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // Tên đăng nhập của nhân viên thanh toán
        [Required]
        [StringLength(50)]
        public string CashierUsername { get; set; } = string.Empty;

        // Số điện thoại khách hàng, có thể để trống
        [StringLength(20)]
        public string? CustomerPhone { get; set; }

        // Tổng tiền của hóa đơn
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // Số tiền khách đưa
        [Column(TypeName = "decimal(18,2)")]
        public decimal CashReceived { get; set; }

        // Số tiền trả lại khách
        [Column(TypeName = "decimal(18,2)")]
        public decimal ChangeAmount { get; set; }

        // Một hóa đơn có nhiều dòng chi tiết
        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}
