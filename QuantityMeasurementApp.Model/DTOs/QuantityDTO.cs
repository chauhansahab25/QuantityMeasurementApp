namespace QuantityMeasurementApp.Model.DTOs
{
    public class QuantityDTO
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public string MeasurementType { get; set; }

        public interface IMeasurableUnit
        {
            string GetMeasurementType();
            string GetUnitName();
        }

        public enum LengthUnit
        {
            FEET, INCH, YARD, CENTIMETER
        }

        public enum WeightUnit
        {
            KILOGRAM, GRAM, POUND
        }

        public enum VolumeUnit
        {
            LITRE, MILLILITRE, GALLON
        }

        public enum TemperatureUnit
        {
            CELSIUS, FAHRENHEIT
        }
    }
}
