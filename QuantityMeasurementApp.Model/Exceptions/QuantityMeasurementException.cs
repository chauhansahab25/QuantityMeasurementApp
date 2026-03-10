using System;

namespace QuantityMeasurementApp.Model.Exceptions
{
    public class QuantityMeasurementException : Exception
    {
        public QuantityMeasurementException(string message) : base(message)
        {
        }

        public QuantityMeasurementException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
