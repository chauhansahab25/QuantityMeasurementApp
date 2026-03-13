using QuantityMeasurementModelLayer.Entities;
using QuantityMeasurementRepositoryLayer.Interfaces;

namespace QuantityMeasurementRepositoryLayer.Repositories;

public class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository
{
    private readonly List<QuantityMeasurementEntity> cache = new();

    public void Save(QuantityMeasurementEntity entity)
    {
        cache.Add(entity);
    }

    public List<QuantityMeasurementEntity> GetAll()
    {
        return cache;
    }
}