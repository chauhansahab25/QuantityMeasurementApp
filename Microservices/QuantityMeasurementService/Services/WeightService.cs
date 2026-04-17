using System;
using QuantityMeasurementModelLayer.Enums;
using QuantityMeasurementModelLayer.Models;
using QuantityMeasurementModelLayer.Extensions;

namespace QuantityMeasurementService.Services;

public class WeightService
{
    private const double EPSILON = 0.00001;

    public static double Convert(double value, WeightUnit fromUnit, WeightUnit toUnit)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            throw new ArgumentException("Invalid value");

        // Convert to base unit first, then to target unit
        double baseValue = fromUnit.ConvertToBaseUnit(value);
        return toUnit.ConvertFromBaseUnit(baseValue);
    }

    public static QuantityWeight Add(QuantityWeight first, QuantityWeight second)
    {
        ValidateOperands(first, second);
        
        double resultBase = PerformBaseArithmetic(first, second, ArithmeticOperation.ADD);
        double result = first.Unit.ConvertFromBaseUnit(resultBase);
        
        return new QuantityWeight(result, first.Unit);
    }

    public static QuantityWeight Add(QuantityWeight first, QuantityWeight second, WeightUnit targetUnit)
    {
        ValidateOperands(first, second);
        
        double resultBase = PerformBaseArithmetic(first, second, ArithmeticOperation.ADD);
        double result = targetUnit.ConvertFromBaseUnit(resultBase);
        
        return new QuantityWeight(result, targetUnit);
    }

    public static QuantityWeight Subtract(QuantityWeight first, QuantityWeight second)
    {
        ValidateOperands(first, second);
        
        double resultBase = PerformBaseArithmetic(first, second, ArithmeticOperation.SUBTRACT);
        double result = first.Unit.ConvertFromBaseUnit(resultBase);
        
        return new QuantityWeight(result, first.Unit);
    }

    public static QuantityWeight Subtract(QuantityWeight first, QuantityWeight second, WeightUnit targetUnit)
    {
        ValidateOperands(first, second);
        
        double resultBase = PerformBaseArithmetic(first, second, ArithmeticOperation.SUBTRACT);
        double result = targetUnit.ConvertFromBaseUnit(resultBase);
        
        return new QuantityWeight(result, targetUnit);
    }

    public static double Divide(QuantityWeight first, QuantityWeight second)
    {
        ValidateOperands(first, second);
        return PerformBaseArithmetic(first, second, ArithmeticOperation.DIVIDE);
    }

    public static bool AreEqual(QuantityWeight first, QuantityWeight second)
    {
        if (first == null || second == null) return false;

        double base1 = first.Unit.ConvertToBaseUnit(first.Value);
        double base2 = second.Unit.ConvertToBaseUnit(second.Value);

        return Math.Abs(base1 - base2) < EPSILON;
    }

    private static void ValidateOperands(QuantityWeight first, QuantityWeight second)
    {
        if (first == null || second == null)
            throw new ArgumentException("Operands cannot be null");

        if (double.IsNaN(first.Value) || double.IsInfinity(first.Value) ||
            double.IsNaN(second.Value) || double.IsInfinity(second.Value))
            throw new ArgumentException("Invalid numeric value");
    }

    private static double PerformBaseArithmetic(QuantityWeight first, QuantityWeight second, ArithmeticOperation operation)
    {
        double base1 = first.Unit.ConvertToBaseUnit(first.Value);
        double base2 = second.Unit.ConvertToBaseUnit(second.Value);

        switch (operation)
        {
            case ArithmeticOperation.ADD:
                return base1 + base2;

            case ArithmeticOperation.SUBTRACT:
                return base1 - base2;

            case ArithmeticOperation.DIVIDE:
                if (Math.Abs(base2) < EPSILON)
                    throw new DivideByZeroException("Division by zero");
                return base1 / base2;

            default:
                throw new InvalidOperationException("Unsupported operation");
        }
    }
}