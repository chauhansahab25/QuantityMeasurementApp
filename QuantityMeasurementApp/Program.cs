using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter value:");
                double value = Convert.ToDouble(Console.ReadLine());

                Console.WriteLine("Enter unit (FEET / INCH):");
                string unitInput = Console.ReadLine()?.ToUpper();

                Unit unit = unitInput switch
                {
                    "FEET" => Unit.FEET,
                    "INCH" => Unit.INCH,
                    _ => throw new ArgumentException("Invalid unit entered")
                };

                Quantity quantity = new Quantity(value, unit);

                Console.WriteLine($"Quantity created: {quantity.Value} {quantity.Unit}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
