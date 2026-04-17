FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["QuantityMeasurementGateway.csproj", "."]
RUN dotnet restore "./QuantityMeasurementGateway.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "QuantityMeasurementGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "QuantityMeasurementGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "QuantityMeasurementGateway.dll"]
