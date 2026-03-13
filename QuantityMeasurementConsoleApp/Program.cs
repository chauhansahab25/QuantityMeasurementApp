using Microsoft.Extensions.DependencyInjection;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementBusinessLayer.Services;
using QuantityMeasurementRepositoryLayer.Interfaces;
using QuantityMeasurementRepositoryLayer.Repositories;
using QuantityMeasurementConsoleApp.Interface;

class Program
{
    static void Main()
    {
        IMenu menu = new Menu();
        menu.Start();
    }
}