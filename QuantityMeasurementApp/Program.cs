﻿﻿using System;
using QuantityMeasurementApp.Model;

namespace QuantityMeasurementApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Select measurement type (LENGTH / WEIGHT):");
                string type = Console.ReadLine().ToUpper();

                if (type == "LENGTH")
                {
                    Console.WriteLine("Enter first value:");
                    double value1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Enter first unit (FEET / INCH / YARD / CENTIMETER):");
                    Unit unit1 = (Unit)Enum.Parse(typeof(Unit), Console.ReadLine().ToUpper());

                    Console.WriteLine("Enter second value:");
                    double value2 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Enter second unit (FEET / INCH / YARD / CENTIMETER):");
                    Unit unit2 = (Unit)Enum.Parse(typeof(Unit), Console.ReadLine().ToUpper());

                    Quantity q1 = new Quantity(value1, unit1);
                    Quantity q2 = new Quantity(value2, unit2);
                    Quantity result = q1.Add(q2);

                    Console.WriteLine($"Result: {result.Value} {result.Unit}");
                }
                else if (type == "WEIGHT")
                {
                    Console.WriteLine("Enter first value:");
                    double value1 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Enter first unit (KILOGRAM / GRAM / POUND):");
                    WeightUnit unit1 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine().ToUpper());

                    Console.WriteLine("Enter second value:");
                    double value2 = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Enter second unit (KILOGRAM / GRAM / POUND):");
                    WeightUnit unit2 = (WeightUnit)Enum.Parse(typeof(WeightUnit), Console.ReadLine().ToUpper());

                    QuantityWeight w1 = new QuantityWeight(value1, unit1);
                    QuantityWeight w2 = new QuantityWeight(value2, unit2);
                    QuantityWeight result = w1.Add(w2);

                    Console.WriteLine($"Result: {result.Value} {result.Unit}");
                }
                else
                {
                    Console.WriteLine("Invalid measurement type.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}