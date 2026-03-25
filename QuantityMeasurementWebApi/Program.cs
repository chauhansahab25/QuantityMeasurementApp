using Microsoft.EntityFrameworkCore;
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
    
    // Include XML comments if available
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

app.UseHttpsRedirection();

app.UseCors("AllowAll");

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
        History = "/api/v1/history"
    }
});

app.Run();
