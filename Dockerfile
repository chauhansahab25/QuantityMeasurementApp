# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files and restore as distinct layers
COPY ["QuantityMeasurementWebApi/QuantityMeasurementWebApi.csproj", "QuantityMeasurementWebApi/"]
COPY ["QuantityMeasurementBusinessLayer/QuantityMeasurementBusinessLayer.csproj", "QuantityMeasurementBusinessLayer/"]
COPY ["QuantityMeasurementRepositoryLayer/QuantityMeasurementRepositoryLayer.csproj", "QuantityMeasurementRepositoryLayer/"]
COPY ["QuantityMeasurementModelLayer/QuantityMeasurementModelLayer.csproj", "QuantityMeasurementModelLayer/"]

RUN dotnet restore "QuantityMeasurementWebApi/QuantityMeasurementWebApi.csproj"

# Copy everything else and build app
COPY . .
WORKDIR "/src/QuantityMeasurementWebApi"
RUN dotnet build "QuantityMeasurementWebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "QuantityMeasurementWebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Expose port 80 and 443
EXPOSE 80
EXPOSE 443

# Environment variables will be provided by Render
ENTRYPOINT ["dotnet", "QuantityMeasurementWebApi.dll"]
