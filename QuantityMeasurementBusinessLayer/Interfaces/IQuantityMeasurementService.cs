using QuantityMeasurementModelLayer.DTO;

namespace QuantityMeasurementBusinessLayer.Interfaces;

public interface IQuantityMeasurementService
{
    bool Compare(QuantityDTO q1, QuantityDTO q2);

    QuantityDTO Convert(QuantityDTO input, string targetUnit);

    QuantityDTO Add(QuantityDTO q1, QuantityDTO q2);

    QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2);

    double Divide(QuantityDTO q1, QuantityDTO q2);
}