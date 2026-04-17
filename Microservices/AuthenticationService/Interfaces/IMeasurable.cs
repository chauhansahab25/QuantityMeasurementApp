namespace QuantityMeasurementApp.Model;
    public interface IMeasurable
    {
        double GetConversionFactor();
        double ConvertToBaseUnit(double value);
        double ConvertFromBaseUnit(double baseValue);
        string GetUnitName();

        // Functional Interface equivalent
        public delegate bool SupportsArithmetic();

        // Default lambda expression (all units support arithmetic by default)
        static SupportsArithmetic supportsArithmetic = () => true;

        // Method to check arithmetic support
        public virtual bool SupportsArithmeticOperation()
        {
            return supportsArithmetic();
        }

        // Default validation method
        public virtual void ValidateOperationSupport(string operation)
        {
            // default does nothing
            // units like Temperature can override this
        }
    }
