using System;
using QuantityMeasurementModelLayer.Enums;
namespace QuantityMeasurementApp.Model;

public class QuantityWeight
{
    private readonly double value;
    private readonly WeightUnit unit;
    private const double EPSILON = 0.00001;

    public QuantityWeight(double value, WeightUnit unit)
    {
        if (unit == null)
            throw new ArgumentException("Unit cannot be null");

        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        this.value = value;
        this.unit = unit;
    }

    public static double Convert(double value, WeightUnit fromUnit, WeightUnit toUnit)
    {
        if (fromUnit == null || toUnit == null)
            throw new ArgumentException("Units cannot be null");

        // Convert to base unit (kilograms) first
        double baseValue = fromUnit.ConvertToBaseUnit(value);
        // Convert from base unit to target unit
        return toUnit.ConvertFromBaseUnit(baseValue);
    }

    private double ConvertToBaseUnit()
    {
        return unit.ConvertToBaseUnit(value);
    }

    public QuantityWeight ConvertTo(WeightUnit targetUnit)
    {
        double baseValue = ConvertToBaseUnit();
        double converted = targetUnit.ConvertFromBaseUnit(baseValue);

        return new QuantityWeight(converted, targetUnit);
    }

    // public QuantityWeight Add(QuantityWeight other)
    // {
    //     if (other == null)
    //         throw new ArgumentException("Other quantity cannot be null");

    //     double sum = this.ConvertToBaseUnit() + other.ConvertToBaseUnit();

    //     double result = unit.ConvertFromBaseUnit(sum);

    //     return new QuantityWeight(result, unit);
    // }
public QuantityWeight Add(QuantityWeight other)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.ADD);

    double result = unit.ConvertFromBaseUnit(resultBase);

    return new QuantityWeight(result, unit);
}
    // public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
    // {
    //     if (other == null)
    //         throw new ArgumentException("Other quantity cannot be null");

    //     double sum = this.ConvertToBaseUnit() + other.ConvertToBaseUnit();

    //     double result = targetUnit.ConvertFromBaseUnit(sum);

    //     return new QuantityWeight(result, targetUnit);
    // }

public QuantityWeight Add(QuantityWeight other, WeightUnit targetUnit)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.ADD);

    double result = targetUnit.ConvertFromBaseUnit(resultBase);

    return new QuantityWeight(result, targetUnit);
}

    public override bool Equals(object obj)
    {
        if (obj == null) return false;

        if (GetType() != obj.GetType()) return false;

        QuantityWeight other = (QuantityWeight)obj;

        double base1 = this.ConvertToBaseUnit();
        double base2 = other.ConvertToBaseUnit();

        return Math.Abs(base1 - base2) < EPSILON;
    }

    // UC12 - Subtraction
    // public QuantityWeight Subtract(QuantityWeight other)
    // {
    //     if (other == null)
    //         throw new ArgumentException("Second operand cannot be null");

    //     double converted = other.ConvertTo(this.unit).value;

    //     return new QuantityWeight(this.value - converted, this.unit);
    // }
    public QuantityWeight Subtract(QuantityWeight other)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

    double result = unit.ConvertFromBaseUnit(resultBase);

    return new QuantityWeight(result, unit);
}

    // UC12 - Subtraction with target unit
    // public QuantityWeight Subtract(QuantityWeight other, WeightUnit targetUnit)
    // {
    //     if (other == null)
    //         throw new ArgumentException("Second operand cannot be null");

    //     double base1 = this.ConvertToBaseUnit();
    //     double base2 = other.ConvertToBaseUnit();

    //     double resultBase = base1 - base2;

    //     double result = targetUnit.ConvertFromBaseUnit(resultBase);

    //     return new QuantityWeight(result, targetUnit);
    // }

    public QuantityWeight Subtract(QuantityWeight other, WeightUnit targetUnit)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

    double result = targetUnit.ConvertFromBaseUnit(resultBase);

    return new QuantityWeight(result, targetUnit);
}

    // UC12 - Division
    // public double Divide(QuantityWeight other)
    // {
    //     if (other == null)
    //         throw new ArgumentException("Second operand cannot be null");

    //     double base1 = this.ConvertToBaseUnit();
    //     double base2 = other.ConvertToBaseUnit();

    //     if (Math.Abs(base2) < EPSILON)
    //         throw new ArithmeticException("Division by zero");

    //     return base1 / base2;
    // }
    public double Divide(QuantityWeight other)
{
    return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
}

    //UC13
    private void ValidateArithmeticOperands(QuantityWeight other)
{
    if (other == null)
        throw new ArgumentException("Operand cannot be null");

    if (double.IsNaN(this.value) || double.IsInfinity(this.value) ||
        double.IsNaN(other.value) || double.IsInfinity(other.value))
        throw new ArgumentException("Invalid numeric value");
}

private double PerformBaseArithmetic(QuantityWeight other, ArithmeticOperation operation)
{
    ValidateArithmeticOperands(other);

    double base1 = this.ConvertToBaseUnit();
    double base2 = other.ConvertToBaseUnit();

    switch (operation)
    {
        case ArithmeticOperation.ADD:
            return base1 + base2;

        case ArithmeticOperation.SUBTRACT:
            return base1 - base2;

        case ArithmeticOperation.DIVIDE:
            if (Math.Abs(base2) < EPSILON)
                throw new ArithmeticException("Division by zero");

            return base1 / base2;

        default:
            throw new InvalidOperationException("Unsupported operation");
    }
}
    public override int GetHashCode()
    {
        return ConvertToBaseUnit().GetHashCode();
    }

    public override string ToString()
    {
        return value + " " + unit;
    }
}
