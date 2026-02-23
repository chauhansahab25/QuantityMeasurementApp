using System;
using QuantityMeasurementApp;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Feet Measurement Equality Check");
        Console.WriteLine("--------------------------------");

        Console.Write("Enter first feet value: ");
        double value1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second feet value: ");
        double value2 = Convert.ToDouble(Console.ReadLine());

        Feet f1 = new Feet(value1);
        Feet f2 = new Feet(value2);

        bool result = f1.Equals(f2);

        Console.WriteLine();
        Console.WriteLine("Input: " + value1 + " ft and " + value2 + " ft");
        Console.WriteLine("Output: Equal (" + result + ")");

        Console.ReadLine();
    }
}