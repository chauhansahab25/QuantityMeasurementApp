using System;
using QuantityMeasurementModelLayer.Enums;
namespace QuantityMeasurementApp.Model;
    public class QuantityLength
    {
        private readonly double value;
        private readonly LengthUnit unit;
        private const double EPSILON = 0.0001;

        // PUBLIC PROPERTIES (Fix)
        public double Value => value;
        public LengthUnit Unit => unit;

        public QuantityLength(double value, LengthUnit unit)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
                throw new ArgumentException("Value must be finite number.");

            this.value = value;
            this.unit = unit;
        }

        // Convert current object value to base unit (Feet)
        private double ConvertToBaseUnit()
        {
            return unit.ConvertToBaseUnit(value);
        }

        // UC5: Static Conversion API
        public static double Convert(double value, LengthUnit source, LengthUnit target)
        {
            if (source == target)
                return value;

            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        private static double AddInBaseUnit(QuantityLength l1, QuantityLength l2)
        {
            return l1.ConvertToBaseUnit() + l2.ConvertToBaseUnit();
        }

        // UC6
        // public QuantityLength Add(QuantityLength other)
        // {
        //     if (other == null)
        //         throw new ArgumentException("Second operand cannot be null");

        //     double sumInBase = AddInBaseUnit(this, other);
        //     double resultValue = this.unit.ConvertFromBaseUnit(sumInBase);

        //     return new QuantityLength(resultValue, this.unit);
        // }

public QuantityLength Add(QuantityLength other)
{
    double baseResult = PerformBaseArithmetic(other, ArithmeticOperation.ADD);

    double result = unit.ConvertFromBaseUnit(baseResult);

    return new QuantityLength(result, unit);
}
        public static QuantityLength AddTwoUnits(QuantityLength l1, QuantityLength l2)
        {
            if (l1 == null || l2 == null)
                throw new ArgumentException("Operands cannot be null");

            return l1.Add(l2);
        }


        // UC7
        // public static QuantityLength AddTwoUnits_TargetUnit(
        //     QuantityLength l1,
        //     QuantityLength l2,
        //     LengthUnit targetUnit)
        // {
        //     if (l1 == null || l2 == null)
        //         throw new ArgumentException("Operands cannot be null");

        //     double sumInBase = AddInBaseUnit(l1, l2);
        //     double resultValue = targetUnit.ConvertFromBaseUnit(sumInBase);

        //     return new QuantityLength(resultValue, targetUnit);
        // }


public static QuantityLength AddTwoUnits_TargetUnit(
    QuantityLength q1,
    QuantityLength q2,
    LengthUnit targetUnit)
{
    double baseResult = q1.PerformBaseArithmetic(q2, ArithmeticOperation.ADD);

    double result = targetUnit.ConvertFromBaseUnit(baseResult);

    return new QuantityLength(result, targetUnit);
}
        // UC8
        public QuantityLength ConvertTo(LengthUnit targetUnit)
        {
            double convertedValue = Convert(this.value, this.unit, targetUnit);
            return new QuantityLength(convertedValue, targetUnit);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is QuantityLength)) return false;

            QuantityLength other = (QuantityLength)obj;

            return Math.Abs(this.ConvertToBaseUnit() - other.ConvertToBaseUnit()) < EPSILON;
        }


// UC12 - Subtraction
// public QuantityLength Subtract(QuantityLength other)
// {
//     if (other == null)
//         throw new ArgumentException("Second operand cannot be null");

//     double resultInBase = this.ConvertToBaseUnit() - other.ConvertToBaseUnit();
//     double resultValue = this.unit.ConvertFromBaseUnit(resultInBase);

//     return new QuantityLength(resultValue, this.unit);
// }

public QuantityLength Subtract(QuantityLength other)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

    double result = unit.ConvertFromBaseUnit(resultBase);

    return new QuantityLength(result, unit);
}
// UC12 - Subtraction with Target Unit
// public QuantityLength Subtract(QuantityLength other, LengthUnit targetUnit)
// {
//     if (other == null)
//         throw new ArgumentException("Second operand cannot be null");

//     double resultInBase = this.ConvertToBaseUnit() - other.ConvertToBaseUnit();
//     double resultValue = targetUnit.ConvertFromBaseUnit(resultInBase);

//     return new QuantityLength(resultValue, targetUnit);
// }

public QuantityLength Subtract(QuantityLength other, LengthUnit targetUnit)
{
    double resultBase = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);

    double result = targetUnit.ConvertFromBaseUnit(resultBase);

    return new QuantityLength(result, targetUnit);
}

// UC12 - Division
// public double Divide(QuantityLength other)
// {
//     if (other == null)
//         throw new ArgumentException("Second operand cannot be null");

//     double divisor = other.ConvertToBaseUnit();

//     if (Math.Abs(divisor) < EPSILON)
//         throw new ArithmeticException("Division by zero");

//     return this.ConvertToBaseUnit() / divisor;
// }
public double Divide(QuantityLength other)
{
    return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
}

//UC13
private void ValidateArithmeticOperands(QuantityLength other)
{
    if (other == null)
        throw new ArgumentException("Operand cannot be null");

    if (double.IsNaN(this.value) || double.IsInfinity(this.value) ||
        double.IsNaN(other.value) || double.IsInfinity(other.value))
        throw new ArgumentException("Invalid numeric value");
}

private double PerformBaseArithmetic(QuantityLength other, ArithmeticOperation operation)
{
    if (other == null)
        throw new ArgumentException("Operand cannot be null");

    double base1 = this.ConvertToBaseUnit();
    double base2 = other.ConvertToBaseUnit();

    switch (operation)
    {
        case ArithmeticOperation.ADD:
            return base1 + base2;

        case ArithmeticOperation.SUBTRACT:
            return base1 - base2;

        case ArithmeticOperation.DIVIDE:
            if (Math.Abs(base2) < 0.00001)
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
