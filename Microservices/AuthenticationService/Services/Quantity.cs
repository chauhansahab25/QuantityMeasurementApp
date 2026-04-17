using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Extensions;

namespace AuthenticationService.Services;

public class Quantity<U> where U : struct
{
    private readonly double value;
    private readonly U unit;
    private const double EPSILON = 0.000001;

    public Quantity(double value, U unit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        this.value = value;
        this.unit = unit;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || !(obj is Quantity<U>))
            return false;

        Quantity<U> other = (Quantity<U>)obj;

        double base1 = ConvertToBase(this.value, this.unit);
        double base2 = ConvertToBase(other.value, other.unit);

        return Math.Abs(base1 - base2) < EPSILON;
    }

    private double ConvertToBase(double value, U unit)
    {
        if (unit is LengthUnit l)
            return l.ConvertToBaseUnit(value);

        if (unit is WeightUnit w)
            return w.ConvertToBaseUnit(value);

        if (unit is VolumeUnit v)
            return v.ConvertToBaseUnit(value);

        if (unit is TemperatureUnit t)
            return t.ConvertToBaseUnit(value);

        throw new ArgumentException("Unsupported unit type");
    }

    private void ValidateArithmetic(string operation)
    {
        if (unit is TemperatureUnit t)
            t.ValidateOperationSupport(operation);
    }

    public Quantity<U> ConvertTo(U targetUnit)
    {
        double baseValue = ConvertToBase(this.value, this.unit);

        double converted;

        if (targetUnit is LengthUnit l)
            converted = l.ConvertFromBaseUnit(baseValue);
        else if (targetUnit is WeightUnit w)
            converted = w.ConvertFromBaseUnit(baseValue);
        else if (targetUnit is VolumeUnit v)
            converted = v.ConvertFromBaseUnit(baseValue);
        else if (targetUnit is TemperatureUnit t)
            converted = t.ConvertFromBaseUnit(baseValue);
        else
            throw new ArgumentException("Unsupported unit type");

        return new Quantity<U>(converted, targetUnit);
    }

    // Static conversion method for tests
    public static double Convert(double value, U fromUnit, U toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        // Convert to base unit first
        double baseValue;
        if (fromUnit is LengthUnit l1)
            baseValue = l1.ConvertToBaseUnit(value);
        else if (fromUnit is WeightUnit w1)
            baseValue = w1.ConvertToBaseUnit(value);
        else if (fromUnit is VolumeUnit v1)
            baseValue = v1.ConvertToBaseUnit(value);
        else if (fromUnit is TemperatureUnit t1)
            baseValue = t1.ConvertToBaseUnit(value);
        else
            throw new ArgumentException("Unsupported unit type");

        // Convert from base unit to target unit
        if (toUnit is LengthUnit l2)
            return l2.ConvertFromBaseUnit(baseValue);
        else if (toUnit is WeightUnit w2)
            return w2.ConvertFromBaseUnit(baseValue);
        else if (toUnit is VolumeUnit v2)
            return v2.ConvertFromBaseUnit(baseValue);
        else if (toUnit is TemperatureUnit t2)
            return t2.ConvertFromBaseUnit(baseValue);
        else
            throw new ArgumentException("Unsupported unit type");
    }

    public Quantity<U> Add(Quantity<U> other, U targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        ValidateArithmetic("addition");

        double base1 = ConvertToBase(this.value, this.unit);
        double base2 = ConvertToBase(other.value, other.unit);

        double sum = base1 + base2;

        dynamic t = targetUnit;
        double result;
        
        if (t is VolumeUnit v)
            result = v.ConvertFromBaseUnit(sum);
        else
            result = t.ConvertFromBaseUnit(sum);

        return new Quantity<U>(result, targetUnit);
    }

    public Quantity<U> Subtract(Quantity<U> other)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        ValidateArithmetic("subtraction");

        double baseValue1 = ConvertToBase(this.value, this.unit);
        double baseValue2 = ConvertToBase(other.value, other.unit);

        double baseResult = baseValue1 - baseValue2;

        dynamic u = this.unit;
        double result;
        
        if (u is VolumeUnit v)
            result = v.ConvertFromBaseUnit(baseResult);
        else
            result = u.ConvertFromBaseUnit(baseResult);

        result = Math.Round(result, 2);

        return new Quantity<U>(result, unit);
    }


    public Quantity<U> Subtract(Quantity<U> other, U targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        ValidateArithmetic("subtraction");

        double baseValue1 = ConvertToBase(this.value, this.unit);
        double baseValue2 = ConvertToBase(other.value, other.unit);

        double baseResult = baseValue1 - baseValue2;

        dynamic t = targetUnit;
        double result;
        
        if (t is VolumeUnit v)
            result = v.ConvertFromBaseUnit(baseResult);
        else
            result = t.ConvertFromBaseUnit(baseResult);

        result = Math.Round(result, 2);

        return new Quantity<U>(result, targetUnit);
    }

    public double Divide(Quantity<U> other)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        ValidateArithmetic("division");

        double baseValue1 = ConvertToBase(this.value, this.unit);
        double baseValue2 = ConvertToBase(other.value, other.unit);

        if (baseValue2 == 0)
            throw new ArithmeticException("Division by zero");

        return baseValue1 / baseValue2;
    }


    public override int GetHashCode()
    {
        double baseValue = ConvertToBase(value, unit);
        return baseValue.GetHashCode();
    }

    public override string ToString()
    {
        return $"Quantity({value}, {unit})";
    }
}

