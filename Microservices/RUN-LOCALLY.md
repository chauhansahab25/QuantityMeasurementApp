# Running Microservices Locally

If Docker Compose is not working, try running services locally:

## Prerequisites
- .NET 10.0 SDK
- SQL Server (localhost:1434)
- Redis (localhost:6380)

## Start Services

### 1. Start SQL Server and Redis with Docker
```bash
docker-compose -f docker-compose.windows.yml up sqlserver redis -d
```

### 2. Run Services in Separate Terminals

#### Terminal 1 - API Gateway
```bash
cd QuantityMeasurementGateway
dotnet run
```

#### Terminal 2 - Authentication Service
```bash
cd AuthenticationService
dotnet run
```

#### Terminal 3 - Quantity Measurement Service
```bash
cd QuantityMeasurementService
dotnet run
```

## Access Points
- API Gateway: http://localhost:5000
- Auth Service: http://localhost:5001
- Quantity Service: http://localhost:5002

## Troubleshooting Docker Issues

If Docker Desktop is not working:
1. Restart Docker Desktop
2. Check if Docker is running: `docker --version`
3. Try: `docker system prune -f`
4. Use Windows containers instead of Linux if needed
