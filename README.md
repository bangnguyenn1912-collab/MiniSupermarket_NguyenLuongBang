# 🛒 CỬA HÀNG TIỆN LỢI CIRCLE K EXPRESS
## HỆ THỐNG QUẢN LÝ BÁN LẺ VÀ TỒN KHO

**Môn học:** Lập trình Ứng dụng .NET Core  
**Mã môn:** 229162  
**Buổi thực hành:** Buổi 04  
**Đề tài:** Cửa hàng Tiện lợi Circle K Express

---

## 1. Giới thiệu dự án

Circle K Express là hệ thống quản lý bán lẻ và tồn kho dành cho cửa hàng tiện lợi, được phát triển bằng **ASP.NET Core Web API, Windows Forms và Microsoft SQL Server**.

Dự án được nâng cấp từ lưu trữ dữ liệu tạm thời (In-Memory) sang cơ sở dữ liệu SQL Server, giúp dữ liệu được lưu trữ lâu dài và hỗ trợ quản lý tập trung.

### Các chức năng chính

- Đăng nhập và xác thực bằng JWT.
- Phân quyền Admin, Cashier và Warehouse.
- Quản lý danh mục sản phẩm.
- Quản lý sản phẩm và tồn kho.
- Quản lý khách hàng.
- Quản lý bán hàng (POS).
- Quản lý hóa đơn.
- Báo cáo doanh thu.
- Quản lý tài khoản và vai trò người dùng.

---

## 2. Mô hình kiến trúc hệ thống

Hệ thống được xây dựng theo mô hình **Client – Server – Database**.

```text
+----------------------------------+
| MiniSupermarket.WinForms         |
| Giao diện người dùng             |
| Admin / Cashier / Warehouse      |
+----------------------------------+
                |
                | HTTP / HTTPS
                | JWT Token
                v
+----------------------------------+
| MiniSupermarket.API              |
| ASP.NET Core Web API             |
| Controllers / Services           |
| Authentication / Authorization   |
+----------------------------------+
                |
                | Entity Framework Core
                | LINQ / DbContext
                v
+----------------------------------+
| Microsoft SQL Server             |
| NguyenLuongBang_MiniSupermarketDb|
| Categories / Products / Customers|
| Orders / OrderDetails / Users    |
+----------------------------------+
```

### 2.1. Database – Microsoft SQL Server

Lưu trữ dữ liệu của hệ thống, bao gồm danh mục, sản phẩm, khách hàng, hóa đơn và tài khoản.

**Tên database:**

`NguyenLuongBang_MiniSupermarketDb`

### 2.2. Backend – MiniSupermarket.API

Sử dụng ASP.NET Core Web API kết hợp Entity Framework Core 8.0.

Nhiệm vụ:

- Cung cấp các RESTful API.
- Xử lý nghiệp vụ CRUD.
- Kết nối SQL Server thông qua DbContext.
- Xác thực JWT.
- Kiểm tra vai trò và trạng thái tài khoản.
- Băm và xác minh mật khẩu bằng BCrypt.

### 2.3. Frontend – MiniSupermarket.WinForms

Ứng dụng Windows Forms đóng vai trò máy trạm quản lý.

Nhiệm vụ:

- Hiển thị giao diện quản lý.
- Gọi API bằng HttpClient.
- Hiển thị dữ liệu trên DataGridView.
- Lưu thông tin phiên đăng nhập thông qua SessionManager.
- Điều hướng giao diện theo vai trò người dùng.

---

## 3. Công nghệ sử dụng

| Thành phần | Công nghệ |
|---|---|
| Ngôn ngữ | C# |
| Framework | .NET 8.0 |
| Backend | ASP.NET Core Web API |
| Frontend | Windows Forms |
| Database | Microsoft SQL Server |
| ORM | Entity Framework Core 8.0 |
| Database Migration | EF Core Migrations |
| Xác thực | JWT Bearer |
| Mã hóa mật khẩu | BCrypt |
| Truy vấn dữ liệu | LINQ |
| Kiểm thử API | Swagger UI |
| IDE | Visual Studio 2022 |
| Quản lý Database | SQL Server Management Studio |

---

## 4. Cấu trúc Solution

```text
MiniSuperMarket_NguyenLuongBang/
│
├── MiniSupermarket.API/
│   ├── Controllers/
│   │   ├── AuthController.cs
│   │   └── Các Controller quản lý dữ liệu
│   │
│   ├── Data/
│   │   ├── SupermarketDbContext.cs
│   │   └── DemoUserSeeder.cs
│   │
│   ├── Models/
│   │   ├── Category.cs
│   │   ├── Product.cs
│   │   ├── Customer.cs
│   │   ├── Order.cs
│   │   ├── OrderDetail.cs
│   │   └── User.cs
│   │
│   ├── Services/
│   │   └── DemoAccountStore.cs
│   │
│   ├── Migrations/
│   ├── appsettings.json
│   └── Program.cs
│
└── MiniSupermarket.WinForms/
    ├── FormLogin.cs
    ├── FormMainShell.cs
    ├── FormCategoryManagement.cs
    ├── FormCustomerManagement.cs
    ├── Các Form quản lý khác
    └── SessionManager.cs
```

---

## 5. Cơ sở dữ liệu

Dự án sử dụng Entity Framework Core theo phương pháp **Code-First**.

Các bảng chính:

| Bảng | Chức năng |
|---|---|
| Categories | Quản lý danh mục |
| Products | Quản lý sản phẩm |
| Customers | Quản lý khách hàng |
| Orders | Quản lý hóa đơn |
| OrderDetails | Chi tiết hóa đơn |
| Users | Quản lý tài khoản |

