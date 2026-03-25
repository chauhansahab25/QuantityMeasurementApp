using System;
using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Extensions;

namespace QuantityMeasurementModelLayer.Models;

public class QuantityWeight
{
    public double Value { get; }
    public WeightUnit Unit { get; }
    private const double EPSILON = 0.00001;

    public QuantityWeight(double value, WeightUnit unit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        Value = value;
        Unit = unit;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (GetType() != obj.GetType()) return false;

        QuantityWeight other = (QuantityWeight)obj;
        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);

        return Math.Abs(base1 - base2) < EPSILON;
    }

    public override int GetHashCode()
    {
        return Unit.ConvertToBaseUnit(Value).GetHashCode();
    }

    public override string ToString()
    {
        return $"{Value} {Unit}";
    }

    // Static method for backward compatibility - delegates to WeightService
    public static double Convert(double value, WeightUnit fromUnit, WeightUnit toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        double baseValue = fromUnit.ConvertToBaseUnit(value);
        return toUnit.ConvertFromBaseUnit(baseValue);
    }

    // Instance methods for backward compatibility - delegate to WeightService
    public QuantityWeight Add(QuantityWeight other)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);
        double sum = base1 + base2;
        double result = Unit.ConvertFromBaseUnit(sum);
        
        return new QuantityWeight(result, Unit);
    }

    public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);
        double sum = base1 + base2;
        double result = targetUnit.ConvertFromBaseUnit(sum);
        
        return new QuantityWeight(result, targetUnit);
    }

    public QuantityWeight Subtract(QuantityWeight other)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);
        double difference = base1 - base2;
        double result = Unit.ConvertFromBaseUnit(difference);
        
        return new QuantityWeight(result, Unit);
    }

    public QuantityWeight Subtract(QuantityWeight other, WeightUnit targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);
        double difference = base1 - base2;
        double result = targetUnit.ConvertFromBaseUnit(difference);
        
        return new QuantityWeight(result, targetUnit);
    }

    public double Divide(QuantityWeight other)
    {
        if (other == null)
            throw new ArgumentException("Operand cannot be null");

        double base1 = Unit.ConvertToBaseUnit(Value);
        double base2 = other.Unit.ConvertToBaseUnit(other.Value);
        
        if (Math.Abs(base2) < EPSILON)
            throw new ArithmeticException("Division by zero");
            
        return base1 / base2;
    }

    // ConvertTo method for backward compatibility
    public QuantityWeight ConvertTo(WeightUnit targetUnit)
    {
        double baseValue = Unit.ConvertToBaseUnit(Value);
        double convertedValue = targetUnit.ConvertFromBaseUnit(baseValue);
        
        return new QuantityWeight(convertedValue, targetUnit);
    }
}
