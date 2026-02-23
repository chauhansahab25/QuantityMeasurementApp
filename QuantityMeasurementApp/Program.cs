using System;
using QuantityMeasurementApp;

public class Program
{
    public static bool CompareFeet(double value1, double value2)
    {
        Feet f1 = new Feet(value1);
        Feet f2 = new Feet(value2);
        return f1.Equals(f2);
    }

    public static bool CompareInches(double value1, double value2)
    {
        Inches i1 = new Inches(value1);
        Inches i2 = new Inches(value2);
        return i1.Equals(i2);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Quantity Measurement Equality Check");
        Console.WriteLine("----------------------------------");

        Console.Write("Enter first feet value: ");
        double feet1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second feet value: ");
        double feet2 = Convert.ToDouble(Console.ReadLine());

        bool feetResult = CompareFeet(feet1, feet2);

        Console.WriteLine();
        Console.WriteLine("Input: " + feet1 + " ft and " + feet2 + " ft");
        Console.WriteLine("Output: Equal (" + feetResult + ")");

        Console.WriteLine();

        Console.Write("Enter first inches value: ");
        double inch1 = Convert.ToDouble(Console.ReadLine());

        Console.Write("Enter second inches value: ");
        double inch2 = Convert.ToDouble(Console.ReadLine());

        bool inchResult = CompareInches(inch1, inch2);

        Console.WriteLine();
        Console.WriteLine("Input: " + inch1 + " inch and " + inch2 + " inch");
        Console.WriteLine("Output: Equal (" + inchResult + ")");

        Console.ReadLine();
    }
}