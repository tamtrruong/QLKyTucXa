using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QLKTX_BUS;
using QLKTX_DAO;
using QLKTX_DAO.Model;
using System.Text;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. KẾT NỐI DATABASE (POSTGRESQL)
// ==========================================
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<QLKTXContext>(options =>
    options.UseNpgsql(connectionString)
);

// ==========================================
// 2. ĐĂNG KÝ CÁC SERVICE (DEPENDENCY INJECTION)
// ==========================================
// --- Đăng ký DAO ---
builder.Services.AddScoped<TaiKhoan_DAO>();
builder.Services.AddScoped<Sinhvien_DAO>();
builder.Services.AddScoped<Phong_DAO>();
builder.Services.AddScoped<HopDong_DAO>();
builder.Services.AddScoped<HoaDon_DAO>();
builder.Services.AddScoped<DienNuoc_DAO>();
builder.Services.AddScoped<BangGia_DAO>();
builder.Services.AddScoped<ThongKe_DAO>();
builder.Services.AddScoped<ViPham_DAO>(); 

// --- Đăng ký BUS ---
builder.Services.AddScoped<Auth_BUS>();
builder.Services.AddScoped<Sinhvien_BUS>();
builder.Services.AddScoped<Phong_BUS>();
builder.Services.AddScoped<HopDong_BUS>();
builder.Services.AddScoped<HoaDon_BUS>();
builder.Services.AddScoped<DienNuoc_BUS>();
builder.Services.AddScoped<Thongke_BUS>();
builder.Services.AddScoped<ViPham_BUS>();

// --- Đăng ký AutoMapper ---
builder.Services.AddAutoMapper(cfg => { },
    typeof(QLKTX_BUS.MappingProfile).Assembly);


// ==========================================
// 3. CẤU HÌNH JWT AUTHENTICATION
// ==========================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, 
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        // Lấy Key từ appsettings.json
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// ==========================================
// 4. CẤU HÌNH SWAGGER (Để nhập Token được)
// ==========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "QLKTX API", Version = "v1" });

    // Cấu hình nút "Authorize" (ổ khóa) trên Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// ==========================================
// 5. CORS (Cho phép Frontend React gọi API)
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();

// ==========================================
// 6. MIDDLEWARE PIPELINE
// ==========================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAll"); // Phải đặt trước Auth

// QUAN TRỌNG:
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.Run($"http://0.0.0.0:{port}");
