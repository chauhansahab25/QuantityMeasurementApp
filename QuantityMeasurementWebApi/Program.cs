using Microsoft.EntityFrameworkCore;
using QuantityMeasurementRepositoryLayer.Context;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementWebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddRouting(options => 
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});


builder.Services.AddEndpointsApiExplorer();// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { 
        Title = "Quantity Measurement API", 
        Version = "v1",
        Description = "REST API for Quantity Measurement operations with caching and database persistence"
    });
    
    
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";// Include XML comments if available
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Support Render's DATABASE_URL if provided
var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(databaseUrl))
{
    try
    {
        // Parse postgres://user:password@host:port/database mapping to Npgsql connection string
        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':');
        var dbPort = uri.Port > 0 ? uri.Port : 5432; // Default PostgreSQL port if not specified
        var password = userInfo.Length > 1 ? userInfo[1] : "";
        var database = uri.AbsolutePath.TrimStart('/').Split('?')[0];
        
        Console.WriteLine($"DEBUG: Parsed DATABASE_URL - Host={uri.Host}, Port={dbPort}, Database={database}, User={userInfo[0]}, HasPassword={!string.IsNullOrEmpty(password)}");
        
        connectionString = $"Host={uri.Host};Port={dbPort};Database={database};Username={userInfo[0]};Password={password};SSL Mode=Require;Trust Server Certificate=true";
    }
    catch (Exception ex)
    {
        Console.WriteLine($"ERROR: Failed to parse DATABASE_URL: {ex.Message}");
        throw;
    }
}
else if (string.IsNullOrEmpty(connectionString))
{
    connectionString = "Host=localhost;Database=QuantityMeasurementDb;Username=postgres;Password=postgres";
}

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("QuantityMeasurementWebApi")));

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
builder.Services.AddScoped<QuantityMeasurementBusinessLayer.Services.IUserService, QuantityMeasurementBusinessLayer.Services.UserService>();
builder.Services.AddScoped<QuantityMeasurementBusinessLayer.Services.ISecurityService, QuantityMeasurementBusinessLayer.Services.SecurityService>();

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

// Automatically apply database migrations on startup
using (var scope = app.Services.CreateScope())
{
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<QuantityMeasurementDbContext>();
        dbContext.Database.Migrate();
        // Since we don't have access to the logger here easily, we'll write to console for Render logs
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
        if (ex.InnerException != null)
        {
            Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
        }
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
    }
}

// Configure the HTTP request pipeline.
// Enable Swagger even in Production for debugging purposes
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quantity Measurement API v1");
    c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
});

// Add global exception handling middleware
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Render handles HTTPS at the load balancer level
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

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
        Auth = "/api/v1/auth"
    }
});
// Use PORT environment variable if provided by Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
app.Run($"http://0.0.0.0:{port}");