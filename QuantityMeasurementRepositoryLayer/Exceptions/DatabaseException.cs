using System;

namespace QuantityMeasurementRepositoryLayer.Exceptions
{
    public class DatabaseException : Exception
    {
        public DatabaseException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}