using System;

namespace QuantityMeasurementApp.Model;

public class Quantity
{
    private readonly double value;
    private readonly Unit unit;

    public double Value => value;
    public Unit Unit => unit;
    public Quantity(double value, Unit unit)
    {
        if (Double.IsNaN(value) || Double.IsInfinity(value))
            throw new ArgumentException("Value must be finite number.");
        this.value = value;
        this.unit = unit;
    }

    private double ConvertToBaseUnit()
    {
        return unit.ConvertToBaseUnit(value);
    }
    public static double Convert(double value, Unit source, Unit target)
    {
        if (source == target)
            return value;

        double baseValue = source.ConvertToBaseUnit(value);
        return target.ConvertFromBaseUnit(baseValue);
    }

    public Quantity Add(Quantity other)
    {
        if (other == null)
            throw new ArgumentException("Second operand cannot be null");

        double thisInBase = this.ConvertToBaseUnit();
        double otherInBase = other.ConvertToBaseUnit();
        double sumInBase = thisInBase + otherInBase;
        double resultValue = this.unit.ConvertFromBaseUnit(sumInBase);

        return new Quantity(resultValue, this.unit);
    }
    public static Quantity Add(Quantity l1, Quantity l2)
    {
        if (l1 == null || l2 == null)
            throw new ArgumentException("Operands cannot be null");

        return l1.Add(l2);
    }


    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        if (this == obj) return true;
        if (!(obj is Quantity)) return false;
        Quantity other = (Quantity)obj;
        double thisInBase = this.ConvertToBaseUnit();
        double otherInBase = other.ConvertToBaseUnit();
        return Math.Abs(thisInBase - otherInBase) < 0.000001;
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