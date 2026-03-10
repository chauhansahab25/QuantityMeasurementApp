using System;

namespace QuantityMeasurementApp.Model.Entities
{
    [Serializable]
    public class QuantityMeasurementEntity
    {
        public string OperationType { get; set; }
        public double? Operand1Value { get; set; }
        public string Operand1Unit { get; set; }
        public double? Operand2Value { get; set; }
        public string Operand2Unit { get; set; }
        public double? ResultValue { get; set; }
        public string ResultUnit { get; set; }
        public bool? BooleanResult { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public DateTime Timestamp { get; set; }

        public QuantityMeasurementEntity()
        {
            Timestamp = DateTime.Now;
        }

        public QuantityMeasurementEntity(string operationType, double operand1Value, string operand1Unit, 
            double operand2Value, string operand2Unit, double resultValue, string resultUnit) : this()
        {
            OperationType = operationType;
            Operand1Value = operand1Value;
            Operand1Unit = operand1Unit;
            Operand2Value = operand2Value;
            Operand2Unit = operand2Unit;
            ResultValue = resultValue;
            ResultUnit = resultUnit;
        }

        public QuantityMeasurementEntity(string operationType, double operand1Value, string operand1Unit, 
            double operand2Value, string operand2Unit, bool booleanResult) : this()
        {
            OperationType = operationType;
            Operand1Value = operand1Value;
            Operand1Unit = operand1Unit;
            Operand2Value = operand2Value;
            Operand2Unit = operand2Unit;
            BooleanResult = booleanResult;
        }

        public QuantityMeasurementEntity(string operationType, double operand1Value, string operand1Unit, 
            string targetUnit, double resultValue) : this()
        {
            OperationType = operationType;
            Operand1Value = operand1Value;
            Operand1Unit = operand1Unit;
            ResultValue = resultValue;
            ResultUnit = targetUnit;
        }

        public QuantityMeasurementEntity(string operationType, string errorMessage) : this()
        {
            OperationType = operationType;
            HasError = true;
            ErrorMessage = errorMessage;
        }
    }
}
