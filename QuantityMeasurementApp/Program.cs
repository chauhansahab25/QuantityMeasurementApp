﻿using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter First Quantity");

                Console.Write("Value: ");
                double value1 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Unit (Feet, Inches, Yards, Centimeters): ");
                Unit unit1 = ParseUnit(Console.ReadLine());

                var quantity1 = new Quantity(value1, unit1);

                Console.WriteLine("\nEnter Second Quantity");

                Console.Write("Value: ");
                double value2 = Convert.ToDouble(Console.ReadLine());

                Console.Write("Unit (Feet, Inches, Yards, Centimeters): ");
                Unit unit2 = ParseUnit(Console.ReadLine());

                var quantity2 = new Quantity(value2, unit2);

                bool result = quantity1.Equals(quantity2);

                Console.WriteLine($"\nAre they equal? {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static Unit ParseUnit(string input)
        {
            if (Enum.TryParse(typeof(Unit), input, true, out object unit))
                return (Unit)unit;

            throw new ArgumentException("Invalid unit entered.");
        }
    }
}