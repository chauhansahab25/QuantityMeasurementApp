using QuantityMeasurementApp.Model.DTOs;

namespace QuantityMeasurementApp.Bussiness.Services
{
    public interface IQuantityMeasurementService
    {
        QuantityDTO Compare(QuantityDTO quantity1, QuantityDTO quantity2);
        QuantityDTO Convert(QuantityDTO quantity, string targetUnit);
        QuantityDTO Add(QuantityDTO quantity1, QuantityDTO quantity2);
        QuantityDTO Subtract(QuantityDTO quantity1, QuantityDTO quantity2);
        QuantityDTO Divide(QuantityDTO quantity1, QuantityDTO quantity2);
    }
}
