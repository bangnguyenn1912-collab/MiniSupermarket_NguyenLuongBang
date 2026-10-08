using Microsoft.EntityFrameworkCore;
using MiniSupermarket.API.Models;

namespace MiniSupermarket.API.Data
{
    // DbContext đại diện cho phiên làm việc với cơ sở dữ liệu SQL Server
    public class SupermarketDbContext : DbContext
    {
        public SupermarketDbContext(
            DbContextOptions<SupermarketDbContext> options
        ) : base(options)
        {
        }

        // ==========================================
        // KHAI BÁO CÁC BẢNG DỮ LIỆU
        // ==========================================

        public DbSet<Category> Categories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }


        // ==========================================
            // CẤU HÌNH DỮ LIỆU MỒI (CIRCLE K EXPRESS)
        // ==========================================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ==========================================
            // SEED 15 CATEGORY
            // DANH MỤC SẢN PHẨM CỬA HÀNG TIỆN LỢI
            // ==========================================

            modelBuilder.Entity<Category>().HasData(

                new Category
                {
                    CategoryId = 1,
                    CategoryName = "Mì gói & Thực phẩm ăn liền",
                    Description = "Mì ly, phở và các món ăn liền"
                },

                new Category
                {
                    CategoryId = 2,
                    CategoryName = "Đồ ăn chế biến sẵn",
                    Description = "Cơm nắm, bánh mì sandwich và thức ăn dùng ngay"
                },

                new Category
                {
                    CategoryId = 3,
                    CategoryName = "Xúc xích & Thực phẩm đóng gói",
                    Description = "Xúc xích và thực phẩm đóng gói tiện lợi"
                },

                new Category
                {
                    CategoryId = 4,
                    CategoryName = "Bánh & Đồ ăn vặt",
                    Description = "Bánh đóng gói, snack và đồ ăn nhẹ"
                },

                new Category
                {
                    CategoryId = 5,
                    CategoryName = "Nước giải khát",
                    Description = "Nước suối và nước giải khát đóng chai, đóng lon"
                },

                new Category
                {
                    CategoryId = 6,
                    CategoryName = "Trà & Cà phê",
                    Description = "Trà đóng chai và cà phê uống liền"
                },

                new Category
                {
                    CategoryId = 7,
                    CategoryName = "Sữa & Sản phẩm từ sữa",
                    Description = "Sữa tươi và thức uống từ sữa"
                },

                new Category
                {
                    CategoryId = 8,
                    CategoryName = "Nước tăng lực",
                    Description = "Các loại nước uống bổ sung năng lượng"
                },

                new Category
                {
                    CategoryId = 9,
                    CategoryName = "Khăn giấy & Chăm sóc cá nhân",
                    Description = "Khăn giấy và vật dụng chăm sóc cá nhân"
                },

                new Category
                {
                    CategoryId = 10,
                    CategoryName = "Hóa phẩm gia đình",
                    Description = "Nước giặt và các hóa phẩm dùng trong gia đình"
                },

                new Category
                {
                    CategoryId = 11,
                    CategoryName = "Bánh mì & Bữa sáng",
                    Description = "Bánh mì và các sản phẩm tiện lợi cho bữa sáng"
                },

                new Category
                {
                    CategoryId = 12,
                    CategoryName = "Đồ hộp & Thực phẩm khô",
                    Description = "Thực phẩm đóng hộp, mì và các sản phẩm khô"
                },

                new Category
                {
                    CategoryId = 13,
                    CategoryName = "Kem & Thực phẩm đông lạnh",
                    Description = "Kem và thực phẩm bảo quản đông lạnh"
                },

                new Category
                {
                    CategoryId = 14,
                    CategoryName = "Gia vị & Nhu yếu phẩm",
                    Description = "Gia vị và các sản phẩm thiết yếu hằng ngày"
                },

                new Category
                {
                    CategoryId = 15,
                    CategoryName = "Đồ dùng tiện lợi",
                    Description = "Các vật dụng nhỏ phục vụ nhu cầu hằng ngày"
                }
            );


            // ==========================================
            // SEED 15 CUSTOMER
            // KHÁCH HÀNG CỬA HÀNG TIỆN LỢI
            // ==========================================

