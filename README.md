# 🛒 CỬA HÀNG TIỆN LỢI CIRCLE K EXPRESS

## HỆ THỐNG QUẢN LÝ BÁN LẺ VÀ TỒN KHO

**Môn học:** Lập trình Ứng dụng .NET Core  
**Mã môn:** 229162  
**Buổi thực hành:** Buổi 04  
**Đề tài:** Cửa hàng Tiện lợi Circle K Express

**Sinh viên thực hiện:** Nguyễn Lương Bằng  
**Mã số sinh viên:** 2124110125

---

## 1. Giới thiệu đề tài

**Circle K Express** là hệ thống quản lý cửa hàng tiện lợi được xây dựng bằng C# trên nền tảng .NET 8.0, sử dụng mô hình Client – Server – Database.

Hệ thống hỗ trợ quản lý bán hàng, sản phẩm, danh mục, khách hàng, hóa đơn, tồn kho, tài khoản nhân viên và báo cáo doanh thu.

Dự án gồm hai thành phần chính:

- **MiniSupermarket.API:** Backend ASP.NET Core Web API, xử lý nghiệp vụ, xác thực JWT và kết nối SQL Server thông qua Entity Framework Core.
- **MiniSupermarket.WinForms:** Ứng dụng Windows Forms cung cấp giao diện quản lý cho Admin, Cashier và Warehouse.

Dữ liệu được lưu trữ trong Microsoft SQL Server, giúp đảm bảo tính bền vững và hỗ trợ các thao tác CRUD.

---

## 2. Mục tiêu thực hành Buổi 04

- Hoàn thiện giao diện quản lý cửa hàng tiện lợi.
- Tích hợp Windows Forms với ASP.NET Core Web API.
- Sử dụng SQL Server để lưu trữ dữ liệu.
- Quản lý tài khoản nhân viên bằng Entity Framework Core.
- Xác thực người dùng bằng JWT.
- Phân quyền Admin, Cashier và Warehouse.
- Thực hiện các chức năng quản lý bán hàng, sản phẩm, khách hàng và tồn kho.
- Kiểm thử hệ thống bằng Swagger UI và SQL Server Management Studio.
