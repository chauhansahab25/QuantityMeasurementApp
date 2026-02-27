using System;

namespace QuantityMeasurementApp.Model;

public class Quantity : Quantity<Unit>
{
    public Quantity(double value, Unit unit) : base(value, unit)
    {
    }

    public Quantity Add(Quantity other)
    {
        var result = base.Add(other);
        return new Quantity(result.Value, result.Unit);
    }

    public static Quantity Add(Quantity l1, Quantity l2)
    {
        var result = Quantity<Unit>.Add(l1, l2);
        return new Quantity(result.Value, result.Unit);
    }
}