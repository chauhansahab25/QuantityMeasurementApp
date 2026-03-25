using System;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementModelLayer.DTO;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementRepositoryLayer.Services;
using QuantityMeasurementConsoleApp.Interfaces;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementConsoleApp.Services;

public class Menu : IMenu
{
    private readonly IConfiguration config;
    private IQuantityMeasurementService? service;
    private IQuantityMeasurementRepository? repository;
    private IQuantityMeasurementRepository? databaseRepository;
    private DataSyncService? syncService;
    private bool isUsingCache;

    public Menu(IConfiguration config)
    {
        this.config = config;
    }

    public void Start()
    {
        // Storage selection logic
        SelectStorage();
        
        // Main application loop
        while (true)
        {
            try
            {
                Console.WriteLine("\n===== Quantity Measurement System =====");
                Console.WriteLine("1 Length");
                Console.WriteLine("2 Volume");
                Console.WriteLine("3 Weight");
                Console.WriteLine("4 Temperature");
                Console.WriteLine("5 Show Records");
                Console.WriteLine("6 Exit");

                Console.Write("Enter choice: ");
                int type = Convert.ToInt32(Console.ReadLine());

                if (type == 6) return;

                if (type == 5)
                {
                    ShowAllData();
                    continue;
                }

                OperationMenu(type);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    private void SelectStorage()
    {
        Console.WriteLine("\n=== Checking Database Connectivity ===");
        
        // First check if database is available
        bool isDatabaseAvailable = CheckDatabaseConnectivity();
        
        if (!isDatabaseAvailable)
        {
            Console.WriteLine("❌ Database is not available. Using Cache Memory until database becomes active.");
            
            // repository = new QuantityMeasurementRepository();
            // TODO: Fix dependency injection for repository
            Console.WriteLine("Repository initialization needs to be updated for new structure.");
            return;
            isUsingCache = true;
            service = new QuantityMeasurementServiceImpl(repository);
            return;
        }
        
        Console.WriteLine("✅ Database is available!");
        
        // Check if there's pending JSON data to upload
        CheckAndUploadPendingJsonData();
        
        Console.WriteLine("\n=== Storage Selection ===");
        Console.WriteLine("Choose your preferred storage method:");
        Console.WriteLine();
        Console.WriteLine("1. Cache Memory (Fast, In-Memory Storage with JSON Persistence)");
        Console.WriteLine();
        Console.WriteLine("2. Database (Persistent SQL Server Storage)");
        Console.WriteLine();
        
        while (true)
        {
            Console.Write("Enter your choice (1 for Cache, 2 for Database): ");
            string input = Console.ReadLine();
            input = input?.Trim() ?? string.Empty;
            
            switch (input)
            {
                case "1":
                    Console.WriteLine("\n✓ Cache Memory selected - Using in-memory storage with JSON persistence");
                    // repository = new QuantityMeasurementRepository();
                    // TODO: Fix dependency injection for repository
                    Console.WriteLine("Repository initialization needs to be updated for new structure.");
                    return;
                    
                case "2":
                    Console.WriteLine("\n✓ Database selected - Using SQL Server persistent storage");
                    // repository = new QuantityMeasurementRepository();
                    // TODO: Fix dependency injection for repository
                    Console.WriteLine("Repository initialization needs to be updated for new structure.");
                    return;
                    
                default:
                    Console.WriteLine("Invalid choice. Please enter 1 or 2.");
                    continue;
            }

            service = new QuantityMeasurementServiceImpl(repository);
            
            // Create database repository for sync service if using cache
            if (isUsingCache)
            {
                // Reuse the same database repository instance if already created for database option
                if (databaseRepository == null)
                {
                    // databaseRepository = new QuantityMeasurementRepository();
                }
                syncService = new DataSyncService((ICacheRepository)repository, databaseRepository);
            }
            
            break;
        }
    }

    private void CheckAndUploadPendingJsonData()
    {
        try
        {
            // Create temporary cache repository to check for pending data
            // var tempCacheRepo = new QuantityMeasurementRepository();
            
            // if (tempCacheRepo.HasPendingData())
            // {
            //     Console.WriteLine("\n📝 Found pending data in JSON file from previous session.");                
            //     // Use existing database repository if available, create only if needed
            //     if (databaseRepository == null)
            //     {
            //         // databaseRepository = new QuantityMeasurementRepository();
            //     }
            //     var tempSyncService = new DataSyncService(tempCacheRepo, databaseRepository);
                
            //     bool success = tempSyncService.UploadPendingDataToDatabase(silent: false);
                
            //     if (success)
            //     {
            //         Console.WriteLine("✅ Pending data uploaded to database!\n");
            //     }
            //     else
            //     {
            //         Console.WriteLine("❌ Failed to upload pending data to database.\n");
            //     }
            // }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Error checking pending data: {ex.Message}\n");
        }
    }

    private bool CheckDatabaseConnectivity()
    {
        try
        {
            Console.WriteLine("Testing database connection...");
            // TODO: Implement proper database connectivity check with dependency injection
            Console.WriteLine("Database connectivity check needs to be updated for new structure.");
            return false; // Return false temporarily
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connectivity check failed: {ex.Message}");
            return false;
        }
    }

    // -------- OPERATION MENU --------
    private void OperationMenu(int type)
    {
        Console.WriteLine("\nSelect Operation");
        Console.WriteLine("1 Add");
        Console.WriteLine("2 Subtract");
        Console.WriteLine("3 Divide");
        Console.WriteLine("4 Compare");
        Console.WriteLine("5 Convert");

        Console.Write("Enter Operation: ");
        int operation = Convert.ToInt32(Console.ReadLine());

        QuantityDTO firstValue = GetQuantity(type);

        if (operation == 5)
        {
            ConvertUnit(firstValue, type);
            return;
        }

        QuantityDTO secondValue = GetQuantity(type);

        switch (operation)
        {
            case 1:
                if (service != null)
                {
                    PrintResult(service.Add(firstValue, secondValue));
                    AutoUploadToDatabase();
                }
                break;

            case 2:
                if (service != null)
                {
                    PrintResult(service.Subtract(firstValue, secondValue));
                    AutoUploadToDatabase();
                }
                break;

            case 3:
                if (service != null)
                {
                    double result = service.Divide(firstValue, secondValue);
                    Console.WriteLine("Result = " + result);
                    AutoUploadToDatabase();
                }
                break;

            case 4:
                if (service != null)
                {
                    Console.WriteLine("Are Equal = " + service.Compare(firstValue, secondValue));
                    AutoUploadToDatabase();
                }
                break;

            default:
                Console.WriteLine("Invalid Operation");
                break;
        }
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
    private void ConvertUnit(QuantityDTO firstValue, int type)
    {
        Console.WriteLine("\nSelect Target Unit");

        string targetUnit = SelectUnit(type);

        if (service != null)
        {
            QuantityDTO result = service.Convert(firstValue, targetUnit);
            PrintResult(result);
            AutoUploadToDatabase();
        }
    }

    // -------- OUTPUT --------
    private void PrintResult(QuantityDTO result)
    {
        Console.WriteLine($"\nResult = {result.Value} {result.Unit}");
    }

    // -------- SHOW DATABASE / CACHE RECORDS --------
    private void ShowAllData()
    {
        if (repository != null)
        {
            var list = repository.GetAll();

            Console.WriteLine("\n===== STORED RECORDS =====");

            foreach (var item in list)
            {
                Console.WriteLine(
                    $"Id: {item.Id} | " +
                    $"FirstValue: {item.FirstValue} {item.FirstUnit} | " +
                    $"SecondValue: {item.SecondValue} {item.SecondUnit} | " +
                    $"Operation: {item.Operation} | " +
                    $"Result: {item.Result} | " +
                    $"Type: {item.MeasurementType}"
                );
            }

            Console.WriteLine("==========================\n");
        }
    }

    private void AutoUploadToDatabase()
    {
        if (!isUsingCache || syncService == null) return;
        
        var cacheRepo = (ICacheRepository)repository!;
        
        if (!cacheRepo.HasPendingData()) return;

        try
        {
            // Check database connectivity before attempting upload
            if (!CheckDatabaseConnectivity())
            {
                Console.WriteLine("⚠️ Database not available");
                return;
            }
            
            bool success = syncService.UploadPendingDataToDatabase(silent: true);
            
            if (success)
            {
                Console.WriteLine("✅ Data automatically uploaded to database");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"⚠️ Auto-upload failed: {ex.Message}");
        }
    }
}