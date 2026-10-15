
using Microsoft.AspNetCore.Authorization; // Sử dụng phân quyền JWT
using Microsoft.AspNetCore.Mvc; // Sử dụng ASP.NET Core Web API
using Microsoft.EntityFrameworkCore; // Sử dụng Entity Framework Core
using MiniSupermarket.API.Data; // Sử dụng SupermarketDbContext
using MiniSupermarket.API.Models; // Sử dụng Model Product

namespace MiniSupermarket.API.Controllers // Namespace Controller
{
    [ApiController] // Đánh dấu lớp là API Controller
    [Route("api/[controller]")] // Đường dẫn gốc: api/Products
    [Authorize] // Tất cả API đều yêu cầu đăng nhập bằng JWT
    public class ProductsController : ControllerBase // Controller quản lý sản phẩm
    {
        private readonly SupermarketDbContext _context; // Khai báo DbContext

        public ProductsController(SupermarketDbContext context) // Constructor
        {
            _context = context; // Gán DbContext
        }

        // ==================== API 1: DANH SÁCH SẢN PHẨM ====================

        [HttpGet] // GET /api/Products
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> GetProducts(
            [FromQuery] int? categoryId) // Mã danh mục tùy chọn
        {
            var query = _context.Products.AsNoTracking(); // Truy vấn chỉ đọc

            if (categoryId.HasValue) // Kiểm tra có lọc danh mục
            {
                if (categoryId.Value <= 0) // Kiểm tra ID danh mục
                {
                    return BadRequest("Mã danh mục phải lớn hơn 0."); // HTTP 400
                }

                query = query.Where(
                    p => p.CategoryId == categoryId.Value); // Lọc danh mục
            }

            var products = await query // Thực hiện truy vấn
                .Select(p => new // Chọn dữ liệu trả về
                {
                    p.ProductId, // Mã sản phẩm
                    p.Barcode, // Mã vạch
                    p.ProductName, // Tên sản phẩm
                    p.Price, // Giá bán
                    p.StockQuantity, // Số lượng tồn kho
                    p.CategoryId, // Mã danh mục
                    CategoryName = _context.Categories // Truy vấn danh mục
                        .Where(c => c.CategoryId == p.CategoryId) // Lọc danh mục
                        .Select(c => c.CategoryName) // Lấy tên danh mục
                        .FirstOrDefault() // Lấy kết quả đầu tiên
                })
                .ToListAsync(); // Lấy danh sách bất đồng bộ

            return Ok(products); // HTTP 200
        }

        // ==================== API 2: TÌM KIẾM SẢN PHẨM ====================

        [HttpGet("search")] // GET /api/Products/search
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string? keyword, // Từ khóa tìm kiếm
            [FromQuery] int? categoryId) // Danh mục tùy chọn
        {
            var query = _context.Products.AsNoTracking(); // Truy vấn chỉ đọc

            if (!string.IsNullOrWhiteSpace(keyword)) // Có từ khóa
            {
                string searchText = keyword.Trim(); // Xóa khoảng trắng

                query = query.Where(p =>
                    p.ProductName.Contains(searchText) || // Tìm theo tên
                    p.Barcode.Contains(searchText)); // Tìm theo Barcode
            }

            if (categoryId.HasValue) // Có lọc danh mục
            {
                if (categoryId.Value <= 0) // Kiểm tra danh mục
                {
                    return BadRequest("Mã danh mục phải lớn hơn 0."); // HTTP 400
                }

                query = query.Where(
                    p => p.CategoryId == categoryId.Value); // Lọc danh mục
            }

            var products = await query // Thực hiện truy vấn
                .Select(p => new // Chọn dữ liệu
                {
                    p.ProductId, // Mã sản phẩm
                    p.Barcode, // Mã vạch
                    p.ProductName, // Tên sản phẩm
                    p.Price, // Giá bán
                    p.StockQuantity, // Số lượng tồn
                    p.CategoryId, // Mã danh mục
                    CategoryName = _context.Categories // Truy vấn danh mục
                        .Where(c => c.CategoryId == p.CategoryId) // Lọc danh mục
                        .Select(c => c.CategoryName) // Lấy tên danh mục
                        .FirstOrDefault() // Lấy kết quả đầu tiên
                })
                .ToListAsync(); // Lấy danh sách

            return Ok(products); // HTTP 200
        }

        // ==================== API 3: CHI TIẾT SẢN PHẨM THEO ID ====================

