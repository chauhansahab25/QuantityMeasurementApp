using QuantityMeasurementApp.Repo.Repositories;
using QuantityMeasurementApp.Bussiness.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// Register N-Tier architecture dependencies
builder.Services.AddSingleton<IQuantityMeasurementRepository>(QuantityMeasurementCacheRepository.GetInstance());
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
