using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedCircleKData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Mì gói & Thực phẩm ăn liền", "Mì ly, phở và các món ăn liền" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Đồ ăn chế biến sẵn", "Cơm nắm, bánh mì sandwich và thức ăn dùng ngay" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Xúc xích & Thực phẩm đóng gói", "Xúc xích và thực phẩm đóng gói tiện lợi" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh & Đồ ăn vặt", "Bánh đóng gói, snack và đồ ăn nhẹ" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Nước giải khát", "Nước suối và nước giải khát đóng chai, đóng lon" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Trà & Cà phê", "Trà đóng chai và cà phê uống liền" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Sữa & Sản phẩm từ sữa", "Sữa tươi và thức uống từ sữa" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Nước tăng lực", "Các loại nước uống bổ sung năng lượng" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Khăn giấy & Chăm sóc cá nhân", "Khăn giấy và vật dụng chăm sóc cá nhân" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Hóa phẩm gia đình", "Nước giặt và các hóa phẩm dùng trong gia đình" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 11,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh mì & Bữa sáng", "Bánh mì và các sản phẩm tiện lợi cho bữa sáng" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 12,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Đồ hộp & Thực phẩm khô", "Thực phẩm đóng hộp, mì và các sản phẩm khô" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 13,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Kem & Thực phẩm đông lạnh", "Kem và thực phẩm bảo quản đông lạnh" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 14,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Gia vị & Nhu yếu phẩm", "Gia vị và các sản phẩm thiết yếu hằng ngày" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 15,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Đồ dùng tiện lợi", "Các vật dụng nhỏ phục vụ nhu cầu hằng ngày" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "CK000001", 1, 18000m, "Mì ly Omachi sườn hầm ngũ quả 67g" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000002", 1, 12000m, "Mì ly Hảo Hảo tôm chua cay 67g", 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000003", 2, 25000m, "Cơm nắm cá ngừ mayonnaise", 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000004", 2, 22000m, "Bánh mì sandwich xúc xích", 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "CK000005", 3, 15000m, "Xúc xích tiệt trùng CP 70g" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000006", 4, 32000m, "Bánh Chocopie Orion 6 cái", 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000007", 4, 28000m, "Khoai tây chiên Lay's vị tự nhiên 95g", 90 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000008", 5, 7000m, "Nước suối Aquafina 500ml", 200 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "CK000009", 5, 11000m, "Nước ngọt Coca-Cola lon 330ml" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000010", 6, 10000m, "Trà xanh C2 hương chanh 455ml", 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000011", 6, 18000m, "Cà phê sữa Highlands 235ml", 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000012", 7, 9000m, "Sữa tươi Vinamilk 100% 180ml", 120 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000013", 8, 13000m, "Nước tăng lực Red Bull 250ml", 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000014", 9, 8000m, "Khăn giấy bỏ túi Pulppy 10 tờ", 70 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "CK000015", 10, 25000m, "Nước giặt OMO túi 400g", 40 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Kẹo", "Các loại kẹo mềm, kẹo cứng, kẹo dẻo nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Socola", "Socola thanh, socola viên và socola hộp nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh quy", "Bánh quy bơ, bánh quy socola và bánh quy nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh xốp", "Các loại bánh xốp, wafer và bánh kem xốp" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Snack", "Snack khoai tây, snack ngô và các loại đồ ăn nhẹ" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 6,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh ngọt", "Bánh ngọt đóng gói và các loại bánh nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 7,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Kẹo dẻo", "Kẹo dẻo trái cây, kẹo dẻo hình thú và kẹo gummy" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 8,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Socola cao cấp", "Socola cao cấp, socola quà tặng và socola hộp" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 9,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh quy cao cấp", "Bánh quy cao cấp dùng làm quà tặng" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 10,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Kẹo bạc hà", "Kẹo bạc hà, kẹo the và kẹo ngậm nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 11,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Kẹo trái cây", "Các loại kẹo có hương vị trái cây" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 12,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh ăn sáng", "Bánh ngũ cốc, bánh ăn sáng và sản phẩm tiện lợi" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 13,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Hạt & Đồ ăn vặt", "Các loại hạt, trái cây sấy và đồ ăn vặt nhập khẩu" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 14,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Quà tặng bánh kẹo", "Hộp bánh kẹo và set quà tặng Sweet Land" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 15,
                columns: new[] { "CategoryName", "Description" },
                values: new object[] { "Bánh kẹo khác", "Các sản phẩm bánh kẹo nhập khẩu khác" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 1,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "SWT000001", 7, 95000m, "Kẹo dẻo Haribo Goldbears 200g" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 2,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000002", 8, 189000m, "Socola Ferrero Rocher 16 viên", 80 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 3,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000003", 9, 165000m, "Bánh quy bơ Danisa 454g", 60 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 4,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000004", 4, 79000m, "Bánh xốp Loacker Quadratini Chocolate 125g", 90 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 5,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "SWT000005", 5, 72000m, "Snack khoai tây Pringles Original 107g" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 6,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000006", 2, 115000m, "Socola KitKat Mini 170g", 100 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 7,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000007", 10, 35000m, "Kẹo bạc hà Alpenliebe 90g", 150 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 8,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000008", 8, 135000m, "Socola Lindt Excellence Dark 100g", 70 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 9,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName" },
                values: new object[] { "SWT000009", 3, 45000m, "Bánh quy Oreo Original 133g" });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 10,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000010", 11, 69000m, "Kẹo trái cây Skittles Fruits 125g", 110 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 11,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000011", 12, 145000m, "Bánh ngũ cốc Kellogg's 300g", 50 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 12,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000012", 14, 175000m, "Hộp quà Socola Merci 250g", 45 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 13,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000013", 13, 120000m, "Hạt hạnh nhân rang muối 200g", 75 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 14,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000014", 7, 105000m, "Kẹo dẻo Trolli Sour Glowworms 200g", 85 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "ProductId",
                keyValue: 15,
                columns: new[] { "Barcode", "CategoryId", "Price", "ProductName", "StockQuantity" },
                values: new object[] { "SWT000015", 3, 125000m, "Bánh quy Walkers Shortbread 150g", 55 });
        }
    }
}