        [HttpGet("{id:int}")] // GET /api/Products/{id}
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> GetProductById(int id) // Nhận ID sản phẩm
        {
            if (id <= 0) // Kiểm tra ID hợp lệ
            {
                return BadRequest("Mã sản phẩm phải lớn hơn 0."); // HTTP 400
            }

            var product = await _context.Products // Truy vấn sản phẩm
                .AsNoTracking() // Không theo dõi thay đổi
                .Where(p => p.ProductId == id) // Tìm theo ID
                .Select(p => new // Chọn dữ liệu
                {
                    p.ProductId, // Mã sản phẩm
                    p.Barcode, // Mã vạch
                    p.ProductName, // Tên sản phẩm
                    p.Price, // Giá bán
                    p.StockQuantity, // Tồn kho
                    p.CategoryId, // Mã danh mục
                    CategoryName = _context.Categories // Truy vấn danh mục
                        .Where(c => c.CategoryId == p.CategoryId) // Lọc danh mục
                        .Select(c => c.CategoryName) // Lấy tên danh mục
                        .FirstOrDefault() // Lấy kết quả đầu tiên
                })
                .FirstOrDefaultAsync(); // Lấy một sản phẩm

            if (product == null) // Không tìm thấy
            {
                return NotFound(
                    "Không tìm thấy sản phẩm có ID = " + id); // HTTP 404
            }

            return Ok(product); // HTTP 200
        }

        // ==================== API 4: TÌM SẢN PHẨM THEO BARCODE ====================

        [HttpGet("barcode/{barcode}")] // GET /api/Products/barcode/{barcode}
        [Authorize(Roles = "Admin,Cashier,Warehouse")] // Cho phép cả Cashier
        public async Task<IActionResult> GetProductByBarcode(
            string barcode) // Nhận mã vạch sản phẩm
        {
            if (string.IsNullOrWhiteSpace(barcode)) // Kiểm tra mã vạch
            {
                return BadRequest("Mã vạch không được để trống."); // HTTP 400
            }

            string barcodeValue = barcode.Trim(); // Chuẩn hóa mã vạch

            if (barcodeValue.Length > 50) // Kiểm tra độ dài
            {
                return BadRequest(
                    "Mã vạch không được vượt quá 50 ký tự."); // HTTP 400
            }

            var product = await _context.Products // Truy vấn sản phẩm
                .AsNoTracking() // Chỉ đọc dữ liệu
                .Where(p => p.Barcode == barcodeValue) // Tìm đúng Barcode
                .Select(p => new // Chọn thông tin
                {
                    p.ProductId, // Mã sản phẩm
                    p.Barcode, // Mã vạch
                    p.ProductName, // Tên sản phẩm
                    p.Price, // Giá bán
                    p.StockQuantity, // Số lượng tồn kho
                    p.CategoryId, // Mã danh mục
                    CategoryName = _context.Categories // Truy vấn danh mục
                        .Where(c => c.CategoryId == p.CategoryId) // Tìm danh mục
                        .Select(c => c.CategoryName) // Lấy tên danh mục
                        .FirstOrDefault() // Lấy kết quả đầu tiên
                })
                .FirstOrDefaultAsync(); // Lấy một sản phẩm

            if (product == null) // Không tìm thấy Barcode
            {
                return NotFound(
                    "Không tìm thấy sản phẩm có mã vạch: " + barcodeValue); // HTTP 404
            }

            return Ok(product); // HTTP 200
        }

        // ==================== API 5: THÊM SẢN PHẨM ====================

        [HttpPost] // POST /api/Products
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> CreateProduct(
            [FromBody] ProductCreateRequest request) // Dữ liệu sản phẩm
        {
            if (string.IsNullOrWhiteSpace(request.Barcode)) // Kiểm tra mã vạch
            {
                return BadRequest("Mã vạch không được để trống."); // HTTP 400
            }

            if (string.IsNullOrWhiteSpace(request.ProductName)) // Kiểm tra tên
            {
                return BadRequest("Tên sản phẩm không được để trống."); // HTTP 400
            }

            string barcode = request.Barcode.Trim(); // Chuẩn hóa Barcode
            string productName = request.ProductName.Trim(); // Chuẩn hóa tên

            if (barcode.Length > 50 || productName.Length > 150) // Kiểm tra độ dài
            {
                return BadRequest("Mã vạch hoặc tên sản phẩm quá dài."); // HTTP 400
            }

            if (request.Price < 0 ||
                request.StockQuantity < 0 ||
                request.CategoryId <= 0) // Kiểm tra giá, tồn kho, danh mục
            {
                return BadRequest(
                    "Giá, số lượng hoặc mã danh mục không hợp lệ."); // HTTP 400
            }

            bool barcodeExists = await _context.Products
                .AnyAsync(p => p.Barcode == barcode); // Kiểm tra Barcode trùng

            if (barcodeExists) // Mã vạch đã tồn tại
            {
                return Conflict("Mã vạch sản phẩm đã tồn tại."); // HTTP 409
            }

            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == request.CategoryId); // Kiểm tra danh mục

            if (!categoryExists) // Danh mục không tồn tại
            {
                return BadRequest("Danh mục sản phẩm không tồn tại."); // HTTP 400
            }