            modelBuilder.Entity<Customer>().HasData(

                new Customer
                {
                    CustomerId = 1,
                    CustomerName = "Nguyễn Văn An",
                    PhoneNumber = "0901122334",
                    Address = "25 Nguyễn Huệ, phường Sài Gòn, TP.HCM",
                    RewardPoints = 1500,
                    MembershipRank = "Vàng"
                },

                new Customer
                {
                    CustomerId = 2,
                    CustomerName = "Trần Thị Bình",
                    PhoneNumber = "0918877665",
                    Address = "118 Võ Văn Tần, phường Bàn Cờ, TP.HCM",
                    RewardPoints = 500,
                    MembershipRank = "Bạc"
                },

                new Customer
                {
                    CustomerId = 3,
                    CustomerName = "Lê Văn Cường",
                    PhoneNumber = "0983344556",
                    Address = "72 Nguyễn Trãi, phường Chợ Quán, TP.HCM",
                    RewardPoints = 100,
                    MembershipRank = "Chuẩn"
                },

                new Customer
                {
                    CustomerId = 4,
                    CustomerName = "Phạm Thị Dung",
                    PhoneNumber = "0905678123",
                    Address = "156 Thành Thái, phường Diên Hồng, TP.HCM",
                    RewardPoints = 2300,
                    MembershipRank = "Vàng"
                },

                new Customer
                {
                    CustomerId = 5,
                    CustomerName = "Hoàng Văn Đức",
                    PhoneNumber = "0912345678",
                    Address = "43 Điện Biên Phủ, phường Gia Định, TP.HCM",
                    RewardPoints = 800,
                    MembershipRank = "Bạc"
                },

                new Customer
                {
                    CustomerId = 6,
                    CustomerName = "Võ Thị Hà",
                    PhoneNumber = "0987654321",
                    Address = "89 Phạm Văn Đồng, phường Hạnh Thông, TP.HCM",
                    RewardPoints = 3200,
                    MembershipRank = "Kim Cương"
                },

                new Customer
                {
                    CustomerId = 7,
                    CustomerName = "Đặng Văn Giang",
                    PhoneNumber = "0909876543",
                    Address = "215 Cộng Hòa, phường Tân Bình, TP.HCM",
                    RewardPoints = 250,
                    MembershipRank = "Chuẩn"
                },

                new Customer
                {
                    CustomerId = 8,
                    CustomerName = "Bùi Thị Hoa",
                    PhoneNumber = "0913456789",
                    Address = "36 Phan Đình Phùng, phường Cầu Kiệu, TP.HCM",
                    RewardPoints = 1200,
                    MembershipRank = "Bạc"
                },

                new Customer
                {
                    CustomerId = 9,
                    CustomerName = "Đỗ Văn Hùng",
                    PhoneNumber = "0981234567",
                    Address = "102 Nguyễn Thị Thập, phường Tân Hưng, TP.HCM",
                    RewardPoints = 4500,
                    MembershipRank = "Kim Cương"
                },

                new Customer
                {
                    CustomerId = 10,
                    CustomerName = "Nguyễn Thị Khánh",
                    PhoneNumber = "0903456789",
                    Address = "68 Hậu Giang, phường Bình Phú, TP.HCM",
                    RewardPoints = 150,
                    MembershipRank = "Chuẩn"
                },

                new Customer
                {
                    CustomerId = 11,
                    CustomerName = "Trương Văn Long",
                    PhoneNumber = "0915678901",
                    Address = "145 Võ Văn Ngân, phường Thủ Đức, TP.HCM",
                    RewardPoints = 1800,
                    MembershipRank = "Vàng"
                },

                new Customer
                {
                    CustomerId = 12,
                    CustomerName = "Phan Thị Mai",
                    PhoneNumber = "0986789012",
                    Address = "234 Lê Văn Khương, phường Thới An, TP.HCM",
                    RewardPoints = 700,
                    MembershipRank = "Bạc"
                },

                new Customer
                {
                    CustomerId = 13,
                    CustomerName = "Lý Văn Nam",
                    PhoneNumber = "0907890123",
                    Address = "57 Lũy Bán Bích, phường Tân Phú, TP.HCM",
                    RewardPoints = 5000,
                    MembershipRank = "Kim Cương"
                },

                new Customer
                {
                    CustomerId = 14,
                    CustomerName = "Huỳnh Thị Phương",
                    PhoneNumber = "0918901234",
                    Address = "321 Tỉnh Lộ 10, phường Bình Trị Đông, TP.HCM",
                    RewardPoints = 350,
                    MembershipRank = "Chuẩn"
                },

                new Customer
                {
                    CustomerId = 15,
                    CustomerName = "Mai Văn Quân",
                    PhoneNumber = "0989012345",
                    Address = "78 Nguyễn Hữu Trí, xã Tân Nhựt, TP.HCM",
                    RewardPoints = 2600,
                    MembershipRank = "Vàng"
                }
            );


