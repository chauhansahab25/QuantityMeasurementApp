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
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            Console.ReadLine();
        }
    }
}