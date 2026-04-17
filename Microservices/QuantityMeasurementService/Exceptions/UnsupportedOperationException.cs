using System;

namespace QuantityMeasurementModelLayer.Exceptions;
    public class UnsupportedOperationException : Exception
    {
        public UnsupportedOperationException(string message) : base(message)
        {
        }
    }
