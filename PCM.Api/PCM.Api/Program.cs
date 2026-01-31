using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using PCM.Api.Data;
using PCM.Api.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ================= PORT (RAILWAY) =================

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://*:{port}");

// ================= DATABASE (POSTGRES) =================

var databaseUrl = Environment.GetEnvironmentVariable("postgresql://postgres:PymXmglHbWCokXWNvmKGCuaBXSvkcBtY@postgres-rz9n.railway.internal:5432/railway");

string connectionString;

if (!string.IsNullOrEmpty(databaseUrl))
{
    var uri = new Uri(databaseUrl);
    var userInfo = uri.UserInfo.Split(':');

    var csb = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo[1],
        Database = uri.AbsolutePath.TrimStart('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    connectionString = csb.ConnectionString;
}
else
{
    // Local fallback (chạy local)
    connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection")!;
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// ================= IDENTITY =================

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// ================= SERVICES =================

builder.Services.AddScoped<IMatchService, MatchService>();

// ================= JWT CONFIG =================

var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key");

if (string.IsNullOrEmpty(jwtKey))
{
    jwtKey = "PCM_DEFAULT_SECRET_KEY_123456789_ABC";
}

var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") ?? "PCM.Api";
var jwtAudience = builder.Configuration.GetValue<string>("Jwt:Audience") ?? "PCM.Client";

// ================= AUTH =================

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

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey))
    };
});

// ================= CONTROLLERS =================

builder.Services.AddControllers();

// ================= CORS =================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// ================= SWAGGER =================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "PCM API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new()
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: Bearer {your_token}"
    });

    options.AddSecurityRequirement(new()
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// ================= MIDDLEWARE =================

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
