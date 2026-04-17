using AuthenticationService.Interfaces;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementModelLayer.Entities;
using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Extensions;

namespace AuthenticationService.Services;

public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
{
    private readonly IQuantityMeasurementRepository repository;

    public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repo)
    {
        repository = repo;
    }

    // 🔹 Convert base value back to specific unit
    private double ConvertFromBase(double baseValue, string unit)
    {
        if (Enum.TryParse(unit, out LengthUnit length))
            return baseValue / length.GetConversionFactor();

        if (Enum.TryParse(unit, out WeightUnit weight))
            return baseValue / weight.GetConversionFactor();

        if (Enum.TryParse(unit, out VolumeUnit volume))
            return baseValue / volume.ToBaseUnit();

        if (Enum.TryParse(unit, out TemperatureUnit temp))
        {
            switch (temp)
            {
                case TemperatureUnit.CELSIUS:
                    return baseValue;

                case TemperatureUnit.FAHRENHEIT:
                    return baseValue * 9 / 5 + 32;
            }
        }

        throw new ArgumentException("Unsupported unit");
    }

    // 🔹 Convert unit to base value
    private double ConvertToBase(double value, string unit)
    {
        if (Enum.TryParse(unit, out LengthUnit length))
            return value * length.GetConversionFactor();

        if (Enum.TryParse(unit, out WeightUnit weight))
            return value * weight.GetConversionFactor();

        if (Enum.TryParse(unit, out VolumeUnit volume))
            return value * volume.ToBaseUnit();

        if (Enum.TryParse(unit, out TemperatureUnit temp))
        {
            switch (temp)
            {
                case TemperatureUnit.CELSIUS:
                    return value;

                case TemperatureUnit.FAHRENHEIT:
                    return (value - 32) * 5 / 9;
            }
        }

        throw new ArgumentException("Unsupported unit");
    }

    // 🔹 Helper method to store operations
    private void SaveOperation(double v1, string u1, double v2, string u2, string operation, double result)
    {
        QuantityMeasurementEntity entity = new QuantityMeasurementEntity(
            v1,
            u1,
            v2,
            u2,
            operation,
            result,
            "Measurement"
        );

        repository.Save(entity);
    }

    public bool Compare(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);

        bool result = v1 == v2;

        SaveOperation(q1.Value, q1.Unit, q2.Value, q2.Unit, "COMPARE", result ? 1 : 0);

        return result;
    }

    public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);

        double resultInBase = v1 + v2;
        double result = ConvertFromBase(resultInBase, q1.Unit);

        SaveOperation(q1.Value, q1.Unit, q2.Value, q2.Unit, "ADD", result);

        return new QuantityDTO(result, q1.Unit);
    }

    public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);

        double resultInBase = v1 - v2;
        double result = ConvertFromBase(resultInBase, q1.Unit);

        SaveOperation(q1.Value, q1.Unit, q2.Value, q2.Unit, "SUBTRACT", result);

        return new QuantityDTO(result, q1.Unit);
    }

    public double Divide(QuantityDTO q1, QuantityDTO q2)
    {
        double v1 = ConvertToBase(q1.Value, q1.Unit);
        double v2 = ConvertToBase(q2.Value, q2.Unit);

        double result = v1 / v2;

        SaveOperation(q1.Value, q1.Unit, q2.Value, q2.Unit, "DIVIDE", result);

        return result;
    }

    public QuantityDTO Convert(QuantityDTO input, string targetUnit)
    {
        double baseValue = ConvertToBase(input.Value, input.Unit);
        double result;

        if (Enum.TryParse(targetUnit, out LengthUnit length))
            result = baseValue / length.GetConversionFactor();

        else if (Enum.TryParse(targetUnit, out WeightUnit weight))
            result = baseValue / weight.GetConversionFactor();

        else if (Enum.TryParse(targetUnit, out VolumeUnit volume))
            result = baseValue / volume.ToBaseUnit();

        else if (Enum.TryParse(targetUnit, out TemperatureUnit temp))
        {
            switch (temp)
            {
                case TemperatureUnit.CELSIUS:
                    result = baseValue;
                    break;

                case TemperatureUnit.FAHRENHEIT:
                    result = baseValue * 9 / 5 + 32;
                    break;

                default:
                    throw new ArgumentException("Unsupported temperature unit");
            }
        }
        else
        {
            throw new ArgumentException("Unsupported target unit");
        }

        SaveOperation(input.Value, input.Unit, 0, targetUnit, "CONVERT", result);

        return new QuantityDTO(result, targetUnit);
    }
}