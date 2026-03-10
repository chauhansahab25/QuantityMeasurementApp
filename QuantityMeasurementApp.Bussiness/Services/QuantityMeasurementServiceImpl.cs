using QuantityMeasurementApp.Model;
using QuantityMeasurementApp.Model.DTOs;
using QuantityMeasurementApp.Model.Entities;
using QuantityMeasurementApp.Model.Exceptions;
using QuantityMeasurementApp.Repo.Repositories;
using System;

namespace QuantityMeasurementApp.Bussiness.Services
{
    public class QuantityMeasurementServiceImpl : IQuantityMeasurementService
    {
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementServiceImpl(IQuantityMeasurementRepository repository)
        {
            _repository = repository;
        }

        public QuantityDTO Compare(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                var result = quantity1.MeasurementType.ToUpper() switch
                {
                    "LENGTH" => CompareQuantities<Unit>(quantity1, quantity2),
                    "WEIGHT" => CompareQuantities<WeightUnit>(quantity1, quantity2),
                    "VOLUME" => CompareQuantities<VolumeUnit>(quantity1, quantity2),
                    "TEMPERATURE" => CompareQuantities<TemperatureUnit>(quantity1, quantity2),
                    _ => throw new QuantityMeasurementException("Invalid measurement type")
                };

                var entity = new QuantityMeasurementEntity("COMPARE", quantity1.Value, quantity1.Unit, 
                    quantity2.Value, quantity2.Unit, result);
                _repository.Save(entity);

                return new QuantityDTO { Value = result ? 1 : 0, Unit = "BOOLEAN", MeasurementType = "RESULT" };
            }
            catch (Exception ex)
            {
                var entity = new QuantityMeasurementEntity("COMPARE", ex.Message);
                _repository.Save(entity);
                throw new QuantityMeasurementException(ex.Message, ex);
            }
        }

        public QuantityDTO Convert(QuantityDTO quantity, string targetUnit)
        {
            try
            {
                var result = quantity.MeasurementType.ToUpper() switch
                {
                    "LENGTH" => ConvertQuantity<Unit>(quantity, targetUnit),
                    "WEIGHT" => ConvertQuantity<WeightUnit>(quantity, targetUnit),
                    "VOLUME" => ConvertQuantity<VolumeUnit>(quantity, targetUnit),
                    "TEMPERATURE" => ConvertQuantity<TemperatureUnit>(quantity, targetUnit),
                    _ => throw new QuantityMeasurementException("Invalid measurement type")
                };

                var entity = new QuantityMeasurementEntity("CONVERT", quantity.Value, quantity.Unit, 
                    targetUnit, result);
                _repository.Save(entity);

                return new QuantityDTO { Value = result, Unit = targetUnit, MeasurementType = quantity.MeasurementType };
            }
            catch (Exception ex)
            {
                var entity = new QuantityMeasurementEntity("CONVERT", ex.Message);
                _repository.Save(entity);
                throw new QuantityMeasurementException(ex.Message, ex);
            }
        }

