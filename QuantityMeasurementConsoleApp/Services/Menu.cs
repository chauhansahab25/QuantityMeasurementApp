using System;
using Microsoft.Extensions.DependencyInjection;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementConsoleApp.Interface;

public class Menu : IMenu
{
    private readonly IQuantityMeasurementService service;

    public Menu()
    {
        this.service = ConfigureServices();
    }

    public Menu(IQuantityMeasurementService service)
    {
        this.service = service;
    }

    private IQuantityMeasurementService ConfigureServices()
    {
        var provider = new ServiceCollection()
            .AddSingleton<IQuantityMeasurementRepository, QuantityMeasurementCacheRepository>()
            .AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>()
            .BuildServiceProvider();

        var service = provider.GetService<IQuantityMeasurementService>();
        if (service == null)
        {
            throw new InvalidOperationException("Failed to configure IQuantityMeasurementService");
        }
        
        return service;
    }

    public void Start()
    {
        while (true)
        {
            try
            {
                ClearScreen();
                DisplayMainMenu();
                int type = GetUserChoice();

                if (type == 5)
                    return;

                ProcessMeasurementType(type);
            }
            catch (Exception ex)
            {
                DisplayError(ex.Message);
                WaitForUserInput();
            }
        }
    }

    // IMenu Interface Implementation
    public void DisplayMainMenu()
    {
        Console.WriteLine("\n===== Quantity Measurement System =====");
        DisplayMeasurementTypeMenu();
    }

    public void DisplayMeasurementTypeMenu()
    {
        Console.WriteLine("1 Length");
        Console.WriteLine("2 Volume");
        Console.WriteLine("3 Weight");
        Console.WriteLine("4 Temperature");
        Console.WriteLine("5 Exit");
    }

    public void DisplayOperationMenu()
    {
        Console.WriteLine("\nSelect Operation");
        Console.WriteLine("1 Add");
        Console.WriteLine("2 Subtract");
        Console.WriteLine("3 Divide");
        Console.WriteLine("4 Compare");
        Console.WriteLine("5 Convert");
    }

    public int GetUserChoice()
    {
        Console.Write("Enter choice: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    public void DisplayResult(string result)
    {
        Console.WriteLine($"\nResult = {result}");
    }

    public void DisplayError(string error)
    {
        Console.WriteLine($"Error: {error}");
    }

    public void WaitForUserInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    public void ClearScreen()
    {
        Console.Clear();
    }

    // -------- BUSINESS LOGIC --------

    private void ProcessMeasurementType(int type)
    {
        DisplayOperationMenu();
        int operation = GetOperationChoice();

        QuantityDTO q1 = GetQuantity(type);

        if (operation == 5)
        {
            ConvertUnit(q1, type);
            return;
        }

        QuantityDTO q2 = GetQuantity(type);

        switch (operation)
        {
            case 1:
                var addResult = service.Add(q1, q2);
                DisplayResult($"{addResult.Value} {addResult.Unit}");
                break;

            case 2:
                var subtractResult = service.Subtract(q1, q2);
                DisplayResult($"{subtractResult.Value} {subtractResult.Unit}");
                break;

            case 3:
                double divideResult = service.Divide(q1, q2);
                DisplayResult(divideResult.ToString());
                break;

            case 4:
                bool compareResult = service.Compare(q1, q2);
                DisplayResult($"Are Equal = {compareResult}");
                break;

            default:
                DisplayError("Invalid Operation");
                break;
        }
        
        WaitForUserInput();
    }

    private int GetOperationChoice()
    {
        Console.Write("Enter Operation: ");
        return Convert.ToInt32(Console.ReadLine());
    }

    // -------- INPUT --------

    private QuantityDTO GetQuantity(int type)
    {
        Console.Write("\nEnter Value: ");
        double value = Convert.ToDouble(Console.ReadLine());

        string unit = SelectUnit(type);

        return new QuantityDTO(value, unit);
    }

    // -------- UNIT MENU --------

    private string SelectUnit(int type)
    {
        Console.WriteLine("Select Unit");

        if (type == 1) // Length
        {
            Console.WriteLine("1 FEET");
            Console.WriteLine("2 INCHES");
            Console.WriteLine("3 YARDS");
            Console.WriteLine("4 CENTIMETERS");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => "FEET",
                2 => "INCHES",
                3 => "YARDS",
                4 => "CENTIMETERS",
                _ => "FEET"
            };
        }
        else if (type == 2) // Volume
        {
            Console.WriteLine("1 LITRE");
            Console.WriteLine("2 MILLILITRE");
            Console.WriteLine("3 GALLON");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => "LITRE",
                2 => "MILLILITRE",
                3 => "GALLON",
                _ => "LITRE"
            };
        }
        else if (type == 3) // Weight
        {
            Console.WriteLine("1 KILOGRAM");
            Console.WriteLine("2 GRAM");
            Console.WriteLine("3 POUND");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => "KILOGRAM",
                2 => "GRAM",
                3 => "POUND",
                _ => "KILOGRAM"
            };
        }
        else // Temperature
        {
            Console.WriteLine("1 CELSIUS");
            Console.WriteLine("2 FAHRENHEIT");

            int choice = Convert.ToInt32(Console.ReadLine());

            return choice switch
            {
                1 => "CELSIUS",
                2 => "FAHRENHEIT",
                _ => "CELSIUS"
            };
        }
    }

    // -------- CONVERT --------

    private void ConvertUnit(QuantityDTO q1, int type)
    {
        Console.WriteLine("\nSelect Target Unit");
        string targetUnit = SelectUnit(type);
        QuantityDTO result = service.Convert(q1, targetUnit);
        DisplayResult($"{result.Value} {result.Unit}");
    }
}