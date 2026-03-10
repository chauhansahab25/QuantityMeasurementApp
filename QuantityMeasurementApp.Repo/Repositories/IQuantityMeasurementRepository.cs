using QuantityMeasurementApp.Model.Entities;
using System.Collections.Generic;

namespace QuantityMeasurementApp.Repo.Repositories
{
    public interface IQuantityMeasurementRepository
    {
        void Save(QuantityMeasurementEntity entity);
        List<QuantityMeasurementEntity> GetAllMeasurements();
    }
}
