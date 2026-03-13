namespace QuantityMeasurementModelLayer.Entities
{
    public class QuantityMeasurementEntity
    {
        public string Operation { get; set; }
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public string Result { get; set; }

        public QuantityMeasurementEntity(string operation,double op1,double op2,string result)
        {
            Operation = operation;
            Operand1 = op1;
            Operand2 = op2;
            Result = result;
        }
    }
}