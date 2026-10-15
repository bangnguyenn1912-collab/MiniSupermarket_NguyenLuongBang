
using System.ComponentModel.DataAnnotations; // Sử dụng thư viện kiểm tra dữ liệu đầu vào

namespace MiniSupermarket.API.Models // Khai báo namespace chứa các Model
{ // Bắt đầu namespace
    public class ProductCreateRequest // Khai báo lớp nhận dữ liệu thêm sản phẩm
    { // Bắt đầu lớp ProductCreateRequest
        [Required(ErrorMessage = "Mã vạch không được để trống")] // Bắt buộc nhập mã vạch
        [StringLength(50)] // Giới hạn mã vạch tối đa 50 ký tự
        public string Barcode { get; set; } = string.Empty; // Lưu mã vạch sản phẩm

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")] // Bắt buộc nhập tên sản phẩm
        [StringLength(150)] // Giới hạn tên sản phẩm tối đa 150 ký tự
        public string ProductName { get; set; } = string.Empty; // Lưu tên sản phẩm

        [Range(typeof(decimal), "0", "9999999999999999.99", ErrorMessage = "Giá bán không hợp lệ")] // Kiểm tra giá phù hợp decimal(18,2)
        public decimal Price { get; set; } // Lưu giá bán sản phẩm

        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không được âm")] // Kiểm tra số lượng không âm
        public int StockQuantity { get; set; } // Lưu số lượng tồn kho

        [Range(1, int.MaxValue, ErrorMessage = "Mã danh mục phải lớn hơn 0")] // Kiểm tra mã danh mục hợp lệ
        public int CategoryId { get; set; } // Lưu mã danh mục sản phẩm
    } // Kết thúc lớp ProductCreateRequest
} // Kết thúc namespace
