
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MiniSupermarket.API.Data;
using MiniSupermarket.API.Services;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CẤU HÌNH JWT
// ==========================================

var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? "SupermarketSecretKeyDoAnMonHoc2026SecureString!!";

// ==========================================
// KẾT NỐI SQL SERVER
// ==========================================

var connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<SupermarketDbContext>(options =>
    options.UseSqlServer(connectionString));

// ==========================================
// ĐĂNG KÝ DỊCH VỤ TÀI KHOẢN
// ==========================================

// MỚI: Scoped để sử dụng SupermarketDbContext.
// Không sử dụng Singleton vì DbContext có vòng đời Scoped.
builder.Services.AddScoped<DemoAccountStore>();

// ==========================================
// CẤU HÌNH XÁC THỰC JWT
// ==========================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(jwtSecret)),

        ValidateIssuer = false,
        ValidateAudience = false,

        // Kiểm tra token hết hạn.
        ValidateLifetime = true,

        ClockSkew = TimeSpan.FromSeconds(30)
    };

    // ==========================================
    // KIỂM TRA TRẠNG THÁI TÀI KHOẢN KHI DÙNG JWT
    // ==========================================

    options.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var store = context.HttpContext.RequestServices
                .GetRequiredService<DemoAccountStore>();

            string username = context.Principal?
                .FindFirstValue(ClaimTypes.Name) ?? "";

            string role = context.Principal?
                .FindFirstValue(ClaimTypes.Role) ?? "";

            // Kiểm tra trực tiếp trạng thái và vai trò
            // của tài khoản trong SQL Server.
            bool isValid = await store.IsActiveWithRoleAsync(
                username,
                role);

            // Nếu tài khoản bị khóa, bị xóa hoặc đổi vai trò,
            // JWT cũ không được tiếp tục truy cập.
            if (!isValid)
            {
                context.Fail(
                    "Tài khoản không còn quyền truy cập.");
            }
        }
    };
});

// ==========================================
// PHÂN QUYỀN
// ==========================================

builder.Services.AddAuthorization();

// ==========================================
// CONTROLLERS VÀ SWAGGER
// ==========================================

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Nhập JWT token theo dạng: Bearer {token}"
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });
});

// ==========================================
// KHỞI TẠO ỨNG DỤNG
// ==========================================

var app = builder.Build();

// ==========================================
// KHỞI TẠO TÀI KHOẢN MẪU TRONG SQL SERVER
// ==========================================

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    var dbContext = scope.ServiceProvider
        .GetRequiredService<SupermarketDbContext>();

    // Chỉ thêm những tài khoản mẫu chưa tồn tại.
    DemoUserSeeder.Seed(dbContext);
}


// ==========================================
// MIDDLEWARE
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Xác thực JWT.
app.UseAuthentication();

// Kiểm tra quyền.
app.UseAuthorization();

// Kích hoạt Controllers.
app.MapControllers();

// Chạy API.
app.Run();
