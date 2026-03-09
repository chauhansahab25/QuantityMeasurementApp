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
                Console.WriteLine("Select measurement type (LENGTH / WEIGHT / VOLUME):");
                string type = Console.ReadLine().ToUpper();

                Console.WriteLine("Select operation (EQUALITY / CONVERSION / ADDITION / SUBTRACTION / DIVISION):");
                string operation = Console.ReadLine().ToUpper();

                if (type == "LENGTH")
                {
                    ProcessOperation<Unit>(operation);
                }
                else if (type == "WEIGHT")
                {
                    ProcessOperation<WeightUnit>(operation);
                }
                else if (type == "VOLUME")
                {
                    ProcessOperation<VolumeUnit>(operation);
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

        static void ProcessOperation<U>(string operation) where U : Enum
        {
            if (operation == "EQUALITY")
            {
                ProcessEquality<U>();
            }
            else if (operation == "CONVERSION")
            {
                ProcessConversion<U>();
            }
            else if (operation == "ADDITION")
            {
                ProcessArithmetic<U>("add");
            }
            else if (operation == "SUBTRACTION")
            {
                ProcessArithmetic<U>("subtract");
            }
            else if (operation == "DIVISION")
            {
                ProcessArithmetic<U>("divide");
            }
            else
            {
                Console.WriteLine("Invalid operation.");
            }
        }

        static void ProcessEquality<U>() where U : Enum
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

            Console.WriteLine($"Result: {q1.Equals(q2)}");
        }

        static void ProcessConversion<U>() where U : Enum
        {
            Console.WriteLine("Enter value:");
            double value = Convert.ToDouble(Console.ReadLine());

            Console.WriteLine($"Enter source unit ({GetUnitOptions<U>()}):");
            U sourceUnit = (U)Enum.Parse(typeof(U), Console.ReadLine().ToUpper());

            Console.WriteLine($"Enter target unit ({GetUnitOptions<U>()}):");
            U targetUnit = (U)Enum.Parse(typeof(U), Console.ReadLine().ToUpper());

            double result = Quantity<U>.Convert(value, sourceUnit, targetUnit);

            Console.WriteLine($"Result: {result} {targetUnit}");
        }

        static void ProcessArithmetic<U>(string operation) where U : Enum
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

            if (operation == "add")
            {
                Quantity<U> result = q1.Add(q2);
                Console.WriteLine($"Result: {result.Value} {result.Unit}");
            }
            else if (operation == "subtract")
            {
                Quantity<U> result = q1.Subtract(q2);
                Console.WriteLine($"Result: {result.Value} {result.Unit}");
            }
            else if (operation == "divide")
            {
                double result = q1.Divide(q2);
                Console.WriteLine($"Result: {result} (dimensionless)");
            }
        }

        static string GetUnitOptions<U>() where U : Enum
        {
            return string.Join(" / ", Enum.GetNames(typeof(U)));
        }
    }
}