# Quantity Measurement API - UC17 Implementation

This is a modern C# .NET Web API implementation of the Quantity Measurement Application with UC17 requirements, featuring Entity Framework, Redis caching, and Swagger documentation.

## Features

- **RESTful API**: Full CRUD operations for quantity measurements
- **Entity Framework Core**: Modern ORM with SQL Server database
- **Redis Caching**: High-performance caching for frequently accessed data
- **Swagger/OpenAPI**: Interactive API documentation
- **Docker Support**: Containerized database and cache services
- **Global Exception Handling**: Centralized error management
- **Health Checks**: Monitoring endpoints for system health

## Architecture

- **Web API Layer**: REST controllers and DTOs
- **Business Layer**: Core business logic and services
- **Repository Layer**: Data access with Entity Framework
- **Model Layer**: Entities and domain models
- **Caching Layer**: Redis integration for performance

## Prerequisites

- .NET 10.0 SDK
- Docker Desktop
- Visual Studio 2022 or VS Code

## Quick Start

### 1. Start Docker Services

```bash
cd e:\QuantityMeasurementApp
docker-compose up -d
```

This will start:
- SQL Server on localhost:1433
- Redis on localhost:6379

### 2. Apply Database Migrations

```bash
cd QuantityMeasurementWebApi
dotnet ef database update
```

### 3. Run the Application

```bash
dotnet run
```

The API will be available at:
- **Swagger UI**: https://localhost:5212
- **API Base**: https://localhost:5212/api/v1
- **Health Check**: https://localhost:5212/health

## API Endpoints

### Quantity Measurement Operations

#### Compare Quantities
```http
POST /api/v1/quantitymeasurement/compare
Content-Type: application/json

{
  "firstValue": 1,
  "firstUnit": "FEET",
  "secondValue": 12,
  "secondUnit": "INCHES",
  "operation": "COMPARE",
  "measurementType": "LengthUnit"
}
```

#### Convert Units
```http
POST /api/v1/quantitymeasurement/convert?targetUnit=METERS
Content-Type: application/json

{
  "firstValue": 100,
  "firstUnit": "CENTIMETERS",
  "secondValue": 0,
  "secondUnit": "",
  "operation": "CONVERT",
  "measurementType": "LengthUnit"
}
```

#### Add Quantities
```http
POST /api/v1/quantitymeasurement/add
Content-Type: application/json

{
  "firstValue": 1,
  "firstUnit": "METERS",
  "secondValue": 50,
  "secondUnit": "CENTIMETERS",
  "operation": "ADD",
  "measurementType": "LengthUnit"
}
```

#### Subtract Quantities
```http
POST /api/v1/quantitymeasurement/subtract
Content-Type: application/json

{
  "firstValue": 2,
  "firstUnit": "METERS",
  "secondValue": 100,
  "secondUnit": "CENTIMETERS",
  "operation": "SUBTRACT",
  "measurementType": "LengthUnit"
}
```

#### Divide Quantities
```http
POST /api/v1/quantitymeasurement/divide
Content-Type: application/json

{
  "firstValue": 100,
  "firstUnit": "METERS",
  "secondValue": 2,
  "secondUnit": "",
  "operation": "DIVIDE",
  "measurementType": "LengthUnit"
}
```

### History Management

#### Get All Measurements
```http
GET /api/v1/history
```

#### Get by Operation Type
```http
GET /api/v1/history/operation/ADD
```

#### Get by Measurement Type
```http
GET /api/v1/history/type/LengthUnit
```

#### Get by Date Range
```http
GET /api/v1/history/daterange?startDate=2024-01-01T00:00:00Z&endDate=2024-12-31T23:59:59Z
```

#### Get Errored Measurements
```http
GET /api/v1/history/errors
```

#### Get Database Health
```http
GET /api/v1/history/health
```

## Supported Units

### LengthUnit
- FEET, INCHES, CENTIMETERS, METERS, KILOMETERS, YARD, MILE

### VolumeUnit  
- GALLON, LITERS, MILLILITERS, CUBIC_FEET, CUBIC_METER

### WeightUnit
- KILOGRAM, GRAM, POUND, OUNCE, TON

### TemperatureUnit
- CELSIUS, FAHRENHEIT

## Configuration

### Database Connection
Update `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=QuantityMeasurementDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;MultipleActiveResultSets=true",
    "Redis": "localhost:6379"
  }
}
```

### Cache Settings
```json
{
  "CacheSettings": {
    "DefaultExpirationMinutes": 30,
    "EnableCaching": true
  }
}
```

## Development

### Add New Migration
```bash
dotnet ef migrations add MigrationName
```

### Update Database
```bash
dotnet ef database update
```

### Run Tests
```bash
dotnet test
```

## Docker Services

### SQL Server
- **Image**: mcr.microsoft.com/mssql/server:2022-latest
- **Port**: 1433
- **Password**: YourStrong@Passw0rd
- **Database**: QuantityMeasurementDb

### Redis
- **Image**: redis:7-alpine
- **Port**: 6379
- **Data Persistence**: Enabled

## Monitoring

### Health Endpoints
- **Application Health**: `/health`
- **Database Health**: `/api/v1/history/health`

### Logging
- Structured logging with Serilog
- Database query logging enabled
- Request/response logging

## Performance Features

- **Redis Caching**: 30-minute default expiration
- **Database Indexing**: Optimized queries on Operation, MeasurementType, CreatedAt
- **Connection Pooling**: Efficient database connections
- **Async Operations**: Non-blocking I/O throughout

## Security

- **CORS Configuration**: Configurable for production
- **Input Validation**: DTO validation attributes
- **SQL Injection Prevention**: Entity Framework parameterization
- **Error Handling**: No sensitive information leakage

## Production Deployment

1. Update connection strings for production databases
2. Configure Redis cluster for high availability
3. Set up proper SSL certificates
4. Configure monitoring and logging
5. Set up backup strategies for SQL Server

## Troubleshooting

### Database Connection Issues
- Ensure Docker containers are running: `docker-compose ps`
- Check SQL Server health: `docker logs quantitymeasurement-sqlserver`
- Verify connection string in appsettings.json

### Redis Connection Issues
- Check Redis container: `docker logs quantitymeasurement-redis`
- Test Redis connection: `redis-cli ping`

### Migration Issues
- Drop and recreate database: `dotnet ef database drop`
- Recreate migrations: `dotnet ef migrations add InitialCreate`
- Apply migrations: `dotnet ef database update`
