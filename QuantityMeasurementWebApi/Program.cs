using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using QuantityMeasurementRepositoryLayer.Context;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementWebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Quantity Measurement API", 
        Version = "v1",
        Description = "REST API for Quantity Measurement operations with caching and database persistence"
    });
    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Configure Entity Framework with SQL Server (Docker)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=localhost,1434;Database=QuantityMeasurementDb;User Id=sa;Password=Glauniversity@123;TrustServerCertificate=true;MultipleActiveResultSets=true";

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("QuantityMeasurementWebApi")));

// Configure Redis Cache
var redisConnectionString = builder.Configuration.GetConnectionString("Redis") ?? "localhost:6380";
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnectionString;
    options.InstanceName = "QuantityMeasurement_";
});

// Register repositories
builder.Services.AddScoped<QuantityMeasurementRepositoryLayer.Interfaces.IQuantityMeasurementRepository, QuantityMeasurementRepositoryLayer.Repositories.QuantityMeasurementRepository>();

// Register services
builder.Services.AddScoped<QuantityMeasurementBusinessLayer.Services.IRedisCacheService, QuantityMeasurementBusinessLayer.Services.RedisCacheService>();

// Register business layer services
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

// Register security services
builder.Services.AddScoped<ISecurityService, SecurityService>();
builder.Services.AddScoped<IUserService, UserService>();

// Configure JWT Authentication
var jwtKey = builder.Configuration["JwtSettings:Key"] ?? throw new InvalidOperationException("JWT Key not configured");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "QuantityMeasurementAPI";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "QuantityMeasurementUsers";

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

// Add basic health checks
builder.Services.AddHealthChecks()
    .AddRedis(redisConnectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
        c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
    });
}

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Add security headers middleware
app.UseMiddleware<SecurityHeadersMiddleware>();

// Add rate limiting middleware
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Add JWT authentication middleware
app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Add health check endpoint
app.MapHealthChecks("/health");

// Add a simple root endpoint
app.MapGet("/", () => new
{
    Message = "Quantity Measurement API",
    Version = "v1",
    Endpoints = new
    {
        Swagger = "/swagger",
        Health = "/health",
        Operations = "/api/v1/quantitymeasurement",
        Auth = "/api/v1/auth"
    }
});

app.Run();
