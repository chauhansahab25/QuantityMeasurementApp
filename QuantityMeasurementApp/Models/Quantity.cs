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

    private double ConvertToInch()
    {
        if (unit == Unit.FEET)
            return value * 12;
        if (unit == Unit.INCH)
            return value;
        if (unit == Unit.YARD)
            return value * 36;
        if (unit == Unit.CENTIMETER)
            return value * 0.393701;
        throw new ArgumentException("Invalid unit");
    }
    // Static method to convert between units
    public static double Convert(double value, Unit source, Unit target)
    {
        if (source == target)
            return value;

        Quantity temp = new Quantity(value, source);
        double valueInInch = temp.ConvertToInch();

        if (target == Unit.FEET)
            return valueInInch / 12;
        if (target == Unit.INCH)
            return valueInInch;
        if (target == Unit.YARD)
            return valueInInch / 36;
        if (target == Unit.CENTIMETER)
            return valueInInch / 0.393701;
        throw new ArgumentException("Invalid target unit");
    }

    
    public Quantity Add(Quantity other)
    {
        if (other == null)
            throw new ArgumentException("Second operand cannot be null");

        double thisInInch = this.ConvertToInch();
        double otherInInch = other.ConvertToInch();

        double sumInInch = thisInInch + otherInInch;

        double resultValue;

        if (this.unit == Unit.FEET)
            resultValue = sumInInch / 12;
        else if (this.unit == Unit.INCH)
            resultValue = sumInInch;
        else if (this.unit == Unit.YARD)
            resultValue = sumInInch / 36;
        else if (this.unit == Unit.CENTIMETER)
            resultValue = sumInInch / 0.393701;
        else
            throw new ArgumentException("Invalid unit");

        return new Quantity(resultValue, this.unit);
    }
    // Static method to add two QuantityLength objects
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
        double thisInInch = this.ConvertToInch();
        double otherInInch = other.ConvertToInch();
        return Math.Abs(thisInInch - otherInInch) < 0.000001;
    }

    public override int GetHashCode()
    {
        return ConvertToInch().GetHashCode();
    }
    public override string ToString()
    {
        return value + " " + unit;
    }
}