            // ==========================================
            // SEED 15 PRODUCT
            // SẢN PHẨM CỬA HÀNG TIỆN LỢI CIRCLE K EXPRESS
            // ==========================================

            modelBuilder.Entity<Product>().HasData(

                new Product
                {
                    ProductId = 1,
                    Barcode = "CK000001",
                    ProductName = "Mì ly Omachi sườn hầm ngũ quả 67g",
                    Price = 18000m,
                    StockQuantity = 100,
                    CategoryId = 1
                },

                new Product
                {
                    ProductId = 2,
                    Barcode = "CK000002",
                    ProductName = "Mì ly Hảo Hảo tôm chua cay 67g",
                    Price = 12000m,
                    StockQuantity = 150,
                    CategoryId = 1
                },

                new Product
                {
                    ProductId = 3,
                    Barcode = "CK000003",
                    ProductName = "Cơm nắm cá ngừ mayonnaise",
                    Price = 25000m,
                    StockQuantity = 50,
                    CategoryId = 2
                },

                new Product
                {
                    ProductId = 4,
                    Barcode = "CK000004",
                    ProductName = "Bánh mì sandwich xúc xích",
                    Price = 22000m,
                    StockQuantity = 60,
                    CategoryId = 2
                },

                new Product
                {
                    ProductId = 5,
                    Barcode = "CK000005",
                    ProductName = "Xúc xích tiệt trùng CP 70g",
                    Price = 15000m,
                    StockQuantity = 120,
                    CategoryId = 3
                },

                new Product
                {
                    ProductId = 6,
                    Barcode = "CK000006",
                    ProductName = "Bánh Chocopie Orion 6 cái",
                    Price = 32000m,
                    StockQuantity = 80,
                    CategoryId = 4
                },

                new Product
                {
                    ProductId = 7,
                    Barcode = "CK000007",
                    ProductName = "Khoai tây chiên Lay's vị tự nhiên 95g",
                    Price = 28000m,
                    StockQuantity = 90,
                    CategoryId = 4
                },

                new Product
                {
                    ProductId = 8,
                    Barcode = "CK000008",
                    ProductName = "Nước suối Aquafina 500ml",
                    Price = 7000m,
                    StockQuantity = 200,
                    CategoryId = 5
                },

                new Product
                {
                    ProductId = 9,
                    Barcode = "CK000009",
                    ProductName = "Nước ngọt Coca-Cola lon 330ml",
                    Price = 11000m,
                    StockQuantity = 180,
                    CategoryId = 5
                },

                new Product
                {
                    ProductId = 10,
                    Barcode = "CK000010",
                    ProductName = "Trà xanh C2 hương chanh 455ml",
                    Price = 10000m,
                    StockQuantity = 150,
                    CategoryId = 6
                },

                new Product
                {
                    ProductId = 11,
                    Barcode = "CK000011",
                    ProductName = "Cà phê sữa Highlands 235ml",
                    Price = 18000m,
                    StockQuantity = 100,
                    CategoryId = 6
                },

                new Product
                {
                    ProductId = 12,
                    Barcode = "CK000012",
                    ProductName = "Sữa tươi Vinamilk 100% 180ml",
                    Price = 9000m,
                    StockQuantity = 120,
                    CategoryId = 7
                },

                new Product
                {
                    ProductId = 13,
                    Barcode = "CK000013",
                    ProductName = "Nước tăng lực Red Bull 250ml",
                    Price = 13000m,
                    StockQuantity = 100,
                    CategoryId = 8
                },

                new Product
                {
                    ProductId = 14,
                    Barcode = "CK000014",
                    ProductName = "Khăn giấy bỏ túi Pulppy 10 tờ",
                    Price = 8000m,
                    StockQuantity = 70,
                    CategoryId = 9
                },

                new Product
                {
                    ProductId = 15,
                    Barcode = "CK000015",
                    ProductName = "Nước giặt OMO túi 400g",
                    Price = 25000m,
                    StockQuantity = 40,
                    CategoryId = 10
                }
            );
        }
    }
}
