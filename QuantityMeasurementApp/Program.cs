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
                    ProcessMeasurement<Unit>();
                }
                else if (type == "WEIGHT")
                {
                    ProcessMeasurement<WeightUnit>();
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

        static void ProcessMeasurement<U>() where U : Enum
        {
            Console.WriteLine("Enter first value:");
            double value1 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine($"Enter first unit ({GetUnitOptions<U>()}):");
            U unit1 = (U)Enum.Parse(typeof(U), Console.ReadLine().ToUpper());

            Console.WriteLine("Enter second value:");
            double value2 = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine($"Enter second unit ({GetUnitOptions<U>()}):");
            U unit2 = (U)Enum.Parse(typeof(U), Console.ReadLine().ToUpper());

            Quantity<U> q1 = new Quantity<U>(value1, unit1);
            Quantity<U> q2 = new Quantity<U>(value2, unit2);
            Quantity<U> result = q1.Add(q2);

            Console.WriteLine($"Result: {result.Value} {result.Unit}");
        }

        static string GetUnitOptions<U>() where U : Enum
        {
            return string.Join(" / ", Enum.GetNames(typeof(U)));
        }
    }
}