            var product = new Product // Tạo đối tượng sản phẩm
            {
                Barcode = barcode, // Mã vạch
                ProductName = productName, // Tên sản phẩm
                Price = request.Price, // Giá bán
                StockQuantity = request.StockQuantity, // Số lượng tồn
                CategoryId = request.CategoryId // Mã danh mục
            };

            _context.Products.Add(product); // Thêm vào DbContext

            await _context.SaveChangesAsync(); // Lưu vào database

            return CreatedAtAction(
                nameof(GetProductById), // Tên API lấy chi tiết
                new { id = product.ProductId }, // ID sản phẩm mới
                product); // HTTP 201
        }

        // ==================== API 6: SỬA SẢN PHẨM ====================

        [HttpPut("{id:int}")] // PUT /api/Products/{id}
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> UpdateProduct(
            int id, // ID cần sửa
            [FromBody] ProductCreateRequest request) // Dữ liệu cập nhật
        {
            if (id <= 0) // Kiểm tra ID
            {
                return BadRequest("Mã sản phẩm phải lớn hơn 0."); // HTTP 400
            }

            var product = await _context.Products.FindAsync(id); // Tìm sản phẩm

            if (product == null) // Không tìm thấy
            {
                return NotFound(
                    "Không tìm thấy sản phẩm có ID = " + id); // HTTP 404
            }

            if (string.IsNullOrWhiteSpace(request.Barcode)) // Kiểm tra Barcode
            {
                return BadRequest("Mã vạch không được để trống."); // HTTP 400
            }

            if (string.IsNullOrWhiteSpace(request.ProductName)) // Kiểm tra tên
            {
                return BadRequest("Tên sản phẩm không được để trống."); // HTTP 400
            }

            string barcode = request.Barcode.Trim(); // Chuẩn hóa mã vạch
            string productName = request.ProductName.Trim(); // Chuẩn hóa tên

            if (barcode.Length > 50 || productName.Length > 150) // Kiểm tra độ dài
            {
                return BadRequest("Mã vạch hoặc tên sản phẩm quá dài."); // HTTP 400
            }

            if (request.Price < 0 ||
                request.StockQuantity < 0 ||
                request.CategoryId <= 0) // Kiểm tra dữ liệu
            {
                return BadRequest(
                    "Giá, số lượng hoặc mã danh mục không hợp lệ."); // HTTP 400
            }

            bool barcodeExists = await _context.Products
                .AnyAsync(p =>
                    p.Barcode == barcode &&
                    p.ProductId != id); // Kiểm tra Barcode trùng sản phẩm khác

            if (barcodeExists) // Nếu bị trùng
            {
                return Conflict(
                    "Mã vạch đã được sử dụng bởi sản phẩm khác."); // HTTP 409
            }

            bool categoryExists = await _context.Categories
                .AnyAsync(c => c.CategoryId == request.CategoryId); // Kiểm tra danh mục

            if (!categoryExists) // Danh mục không tồn tại
            {
                return BadRequest("Danh mục sản phẩm không tồn tại."); // HTTP 400
            }

            product.Barcode = barcode; // Cập nhật Barcode
            product.ProductName = productName; // Cập nhật tên
            product.Price = request.Price; // Cập nhật giá
            product.StockQuantity = request.StockQuantity; // Cập nhật tồn kho
            product.CategoryId = request.CategoryId; // Cập nhật danh mục

            await _context.SaveChangesAsync(); // Lưu thay đổi

            return NoContent(); // HTTP 204
        }

        // ==================== API 7: XÓA SẢN PHẨM ====================

        [HttpDelete("{id:int}")] // DELETE /api/Products/{id}
        [Authorize(Roles = "Admin,Warehouse")] // Chỉ Admin và Warehouse
        public async Task<IActionResult> DeleteProduct(int id) // ID cần xóa
        {
            if (id <= 0) // Kiểm tra ID
            {
                return BadRequest("Mã sản phẩm phải lớn hơn 0."); // HTTP 400
            }

            var product = await _context.Products.FindAsync(id); // Tìm sản phẩm

            if (product == null) // Không tìm thấy
            {
                return NotFound(
                    "Không tìm thấy sản phẩm có ID = " + id); // HTTP 404
            }

            _context.Products.Remove(product); // Đánh dấu xóa

            try // Thực hiện lưu
            {
                await _context.SaveChangesAsync(); // Xóa khỏi database
            }
            catch (DbUpdateException) // Bắt lỗi ràng buộc dữ liệu
            {
                return Conflict(
                    "Không thể xóa sản phẩm. Sản phẩm có thể đang được sử dụng trong hóa đơn hoặc dữ liệu khác."); // HTTP 409
            }

            return NoContent(); // HTTP 204
        }
    }
}
