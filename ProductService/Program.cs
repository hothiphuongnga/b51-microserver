
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Mapping;
using ProductService.Repositories;
using ProductService.Repositories.Base;
using UserService.Services;

var builder = WebApplication.CreateBuilder(args);

// === ĐĂNG KÝ CÁC SERVICE (DEPENDENCY INJECTION) ===

// Đăng ký DbContext, cấu hình sử dụng SQL Server với chuỗi kết nối từ appsettings.json
builder.Services.AddDbContext<ProductDbServiceContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    options.UseSqlServer(connectionString);
});

// Đăng ký UnitOfWork và Repository theo mô hình Dependency Injection
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductServicee>();

// Đăng ký AutoMapper để hỗ trợ ánh xạ giữa các đối tượng
builder.Services.AddAutoMapper(cf=>{}, typeof(MappingProfile));

builder.Services.AddRazorPages();          // Hỗ trợ Razor Pages
builder.Services.AddServerSideBlazor();    // Hỗ trợ Blazor Server
builder.Services.AddControllers();         // Hỗ trợ API Controllers
builder.Services.AddSwaggerGen();          // Hỗ trợ Swagger (OpenAPI) cho tài liệu API



// === Câu hình AUTHEN, AUTHOR ===
var privateKey = builder.Configuration["jwt:Serect-Key"];
var Issuer = builder.Configuration["jwt:Issuer"];
var Audience = builder.Configuration["jwt:Audience"];




builder.Services.AddAuthorization();


// CORS cho order

builder.Services.AddCors(client =>
{
    client.AddPolicy("AllowOrderService", policy =>
    {
        // mở cho tất cả 
            // policy.AllowAnyOrigin()
            //     .AllowAnyHeader()
            //     .AllowAnyMethod();
        policy.WithOrigins("https://localhost:7138") // Địa chỉ của OrderService
              .AllowAnyHeader()
              .AllowAnyMethod();
});
});

var app = builder.Build();

// === CẤU HÌNH MIDDLEWARE PIPELINE ===

// Kích hoạt Swagger & giao diện Swagger UI cho API docs & thử nghiệm
app.UseSwagger();
app.UseSwaggerUI();

// Tự động chuyển hướng HTTP sang HTTPS (bảo mật)
app.UseHttpsRedirection();

// Cho phép truy cập các file tĩnh (CSS, JS, ảnh, ...)
app.UseStaticFiles();

// Kích hoạt định tuyến
app.UseRouting();

// Map các endpoint cho Controller API, RazorPages, Blazor và fallback
app.MapControllers();

app.Run();