        public QuantityDTO Add(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                var (resultValue, resultUnit) = quantity1.MeasurementType.ToUpper() switch
                {
                    "LENGTH" => PerformArithmetic<Unit>(quantity1, quantity2, "ADD"),
                    "WEIGHT" => PerformArithmetic<WeightUnit>(quantity1, quantity2, "ADD"),
                    "VOLUME" => PerformArithmetic<VolumeUnit>(quantity1, quantity2, "ADD"),
                    "TEMPERATURE" => PerformArithmetic<TemperatureUnit>(quantity1, quantity2, "ADD"),
                    _ => throw new QuantityMeasurementException("Invalid measurement type")
                };

                var entity = new QuantityMeasurementEntity("ADD", quantity1.Value, quantity1.Unit, 
                    quantity2.Value, quantity2.Unit, resultValue, resultUnit);
                _repository.Save(entity);

                return new QuantityDTO { Value = resultValue, Unit = resultUnit, MeasurementType = quantity1.MeasurementType };
            }
            catch (Exception ex)
            {
                var entity = new QuantityMeasurementEntity("ADD", ex.Message);
                _repository.Save(entity);
                throw new QuantityMeasurementException(ex.Message, ex);
            }
        }

        public QuantityDTO Subtract(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                var (resultValue, resultUnit) = quantity1.MeasurementType.ToUpper() switch
                {
                    "LENGTH" => PerformArithmetic<Unit>(quantity1, quantity2, "SUBTRACT"),
                    "WEIGHT" => PerformArithmetic<WeightUnit>(quantity1, quantity2, "SUBTRACT"),
                    "VOLUME" => PerformArithmetic<VolumeUnit>(quantity1, quantity2, "SUBTRACT"),
                    "TEMPERATURE" => PerformArithmetic<TemperatureUnit>(quantity1, quantity2, "SUBTRACT"),
                    _ => throw new QuantityMeasurementException("Invalid measurement type")
                };

                var entity = new QuantityMeasurementEntity("SUBTRACT", quantity1.Value, quantity1.Unit, 
                    quantity2.Value, quantity2.Unit, resultValue, resultUnit);
                _repository.Save(entity);

                return new QuantityDTO { Value = resultValue, Unit = resultUnit, MeasurementType = quantity1.MeasurementType };
            }
            catch (Exception ex)
            {
                var entity = new QuantityMeasurementEntity("SUBTRACT", ex.Message);
                _repository.Save(entity);
                throw new QuantityMeasurementException(ex.Message, ex);
            }
        }

        public QuantityDTO Divide(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                var result = quantity1.MeasurementType.ToUpper() switch
                {
                    "LENGTH" => DivideQuantities<Unit>(quantity1, quantity2),
                    "WEIGHT" => DivideQuantities<WeightUnit>(quantity1, quantity2),
                    "VOLUME" => DivideQuantities<VolumeUnit>(quantity1, quantity2),
                    "TEMPERATURE" => DivideQuantities<TemperatureUnit>(quantity1, quantity2),
                    _ => throw new QuantityMeasurementException("Invalid measurement type")
                };

                var entity = new QuantityMeasurementEntity("DIVIDE", quantity1.Value, quantity1.Unit, 
                    quantity2.Value, quantity2.Unit, result, "DIMENSIONLESS");
                _repository.Save(entity);

                return new QuantityDTO { Value = result, Unit = "DIMENSIONLESS", MeasurementType = "RESULT" };
            }
            catch (Exception ex)
            {
                var entity = new QuantityMeasurementEntity("DIVIDE", ex.Message);
                _repository.Save(entity);
                throw new QuantityMeasurementException(ex.Message, ex);
            }
        }

        private bool CompareQuantities<U>(QuantityDTO q1, QuantityDTO q2) where U : Enum
        {
            var unit1 = (U)Enum.Parse(typeof(U), q1.Unit.ToUpper());
            var unit2 = (U)Enum.Parse(typeof(U), q2.Unit.ToUpper());
            var quantity1 = new Quantity<U>(q1.Value, unit1);
            var quantity2 = new Quantity<U>(q2.Value, unit2);
            return quantity1.Equals(quantity2);
        }

        private double ConvertQuantity<U>(QuantityDTO q, string targetUnit) where U : Enum
        {
            var sourceUnit = (U)Enum.Parse(typeof(U), q.Unit.ToUpper());
            var target = (U)Enum.Parse(typeof(U), targetUnit.ToUpper());
            return Quantity<U>.Convert(q.Value, sourceUnit, target);
        }

        private (double, string) PerformArithmetic<U>(QuantityDTO q1, QuantityDTO q2, string operation) where U : Enum
        {
            var unit1 = (U)Enum.Parse(typeof(U), q1.Unit.ToUpper());
            var unit2 = (U)Enum.Parse(typeof(U), q2.Unit.ToUpper());
            var quantity1 = new Quantity<U>(q1.Value, unit1);
            var quantity2 = new Quantity<U>(q2.Value, unit2);

            var result = operation switch
            {
                "ADD" => quantity1.Add(quantity2),
                "SUBTRACT" => quantity1.Subtract(quantity2),
                _ => throw new QuantityMeasurementException("Invalid operation")
            };

            return (result.Value, result.Unit.ToString());
        }

        private double DivideQuantities<U>(QuantityDTO q1, QuantityDTO q2) where U : Enum
        {
            var unit1 = (U)Enum.Parse(typeof(U), q1.Unit.ToUpper());
            var unit2 = (U)Enum.Parse(typeof(U), q2.Unit.ToUpper());
            var quantity1 = new Quantity<U>(q1.Value, unit1);
            var quantity2 = new Quantity<U>(q2.Value, unit2);
            return quantity1.Divide(quantity2);
        }
    }
}
