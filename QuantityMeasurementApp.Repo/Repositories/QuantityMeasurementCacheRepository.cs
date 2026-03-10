using QuantityMeasurementApp.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuantityMeasurementApp.Repo.Repositories
{
    public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
    {
        private static QuantityMeasurementCacheRepository _instance;
        private static readonly object _lock = new object();
        private readonly List<QuantityMeasurementEntity> _cache;
        private readonly string _filePath = "quantity_measurements.dat";

        private QuantityMeasurementCacheRepository()
        {
            _cache = new List<QuantityMeasurementEntity>();
            LoadFromDisk();
        }

        public static QuantityMeasurementCacheRepository GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new QuantityMeasurementCacheRepository();
                    }
                }
            }
            return _instance;
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _cache.Add(entity);
            SaveToDisk(entity);
        }

        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            return new List<QuantityMeasurementEntity>(_cache);
        }

        private void SaveToDisk(QuantityMeasurementEntity entity)
        {
            try
            {
                using (var stream = new FileStream(_filePath, FileMode.Append, FileAccess.Write))
                using (var writer = new StreamWriter(stream))
                {
                    writer.WriteLine($"{entity.OperationType}|{entity.Operand1Value}|{entity.Operand1Unit}|{entity.Operand2Value}|{entity.Operand2Unit}|{entity.ResultValue}|{entity.ResultUnit}|{entity.BooleanResult}|{entity.HasError}|{entity.ErrorMessage}|{entity.Timestamp}");
                }
            }
            catch (Exception)
            {
                // Silently fail for disk operations
            }
        }

        private void LoadFromDisk()
        {
            if (!File.Exists(_filePath)) return;

            try
            {
                var lines = File.ReadAllLines(_filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length >= 11)
                    {
                        var entity = new QuantityMeasurementEntity
                        {
                            OperationType = parts[0],
                            Operand1Value = string.IsNullOrEmpty(parts[1]) ? null : double.Parse(parts[1]),
                            Operand1Unit = parts[2],
                            Operand2Value = string.IsNullOrEmpty(parts[3]) ? null : double.Parse(parts[3]),
                            Operand2Unit = parts[4],
                            ResultValue = string.IsNullOrEmpty(parts[5]) ? null : double.Parse(parts[5]),
                            ResultUnit = parts[6],
                            BooleanResult = string.IsNullOrEmpty(parts[7]) ? null : bool.Parse(parts[7]),
                            HasError = bool.Parse(parts[8]),
                            ErrorMessage = parts[9],
                            Timestamp = DateTime.Parse(parts[10])
                        };
                        _cache.Add(entity);
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail for disk operations
            }
        }
    }
}
