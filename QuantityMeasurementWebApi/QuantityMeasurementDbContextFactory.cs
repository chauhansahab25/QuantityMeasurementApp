using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using QuantityMeasurementRepositoryLayer.Context;

namespace QuantityMeasurementWebApi;

public class QuantityMeasurementDbContextFactory : IDesignTimeDbContextFactory<QuantityMeasurementDbContext>
{
    public QuantityMeasurementDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<QuantityMeasurementDbContext>();
        optionsBuilder.UseNpgsql(
            "Host=localhost;Database=QuantityMeasurementDb;Username=postgres;Password=postgres",
            b => b.MigrationsAssembly("QuantityMeasurementWebApi"));
        
        return new QuantityMeasurementDbContext(optionsBuilder.Options);
    }
}