### 5.1. Dữ liệu mẫu

Hệ thống có dữ liệu mẫu phục vụ thực hành:

- 15 danh mục.
- 15 sản phẩm.
- 15 khách hàng.
- 15 hóa đơn.
- 15 chi tiết hóa đơn.
- 17 tài khoản người dùng.

### 5.2. Tài khoản người dùng

17 tài khoản được lưu trong bảng `dbo.Users`:

| Vai trò | Số lượng |
|---|---:|
| Admin | 5 |
| Cashier | 7 |
| Warehouse | 5 |
| **Tổng cộng** | **17** |

Các tài khoản mẫu được khởi tạo thông qua `DemoUserSeeder.cs` trong môi trường Development.

Mật khẩu được lưu dưới dạng BCrypt hash thay vì văn bản thuần.

---

## 6. Hướng dẫn cài đặt và chạy dự án

### Bước 1. Mở Solution

Mở Visual Studio 2022 và tải Solution `MiniSuperMarket_NguyenLuongBang`.

### Bước 2. Kiểm tra kết nối SQL Server

Mở file:

`MiniSupermarket.API/appsettings.json`

Kiểm tra chuỗi kết nối:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=NguyenLuongBang_MiniSupermarketDb;Trusted_Connection=true;TrustServerCertificate=True;"
  }
}
```

Giữ nguyên các cấu hình khác đang có trong file.

### Bước 3. Cập nhật Database

Mở:

**Tools → NuGet Package Manager → Package Manager Console**

Chọn Default Project:

`MiniSupermarket.API`

Chạy:

```powershell
Update-Database
```

Lệnh này áp dụng các migration còn thiếu. Nếu database đã được cập nhật đầy đủ, không cần tạo migration mới.

### Bước 4. Chạy Backend

Chọn `MiniSupermarket.API` làm Startup Project.

Nhấn `F5` để chạy.

Địa chỉ Swagger trong môi trường phát triển hiện tại:

`https://localhost:7158/swagger/index.html`

Lưu ý: Cổng HTTPS có thể thay đổi tùy cấu hình máy.

### Bước 5. Chạy Frontend

Nhấp chuột phải vào `MiniSupermarket.WinForms`.

Chọn **Debug → Start New Instance**.

Kiểm tra địa chỉ API trong các Form và đăng nhập vào hệ thống.

---

## 7. Xác thực và phân quyền

Hệ thống sử dụng JWT Bearer Authentication.

### Admin

- Quản lý danh mục.
- Quản lý sản phẩm.
- Quản lý khách hàng.
- Quản lý tài khoản.
- Quản lý vai trò.
- Truy cập các chức năng quản trị được cấp quyền.

### Cashier

- Thực hiện chức năng bán hàng.
- Thao tác với các chức năng thu ngân được cấp quyền.
- Không được thực hiện các thao tác quản trị bị hạn chế.

### Warehouse

- Thực hiện các chức năng quản lý kho được cấp quyền.
- Không được sử dụng chức năng quản trị ngoài phạm vi vai trò.

Hệ thống kiểm tra trạng thái tài khoản và vai trò từ SQL Server khi xác thực JWT.

---

## 8. Kết quả thực hiện

### 8.1. Backend

- Xây dựng Web API bằng ASP.NET Core.
- Tích hợp Entity Framework Core.
- Kết nối thành công SQL Server.
- Tạo và cập nhật bảng thông qua Migrations.
- Chuyển dịch vụ tài khoản từ In-Memory sang SQL Server.
- Build API thành công.

### 8.2. Database

- Bảng `dbo.Users` đã được tạo.
- Đã lưu thành công 17 tài khoản mẫu.
- Phân bố tài khoản: 5 Admin, 7 Cashier, 5 Warehouse.
- Truy vấn kiểm tra trong SSMS thực thi thành công.

### 8.3. Frontend

- Ứng dụng WinForms khởi chạy được.
- Giao diện chính hiển thị Sidebar và các chức năng quản lý.
- Giao diện hiển thị thông tin nhân viên và vai trò Admin.

### 8.4. Nội dung cần tiếp tục kiểm thử

- Xác minh kết quả HTTP 200 và JWT của API đăng nhập.
- Kiểm thử phân quyền Admin, Cashier và Warehouse.
- Kiểm thử các thao tác CRUD với SQL Server.
- Kiểm tra chức năng báo cáo doanh thu.

---

## 9. Kết luận

Qua **Buổi thực hành 04**, dự án **Cửa hàng Tiện lợi Circle K Express** đã được tích hợp Microsoft SQL Server và Entity Framework Core theo phương pháp Code-First.

Việc sử dụng cơ sở dữ liệu quan hệ giúp hệ thống quản lý dữ liệu bền vững hơn so với lưu trữ In-Memory.

Hệ thống cũng được mở rộng với chức năng quản lý tài khoản trong SQL Server, xác thực JWT và phân quyền người dùng.

Các nội dung tiếp theo tập trung vào kiểm thử API, hoàn thiện nghiệp vụ và kiểm tra sự đồng bộ giữa Windows Forms, Web API và SQL Server.

---

## 10. Thông tin sinh viên

**Họ và tên:** Nguyễn Lương Bằng  
**Mã số sinh viên:** 2124110125  
**Môn học:** Lập trình Ứng dụng .NET Core  
**Mã môn:** 229162  
**Buổi thực hành:** Buổi 04  
**Đề tài:** Cửa hàng Tiện lợi Circle K Express
