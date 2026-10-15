
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MiniSupermarket.API.Data;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Controllers
{
    // ==========================================
    // API QUẢN LÝ HÓA ĐƠN VÀ THANH TOÁN POS
    // ==========================================

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Cashier")]
    public class OrdersController : ControllerBase
    {
        private readonly SupermarketDbContext _context;

        // Khởi tạo Controller
        public OrdersController(SupermarketDbContext context)
        {
            _context = context;
        }

        // ==========================================
        // POST: api/Orders/checkout
        // THANH TOÁN HÓA ĐƠN
        // ==========================================

        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(
            [FromBody] CheckoutRequest request)
        {
            // ------------------------------------------
            // 1. KIỂM TRA DỮ LIỆU ĐẦU VÀO
            // ------------------------------------------

            if (request == null)
            {
                return BadRequest(new
                {
                    message = "Dữ liệu thanh toán không hợp lệ."
                });
            }

            if (request.Items == null ||
                request.Items.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Giỏ hàng đang trống."
                });
            }

            if (request.CashReceived < 0)
            {
                return BadRequest(new
                {
                    message = "Tiền khách đưa không hợp lệ."
                });
            }

            if (!string.IsNullOrWhiteSpace(
                request.CustomerPhone) &&
                request.CustomerPhone.Length > 20)
            {
                return BadRequest(new
                {
                    message = "Số điện thoại tối đa 20 ký tự."
                });
            }

            // Kiểm tra ProductId và Quantity
            foreach (var item in request.Items)
            {
                if (item.ProductId <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Mã sản phẩm không hợp lệ."
                    });
                }

                if (item.Quantity <= 0)
                {
                    return BadRequest(new
                    {
                        message = "Số lượng sản phẩm phải lớn hơn 0."
                    });
                }
            }

            // ------------------------------------------
            // 2. LẤY TÊN NHÂN VIÊN TỪ JWT
            // ------------------------------------------

            string? cashierUsername =
                User.FindFirst(ClaimTypes.Name)?.Value
                ?? User.FindFirst("unique_name")?.Value
                ?? User.FindFirst("name")?.Value;

            if (string.IsNullOrWhiteSpace(cashierUsername))
            {
                return Unauthorized(new
                {
                    message = "Không xác định được nhân viên đăng nhập."
                });
            }

            if (cashierUsername.Length > 50)
            {
                return BadRequest(new
                {
                    message = "Tên nhân viên vượt quá 50 ký tự."
                });
            }

            // ------------------------------------------
            // 3. GỘP SẢN PHẨM TRÙNG ID
            // ------------------------------------------

            List<CheckoutItem> cartItems;

            try
            {
                cartItems = request.Items
                    .GroupBy(x => x.ProductId)
                    .Select(group => new CheckoutItem
                    {
                        ProductId = group.Key,
                        Quantity = checked(
                            group.Sum(x => (long)x.Quantity)
                        ) > int.MaxValue
                            ? throw new OverflowException()
                            : (int)group.Sum(
                                x => (long)x.Quantity)
                    })
                    .OrderBy(x => x.ProductId)
                    .ToList();
            }
            catch (OverflowException)
            {
                return BadRequest(new
                {
                    message = "Số lượng sản phẩm quá lớn."
                });
            }

            // ------------------------------------------
            // 4. BẮT ĐẦU TRANSACTION
            // ------------------------------------------

            await using var transaction =
                await _context.Database
                    .BeginTransactionAsync(
                        IsolationLevel.Serializable);

            try
            {
                decimal totalAmount = 0;

                var orderDetails = new List<OrderDetail>();

                // --------------------------------------
                // 5. KIỂM TRA SẢN PHẨM VÀ TỒN KHO
                // --------------------------------------

                foreach (var cartItem in cartItems)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(
                            p => p.ProductId ==
                                 cartItem.ProductId);

                    if (product == null)
                    {
                        await transaction.RollbackAsync();

                        return NotFound(new
                        {
                            message =
                                $"Không tìm thấy sản phẩm ID " +
                                $"{cartItem.ProductId}."
                        });
                    }

                    if (product.StockQuantity <
                        cartItem.Quantity)
                    {
                        await transaction.RollbackAsync();

                        // HTTP 409 cho biết dữ liệu tồn kho đã thay đổi hoặc
                        // không còn đủ để hoàn thành yêu cầu hiện tại.
                        return Conflict(new
                        {
                            message =
                                $"Sản phẩm '{product.ProductName}' " +
                                $"chỉ còn {product.StockQuantity} " +
                                $"trong kho."
                        });
                    }

                    if (product.Price < 0)
                    {
                        await transaction.RollbackAsync();

                        return BadRequest(new
                        {
                            message =
                                $"Giá sản phẩm '{product.ProductName}' " +
                                "không hợp lệ."
                        });
                    }

                    // ----------------------------------
                    // 6. TÍNH TIỀN THEO GIÁ DATABASE
                    // ----------------------------------

                    decimal unitPrice = product.Price;

                    decimal lineTotal = checked(
                        unitPrice * cartItem.Quantity
                    );

                    totalAmount = checked(
                        totalAmount + lineTotal
                    );

                    // ----------------------------------
                    // 7. TRỪ SỐ LƯỢNG TỒN KHO
                    // ----------------------------------

                    product.StockQuantity -=
                        cartItem.Quantity;

                    // EF Core theo dõi product và
                    // cập nhật khi SaveChangesAsync()

                    // ----------------------------------
                    // 8. TẠO CHI TIẾT HÓA ĐƠN
                    // ----------------------------------

                    orderDetails.Add(new OrderDetail
                    {
                        ProductId = product.ProductId,
                        Quantity = cartItem.Quantity,
                        UnitPrice = unitPrice,
                        TotalPrice = lineTotal
                    });
                }

                // --------------------------------------
                // 9. KIỂM TRA TIỀN KHÁCH ĐƯA
                // --------------------------------------

                if (request.CashReceived < totalAmount)
                {
                    await transaction.RollbackAsync();

                    return BadRequest(new
                    {
                        message = "Tiền khách đưa chưa đủ.",
                        totalAmount = totalAmount,
                        cashReceived = request.CashReceived,
                        amountMissing =
                            totalAmount - request.CashReceived
                    });
                }

                decimal changeAmount =
                    request.CashReceived - totalAmount;

                // --------------------------------------
                // 10. TẠO HÓA ĐƠN
                // --------------------------------------

                var order = new Order
                {
                    OrderDate = DateTime.UtcNow,

                    CashierUsername = cashierUsername,

                    CustomerPhone =
                        string.IsNullOrWhiteSpace(
                            request.CustomerPhone)
                            ? null
                            : request.CustomerPhone.Trim(),

                    TotalAmount = totalAmount,

                    CashReceived = request.CashReceived,

                    ChangeAmount = changeAmount,

                    OrderDetails = orderDetails
                };

                // --------------------------------------
                // 11. LƯU HÓA ĐƠN VÀ TRỪ KHO
                // --------------------------------------

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                // --------------------------------------
                // 12. XÁC NHẬN TRANSACTION
                // --------------------------------------

                await transaction.CommitAsync();

                // --------------------------------------
                // 13. TRẢ KẾT QUẢ CHO WINFORMS
                // --------------------------------------

                return Ok(new
                {
                    success = true,

                    message = "Thanh toán thành công.",

                    orderId = order.OrderId,

                    orderDate = order.OrderDate,

                    cashierUsername =
                        order.CashierUsername,

                    customerPhone =
                        order.CustomerPhone,

                    totalAmount =
                        order.TotalAmount,

                    cashReceived =
                        order.CashReceived,

                    changeAmount =
                        order.ChangeAmount,

                    items = orderDetails.Select(d => new
                    {
                        productId = d.ProductId,
                        quantity = d.Quantity,
                        unitPrice = d.UnitPrice,
                        totalPrice = d.TotalPrice
                    }).ToList()
                });
            }
            catch (OverflowException)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message =
                        "Tổng tiền vượt quá giới hạn cho phép."
                });
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message =
                        "Không thể lưu hóa đơn vào cơ sở dữ liệu."
                });
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();

                return StatusCode(500, new
                {
                    message =
                        "Có lỗi xảy ra khi thanh toán."
                });
            }
        }

        // ==========================================
        // GET: api/Orders
        // XEM DANH SÁCH HÓA ĐƠN
        // ==========================================

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .AsNoTracking()
                .OrderByDescending(o => o.OrderId)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.CashierUsername,
                    o.CustomerPhone,
                    o.TotalAmount,
                    o.CashReceived,
                    o.ChangeAmount
                })
                .ToListAsync();

            return Ok(orders);
        }

        // ==========================================
        // GET: api/Orders/quick-report?date=2026-10-15
        // BÁO CÁO NHANH THEO NGÀY - CHỈ ADMIN
        // ==========================================

        [HttpGet("quick-report")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetQuickReport(
            [FromQuery] DateTime date)
        {
            if (date == default)
            {
                return BadRequest(new
                {
                    message = "Vui lòng chọn ngày báo cáo hợp lệ."
                });
            }

            DateTime startDate = date.Date;
            DateTime endDate = startDate.AddDays(1);

            // Chỉ đọc dữ liệu hóa đơn, không phát sinh thay đổi database.
            var orders = await _context.Orders
                .AsNoTracking()
                .Where(o =>
                    o.OrderDate >= startDate &&
                    o.OrderDate < endDate)
                .OrderByDescending(o => o.OrderDate)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.CashierUsername,
                    o.TotalAmount
                })
                .ToListAsync();

            decimal totalRevenue = orders.Sum(o => o.TotalAmount);

            var bestSeller = await _context.OrderDetails
                .AsNoTracking()
                .Where(d =>
                    d.Order.OrderDate >= startDate &&
                    d.Order.OrderDate < endDate)
                .GroupBy(d => new
                {
                    d.ProductId,
                    d.Product.ProductName
                })
                .Select(group => new
                {
                    group.Key.ProductId,
                    group.Key.ProductName,
                    Quantity = group.Sum(item => item.Quantity)
                })
                .OrderByDescending(item => item.Quantity)
                .ThenBy(item => item.ProductName)
                .FirstOrDefaultAsync();

            return Ok(new
            {
                ReportDate = startDate,
                TotalOrders = orders.Count,
                TotalRevenue = totalRevenue,
                BestSeller = bestSeller?.ProductName,
                BestSellerQuantity = bestSeller?.Quantity ?? 0,
                Orders = orders
            });
        }

        // ==========================================
        // GET: api/Orders/5
        // XEM CHI TIẾT MỘT HÓA ĐƠN
        // ==========================================

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .AsNoTracking()
                .Where(o => o.OrderId == id)
                .Select(o => new
                {
                    o.OrderId,
                    o.OrderDate,
                    o.CashierUsername,
                    o.CustomerPhone,
                    o.TotalAmount,
                    o.CashReceived,
                    o.ChangeAmount,

                    Details = o.OrderDetails
                        .Select(d => new
                        {
                            d.OrderDetailId,
                            d.ProductId,
                            ProductName =
                                d.Product.ProductName,
                            d.Quantity,
                            d.UnitPrice,
                            d.TotalPrice
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy hóa đơn."
                });
            }

            return Ok(order);
        }
    }

    // ==========================================
    // DTO NHẬN DỮ LIỆU THANH TOÁN TỪ WINFORMS
    // ==========================================

    public class CheckoutRequest
    {
        // Số điện thoại khách hàng
        public string? CustomerPhone { get; set; }

        // Tiền khách đưa
        [Range(typeof(decimal), "0", "9999999999999999")]
        public decimal CashReceived { get; set; }

        // Danh sách sản phẩm cần thanh toán
        [Required]
        [MinLength(1)]
        public List<CheckoutItem> Items { get; set; }
            = new List<CheckoutItem>();
    }

    // ==========================================
    // DTO SẢN PHẨM TRONG GIỎ HÀNG
    // ==========================================

    public class CheckoutItem
    {
        // Mã sản phẩm
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        // Số lượng mua
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
