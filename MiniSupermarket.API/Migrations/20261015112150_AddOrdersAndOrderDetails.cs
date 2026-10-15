using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniSupermarket.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdersAndOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CashierUsername = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CashReceived = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ChangeAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.OrderDetailId);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CashReceived", "CashierUsername", "ChangeAmount", "CustomerPhone", "OrderDate", "TotalAmount" },
                values: new object[,]
                {
                    { 1, 40000m, "admin", 4000m, "0901122334", new DateTime(2026, 10, 1, 8, 0, 0, 0, DateTimeKind.Utc), 36000m },
                    { 2, 30000m, "cashier", 6000m, "0918877665", new DateTime(2026, 10, 1, 9, 0, 0, 0, DateTimeKind.Utc), 24000m },
                    { 3, 50000m, "admin", 0m, "0983344556", new DateTime(2026, 10, 2, 8, 0, 0, 0, DateTimeKind.Utc), 50000m },
                    { 4, 50000m, "cashier", 6000m, "0905678123", new DateTime(2026, 10, 2, 9, 0, 0, 0, DateTimeKind.Utc), 44000m },
                    { 5, 30000m, "admin", 0m, "0912345678", new DateTime(2026, 10, 3, 8, 0, 0, 0, DateTimeKind.Utc), 30000m },
                    { 6, 70000m, "cashier", 6000m, "0987654321", new DateTime(2026, 10, 3, 9, 0, 0, 0, DateTimeKind.Utc), 64000m },
                    { 7, 60000m, "admin", 4000m, "0909876543", new DateTime(2026, 10, 4, 8, 0, 0, 0, DateTimeKind.Utc), 56000m },
                    { 8, 20000m, "cashier", 6000m, "0913456789", new DateTime(2026, 10, 4, 9, 0, 0, 0, DateTimeKind.Utc), 14000m },
                    { 9, 30000m, "admin", 8000m, "0981234567", new DateTime(2026, 10, 5, 8, 0, 0, 0, DateTimeKind.Utc), 22000m },
                    { 10, 20000m, "cashier", 0m, "0903456789", new DateTime(2026, 10, 5, 9, 0, 0, 0, DateTimeKind.Utc), 20000m },
                    { 11, 40000m, "admin", 4000m, "0915678901", new DateTime(2026, 10, 6, 8, 0, 0, 0, DateTimeKind.Utc), 36000m },
                    { 12, 20000m, "cashier", 2000m, "0986789012", new DateTime(2026, 10, 6, 9, 0, 0, 0, DateTimeKind.Utc), 18000m },
                    { 13, 30000m, "admin", 4000m, "0907890123", new DateTime(2026, 10, 7, 8, 0, 0, 0, DateTimeKind.Utc), 26000m },
                    { 14, 20000m, "cashier", 4000m, "0918901234", new DateTime(2026, 10, 7, 9, 0, 0, 0, DateTimeKind.Utc), 16000m },
                    { 15, 50000m, "admin", 0m, "0989012345", new DateTime(2026, 10, 8, 8, 0, 0, 0, DateTimeKind.Utc), 50000m }
                });

            migrationBuilder.InsertData(
                table: "OrderDetails",
                columns: new[] { "OrderDetailId", "OrderId", "ProductId", "Quantity", "TotalPrice", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 36000m, 18000m },
                    { 2, 2, 2, 2, 24000m, 12000m },
                    { 3, 3, 3, 2, 50000m, 25000m },
                    { 4, 4, 4, 2, 44000m, 22000m },
                    { 5, 5, 5, 2, 30000m, 15000m },
                    { 6, 6, 6, 2, 64000m, 32000m },
                    { 7, 7, 7, 2, 56000m, 28000m },
                    { 8, 8, 8, 2, 14000m, 7000m },
                    { 9, 9, 9, 2, 22000m, 11000m },
                    { 10, 10, 10, 2, 20000m, 10000m },
                    { 11, 11, 11, 2, 36000m, 18000m },
                    { 12, 12, 12, 2, 18000m, 9000m },
                    { 13, 13, 13, 2, 26000m, 13000m },
                    { 14, 14, 14, 2, 16000m, 8000m },
                    { 15, 15, 15, 2, 50000m, 25000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductId",
                table: "OrderDetails",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
