using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuantityMeasurementService.Context;
using QuantityMeasurementService.Repositories;
using QuantityMeasurementService.Interfaces;
using QuantityMeasurementService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

// Configure Swagger/OpenAPI (Removed due to Swashbuckle .NET 10 incompatibility)

// Configure Entity Framework with SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost,1434;Database=QuantityMeasurementDb;User Id=sa;Password=Glauniversity@123;TrustServerCertificate=true;MultipleActiveResultSets=true";

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("QuantityMeasurementService")));

// Configure Redis Cache
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6380";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "QuantityMeasurement_";
});

// Register repositories
builder.Services.AddScoped<IQuantityMeasurementRepository, QuantityMeasurementRepository>();

// Register services
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "QuantityMeasurementAPI";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "QuantityMeasurementUsers";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = true,
            ValidAudience = jwtAudience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization
builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
}


app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add health check endpoint
app.MapHealthChecks("/health");

app.MapGet("/", () => new
{
    Message = "Quantity Measurement Service",
    Version = "v1",
    Endpoints = new
    {
        Swagger = "/swagger",
        Health = "/health",
        Operations = "/api/v1/quantitymeasurement"
    }
});

app.Run();
