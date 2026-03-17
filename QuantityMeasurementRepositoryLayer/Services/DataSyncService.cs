using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementModelLayer.Entities;

namespace QuantityMeasurementRepositoryLayer.Services;

public class DataSyncService
{
    private readonly ICacheRepository cacheRepository;
    private readonly IQuantityMeasurementRepository databaseRepository;

    public DataSyncService(ICacheRepository cacheRepository, IQuantityMeasurementRepository databaseRepository)
    {
        this.cacheRepository = cacheRepository;
        this.databaseRepository = databaseRepository;
    }

    public bool UploadPendingDataToDatabase(bool silent = false)
    {
        try
        {
            if (!cacheRepository.HasPendingData())
            {
                if (!silent) Console.WriteLine("No pending data to upload.");
                return true;
            }

            var pendingData = cacheRepository.GetPendingData();

            // Upload all records - let SQL Server handle IDENTITY automatically
            foreach (var entity in pendingData)
            {
                // Don't assign ID manually - let SQL Server IDENTITY handle it
                databaseRepository.Save(entity);
            }

            // Only clear cache after ALL records are successfully uploaded
            cacheRepository.ClearPendingData();
            if (!silent)
            {
            }
            return true;
        }
        catch (Exception ex)
        {
            if (!silent) Console.WriteLine($"Error uploading data to database");
            // DO NOT clear cache on failure - data remains in JSON file
            return false;
        }
    }

    public int GetPendingDataCount()
    {
        return cacheRepository.HasPendingData() ? cacheRepository.GetPendingData().Count : 0;
    }

    public void DisplayPendingData()
    {
        if (!cacheRepository.HasPendingData())
        {
            Console.WriteLine("No pending data in cache.");
            return;
        }

        var pendingData = cacheRepository.GetPendingData();
        Console.WriteLine($"\nPending data in cache ({pendingData.Count} records):");
        Console.WriteLine("=".PadRight(50, '='));

        foreach (var entity in pendingData)
        {
            Console.WriteLine($"Operation: {entity.FirstValue} {entity.FirstUnit} {entity.Operation} {entity.SecondValue} {entity.SecondUnit} = {entity.Result}");
        }
        Console.WriteLine("=".PadRight(50, '='));
    }

    public bool CheckDatabaseConnectivity()
    {
        try
        {
            // Try to get count from database to test connectivity
            databaseRepository.GetTotalCount();
            return true;
        }
        catch
        {
            return false;
        }
    }
}