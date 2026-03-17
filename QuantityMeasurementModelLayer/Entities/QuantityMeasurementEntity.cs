namespace QuantityMeasurementModelLayer.Entities;
    public class QuantityMeasurementEntity
    {
        public int Id { get; set; }

        public double FirstValue { get; set; }
        public string FirstUnit { get; set; } = string.Empty;

        public double SecondValue { get; set; }
        public string SecondUnit { get; set; } = string.Empty;

        public string Operation { get; set; } = string.Empty;

        public double Result { get; set; }

        public string MeasurementType { get; set; } = string.Empty;

        public QuantityMeasurementEntity() { }

        public QuantityMeasurementEntity(
            double firstValue,
            string firstUnit,
            double secondValue,
            string secondUnit,
            string operation,
            double result,
            string measurementType)
        {
            FirstValue = firstValue;
            FirstUnit = firstUnit ?? string.Empty;
            SecondValue = secondValue;
            SecondUnit = secondUnit ?? string.Empty;
            Operation = operation ?? string.Empty;
            Result = result;
            MeasurementType = measurementType ?? string.Empty;
        }
    }
