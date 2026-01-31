using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics; // Cần thêm dòng này để xử lý lỗi Warning
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using PCM.Api.Data;
using PCM.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= 1. DATABASE CONFIGURATION =================

var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
string connectionString;

if (!string.IsNullOrWhiteSpace(databaseUrl))
{
    // Cấu hình cho Railway (Production)
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    connectionString = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    }.ConnectionString;
}
else
{
    // Cấu hình cho Local (Máy cá nhân)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new Exception("Không tìm thấy chuỗi kết nối Database!");
}


// ================= 2. DB CONTEXT & WARNING FIX =================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString)
           // QUAN TRỌNG: Dòng này giúp bỏ qua lỗi "PendingModelChangesWarning" của EF Core 9
           .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
});


// ================= 3. SERVICES & IDENTITY =================

builder.Services.AddScoped<IMatchService, MatchService>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


// ================= 4. JWT AUTHENTICATION =================

var jwtKey = builder.Configuration["Jwt:Key"] ?? "PCM_DEFAULT_SECRET_KEY_123456789_ABC";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "PCM.Api";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "PCM.Client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});


// ================= 5. CONTROLLERS & CORS =================

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("https://pcm-pickleball.netlify.app") // Frontend của bạn
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


// ================= 6. SWAGGER =================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PCM API", Version = "v1" });
    
    // Cấu hình nút Authorize nhập Token trên Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập vào ô bên dưới: Bearer {token_của_bạn}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new string[] {}
        }
    });
});

// ================= BUILD APP =================
var app = builder.Build();


// ================= 7. AUTO MIGRATION & RESET DB =================
// Phần này sẽ tự động chạy mỗi khi App khởi động
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();

        // [CỰC KỲ QUAN TRỌNG]
        // Dòng này sẽ XÓA SẠCH database cũ để sửa lỗi "relation already exists".
        // Sau khi web chạy ngon lành, hãy xóa dòng này đi để tránh mất dữ liệu.
        db.Database.EnsureDeleted(); 

        // Tạo lại database mới tinh từ đầu
        db.Database.Migrate();
        Console.WriteLine("--> Database đã được Reset và Update thành công!");

        // Gọi hàm Seed dữ liệu (Tạo Admin, Member, Court...)
        Console.WriteLine(">>> CALLING SEED <<<");
        await DbInitializer.SeedAsync(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"--> Lỗi khi khởi tạo Database: {ex.Message}");
    }
}


// ================= 8. MIDDLEWARE PIPELINE =================

// Chỉ hiện Swagger ở môi trường Development hoặc nếu bạn muốn bật luôn trên Prod thì để ra ngoài if
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();