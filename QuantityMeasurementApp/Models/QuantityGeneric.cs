using System;

namespace QuantityMeasurementApp.Model
{
    public class Quantity<U> where U : Enum
    {
        private readonly double value;
        private readonly U unit;

        public double Value => value;
        public U Unit => unit;

        private enum ArithmeticOperation
        {
            ADD,
            SUBTRACT,
            DIVIDE
        }

        public Quantity(double value, U unit)
        {
            if (Double.IsNaN(value) || Double.IsInfinity(value))
                throw new ArgumentException("Value must be finite number.");
            this.value = value;
            this.unit = unit;
        }

        private double ConvertToBaseUnit()
        {
            return unit.ConvertToBaseUnit(value);
        }

        public static double Convert(double value, U source, U target)
        {
            if (source.Equals(target))
                return value;

            double baseValue = source.ConvertToBaseUnit(value);
            return target.ConvertFromBaseUnit(baseValue);
        }

        private void ValidateArithmeticOperands(Quantity<U> other)
        {
            if (other == null)
                throw new ArgumentException("Second operand cannot be null");
            if (this.unit.GetType() != other.unit.GetType())
                throw new ArgumentException("Cannot perform arithmetic on different measurement categories");
            if (Double.IsNaN(other.value) || Double.IsInfinity(other.value))
                throw new ArgumentException("Operand value must be finite number");
        }

        private double PerformBaseArithmetic(Quantity<U> other, ArithmeticOperation operation)
        {
            ValidateArithmeticOperands(other);

            double thisInBase = this.ConvertToBaseUnit();
            double otherInBase = other.ConvertToBaseUnit();

            return operation switch
            {
                ArithmeticOperation.ADD => thisInBase + otherInBase,
                ArithmeticOperation.SUBTRACT => thisInBase - otherInBase,
                ArithmeticOperation.DIVIDE => otherInBase == 0 ? throw new ArithmeticException("Cannot divide by zero") : thisInBase / otherInBase,
                _ => throw new ArgumentException("Invalid operation")
            };
        }

        public Quantity<U> Add(Quantity<U> other)
        {
            double resultInBase = PerformBaseArithmetic(other, ArithmeticOperation.ADD);
            double resultValue = this.unit.ConvertFromBaseUnit(resultInBase);
            return new Quantity<U>(resultValue, this.unit);
        }

        public static Quantity<U> Add(Quantity<U> q1, Quantity<U> q2)
        {
            if (q1 == null || q2 == null)
                throw new ArgumentException("Operands cannot be null");
            return q1.Add(q2);
        }

        public Quantity<U> Subtract(Quantity<U> other)
        {
            double resultInBase = PerformBaseArithmetic(other, ArithmeticOperation.SUBTRACT);
            double resultValue = this.unit.ConvertFromBaseUnit(resultInBase);
            return new Quantity<U>(resultValue, this.unit);
        }

        public double Divide(Quantity<U> other)
        {
            return PerformBaseArithmetic(other, ArithmeticOperation.DIVIDE);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (!(obj is Quantity<U>)) return false;
            Quantity<U> other = (Quantity<U>)obj;
            if (this.unit.GetType() != other.unit.GetType()) return false;
            double thisInBase = this.ConvertToBaseUnit();
            double otherInBase = other.ConvertToBaseUnit();
            return Math.Abs(thisInBase - otherInBase) < 0.000001;
        }

        public override int GetHashCode()
        {
            return ConvertToBaseUnit().GetHashCode();
        }

        public override string ToString()
        {
            return value + " " + unit;
        }
    }
}
