using QuantityMeasurementModelLayer.Entities;
using QuantityMeasurementRepositoryLayer.Interfaces;
using System.Text.Json;

namespace QuantityMeasurementRepositoryLayer.Repositories;

public class QuantityMeasurementCacheRepository : ICacheRepository
{
    private readonly List<QuantityMeasurementEntity> cache = new();
    private readonly string jsonFilePath = "cache_data.json";

    public QuantityMeasurementCacheRepository()
    {
        LoadFromJsonFile();
    }

    public void Save(QuantityMeasurementEntity entity)
    {
        // Reset ID to 0 so SQL Server can assign proper IDENTITY value
        entity.Id = 0;
        cache.Add(entity);
        SaveToJsonFile();
    }

    public List<QuantityMeasurementEntity> GetAll()
    {
        return cache;
    }

    public List<QuantityMeasurementEntity> GetByOperation(string operation)
    {
        List<QuantityMeasurementEntity> result = new List<QuantityMeasurementEntity>();

        foreach (var measurement in cache)
        {
            if (measurement.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(measurement);
            }
        }

        return result;
    }

    public List<QuantityMeasurementEntity> GetByMeasurementType(string measurementType)
    {
        List<QuantityMeasurementEntity> result = new List<QuantityMeasurementEntity>();

        foreach (var measurement in cache)
        {
            if (measurement.MeasurementType.Equals(measurementType, StringComparison.OrdinalIgnoreCase))
            {
                result.Add(measurement);
            }
        }

        return result;
    }

    public int GetTotalCount()
    {
        return cache.Count;
    }

    public void DeleteAll()
    {
        cache.Clear();
        SaveToJsonFile(); // Save empty array to JSON, don't delete file
    }

    public bool OperationExists(double firstValue, string firstUnit, double secondValue, string secondUnit, string operation)
    {
        return cache.Any(m => 
            Math.Abs(m.FirstValue - firstValue) < 0.0001 &&
            m.FirstUnit.Equals(firstUnit, StringComparison.OrdinalIgnoreCase) &&
            Math.Abs(m.SecondValue - secondValue) < 0.0001 &&
            m.SecondUnit.Equals(secondUnit, StringComparison.OrdinalIgnoreCase) &&
            m.Operation.Equals(operation, StringComparison.OrdinalIgnoreCase));
    }

    public QuantityMeasurementEntity GetLastSavedOperation()
    {
        return cache.LastOrDefault() ?? new QuantityMeasurementEntity();
    }

    public bool TestConnection()
    {
        // Cache repository is always "connected"
        return true;
    }

    public void ResetIdentity()
    {
        // Cache doesn't use IDENTITY, so nothing to reset
        Console.WriteLine("🔧 Cache repository: No IDENTITY to reset");
    }

    private void LoadFromJsonFile()
    {
        try
        {
            if (File.Exists(jsonFilePath))
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    var loadedData = JsonSerializer.Deserialize<List<QuantityMeasurementEntity>>(jsonString);
                    if (loadedData != null)
                    {
                        // Reset all IDs to 0 to prevent IDENTITY conflicts when uploading to database
                        foreach (var entity in loadedData)
                        {
                            entity.Id = 0;
                        }
                        cache.Clear();
                        cache.AddRange(loadedData);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading from JSON file: {ex.Message}");
        }
    }

    private void SaveToJsonFile()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            string jsonString = JsonSerializer.Serialize(cache, options);
            File.WriteAllText(jsonFilePath, jsonString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving to JSON file: {ex.Message}");
        }
    }

    public void ClearJsonFile()
    {
        try
        {
            // Don't delete file, just save empty array
            cache.Clear();
            SaveToJsonFile();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing JSON file: {ex.Message}");
        }
    }

    public bool HasPendingData()
    {
        return cache.Count > 0;
    }

    public List<QuantityMeasurementEntity> GetPendingData()
    {
        return new List<QuantityMeasurementEntity>(cache);
    }

    public void ClearPendingData()
    {
        // Only clear cache and JSON when explicitly called after successful upload
        cache.Clear();
        SaveToJsonFile(); // Save empty array, keep file
    }

    public void ClearPendingDataOnlyAfterSuccessfulUpload()
    {
        // This method should only be called after successful database upload
        cache.Clear();
        SaveToJsonFile();
    }
}