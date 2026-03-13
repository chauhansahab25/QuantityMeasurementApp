using QuantityMeasurementModelLayer.Enums;
namespace QuantityMeasurementApp.Model;

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

    public static double Convert(double value, U fromUnit, U toUnit)
    {
        if (fromUnit is VolumeUnit from && toUnit is VolumeUnit to)
        {
            return ConvertVolume(value, from, to);
        }
        if (fromUnit is LengthUnit fromLength && toUnit is LengthUnit toLength)
        {
            double baseValue = fromLength.ConvertToBaseUnit(value);
            return toLength.ConvertFromBaseUnit(baseValue);
        }
        if (fromUnit is WeightUnit fromWeight && toUnit is WeightUnit toWeight)
        {
            double baseValue = fromWeight.ConvertToBaseUnit(value);
            return toWeight.ConvertFromBaseUnit(baseValue);
        }
        if (fromUnit is TemperatureUnit fromTemp && toUnit is TemperatureUnit toTemp)
        {
            double baseValue = fromTemp.ConvertToBaseUnit(value);
            return toTemp.ConvertFromBaseUnit(baseValue);
        }
        
        throw new ArgumentException("Unsupported unit conversion");
    }

    private static double ConvertVolume(double value, VolumeUnit from, VolumeUnit to)
    {
        // Convert to base unit (litres) first
        double baseValue = value * from.ToBaseUnit();
        // Convert from base unit to target unit
        return baseValue / to.ToBaseUnit();
    }

    public override bool Equals(object obj)
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

    public Quantity<U> Add(Quantity<U> other, U targetUnit)
    {
        if (other == null)
            throw new ArgumentException("Quantity cannot be null");

        ValidateArithmetic("addition");

        double base1 = ConvertToBase(this.value, this.unit);
        double base2 = ConvertToBase(other.value, other.unit);

        double sum = base1 + base2;

        double result;
        if (targetUnit is LengthUnit l)
            result = l.ConvertFromBaseUnit(sum);
        else if (targetUnit is WeightUnit w)
            result = w.ConvertFromBaseUnit(sum);
        else if (targetUnit is VolumeUnit v)
            result = v.ConvertFromBaseUnit(sum);
        else if (targetUnit is TemperatureUnit t)
            result = t.ConvertFromBaseUnit(sum);
        else
            throw new ArgumentException("Unsupported unit type");

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

        double result;
        if (this.unit is LengthUnit l)
            result = l.ConvertFromBaseUnit(baseResult);
        else if (this.unit is WeightUnit w)
            result = w.ConvertFromBaseUnit(baseResult);
        else if (this.unit is VolumeUnit v)
            result = v.ConvertFromBaseUnit(baseResult);
        else if (this.unit is TemperatureUnit t)
            result = t.ConvertFromBaseUnit(baseResult);
        else
            throw new ArgumentException("Unsupported unit type");

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

        double result;
        if (targetUnit is LengthUnit l)
            result = l.ConvertFromBaseUnit(baseResult);
        else if (targetUnit is WeightUnit w)
            result = w.ConvertFromBaseUnit(baseResult);
        else if (targetUnit is VolumeUnit v)
            result = v.ConvertFromBaseUnit(baseResult);
        else if (targetUnit is TemperatureUnit t)
            result = t.ConvertFromBaseUnit(baseResult);
        else
            throw new ArgumentException("Unsupported unit type");

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

