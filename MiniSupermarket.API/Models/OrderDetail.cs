
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniSupermarket.API.Models
{
    // Model lưu chi tiết từng sản phẩm trong hóa đơn
    [Table("OrderDetails")]
    public class OrderDetail
    {
        // Mã chi tiết hóa đơn, tự động tăng
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        // Mã hóa đơn liên kết đến bảng Orders
        [Required]
        public int OrderId { get; set; }

        // Thuộc tính điều hướng đến hóa đơn
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } = null!;

        // Mã sản phẩm liên kết đến bảng Products
        [Required]
        public int ProductId { get; set; }

        // Thuộc tính điều hướng đến sản phẩm
        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        // Số lượng sản phẩm đã bán
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        // Giá bán tại thời điểm thanh toán
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        // Thành tiền = Số lượng * Đơn giá
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }
    }
}
