using System;
using System.Collections.Generic;
using System.Linq;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.Time;

namespace OSIsoft.AF.Asset.DataReference
{
    class RollupCalculation
    {
        /// <summary>
        /// Calculates the arithmetic average of the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the calculated average</returns>
        public AFValue Average(AFValues inputValues, UOM defaultUOM)
        {
            double _calcValue = 0;
            int _count = 0;
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        _calcValue += dCurrVal;
                        _count++;
                    }
                }
                _calcValue = _calcValue / _count;

                return new AFValue(_calcValue, time);
                //return new AFValue(_calcValue, (AFTime)DateTime.Now);
            }
            catch (SystemException sysEx)
            {
                return new AFValue(sysEx.Message, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Calculates the sum of the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the calculated sum</returns>
        public AFValue Total(AFValues inputValues, UOM defaultUOM)
        {
            double _calcValue = 0;
            int _count = 0;
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        _calcValue += dCurrVal;
                        _count++;
                    }
                }
                return new AFValue(_calcValue, time);
                //return new AFValue(_calcValue, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Calculates the median of the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the calculated median</returns>
        public AFValue Median(AFValues inputValues, UOM defaultUOM)
        {
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                double _median = 0;
                List<double> dValues = new List<double>();
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        dValues.Add(dCurrVal);
                    }
                }

                // Convert the list to an array and sort it
                double[] _sortedValues = dValues.ToArray();
                Array.Sort(_sortedValues);

                // now find the median value
                int _count = _sortedValues.Length;
                if (_count == 0)
                {
                    return new AFValue(Double.NaN, (AFTime)DateTime.Now);
                }
                else if (_count % 2 == 0)
                {
                    // count is even, so we average the two middle elements
                    double a = _sortedValues[_count / 2 - 1];
                    double b = _sortedValues[_count / 2];
                    _median = (a + b) / 2;
                }
                else
                {
                    _median = _sortedValues[_count / 2];
                }

                return new AFValue(_median, time);
                //return new AFValue(_median, (AFTime)DateTime.Now);;
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Determines the minimum value from the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the Minimum value</returns>
        public AFValue Minimum(AFValues inputValues, UOM defaultUOM)
        {
            double _minValue = 0;
            bool _calcSet = false;
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        if (!_calcSet)
                        {
                            _minValue = dCurrVal;
                            _calcSet = true;
                        }
                        _minValue = Math.Min(_minValue, dCurrVal);
                    }
                }
                return new AFValue(_minValue, time);
                //return new AFValue(_minValue, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Determines the maximum value from the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the Maximum value</returns>
        public AFValue Maximum(AFValues inputValues, UOM defaultUOM)
        {
            double _maxValue = 0;
            bool _calcSet = false;
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        if (!_calcSet)
                        {
                            _maxValue = dCurrVal;
                            _calcSet = true;
                        }
                        _maxValue = Math.Max(_maxValue, dCurrVal);
                    }
                }
                return new AFValue(_maxValue, time);
                //return new AFValue(_maxValue, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Calculates the standard deviation of the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the calculated standard deviation</returns>
        public AFValue StdDeviation(AFValues inputValues, UOM defaultUOM)
        {
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                List<double> dValues = new List<double>();
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        dValues.Add(dCurrVal);
                    }
                }

                double _avgValue = dValues.Average();
                double sumOfSquareOfDifferences = dValues.Select(val => (val - _avgValue) * (val - _avgValue)).Sum();
                double _stdDev = Math.Sqrt(sumOfSquareOfDifferences / dValues.Count);

                return new AFValue(_stdDev, time);
                //return new AFValue(_stdDev, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// Calculates the range of the passed in values
        /// </summary>
        /// <param name="inputValues">Collection of AFValues that represent the child element attribute values to be rolled up</param>
        /// <param name="defaultUOM">The Unit of Measure of the owning AFAttribute</param>
        /// <returns>Double representing the calculated range</returns>
        public AFValue Range(AFValues inputValues, UOM defaultUOM)
        {
            double _maxValue = 0;
            double _minValue = 0;
            bool _calcSet = false;
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                foreach (AFValue inputVal in inputValues)
                {
                    if (inputVal.IsGood && inputVal != null)
                    {
                        double dCurrVal;

                        // make sure we do all operations in same UOM, if applicable
                        if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                            dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                        else
                            dCurrVal = Convert.ToDouble(inputVal.Value);

                        if (!_calcSet)
                        {
                            _maxValue = dCurrVal;
                            _minValue = dCurrVal;
                            _calcSet = true;
                        }
                        _maxValue = Math.Max(_maxValue, dCurrVal);
                        _minValue = Math.Min(_minValue, dCurrVal);
                    }
                }
                double _range = _maxValue - _minValue;
                return new AFValue(_range, time);
                //return new AFValue(_range, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owningElement"></param>
        /// <param name="restrictByTemplate"></param>
        /// <param name="requiredTemplate"></param>
        /// <returns></returns>
        public AFValue Count(AFElement owningElement, bool restrictByTemplate, AFElementTemplate requiredTemplate, bool restrictByCategory, AFCategory requiredCategory)
        {
            try
            {
                long _count = 0;
                if ((restrictByTemplate) && (requiredTemplate != null))
                {
                    foreach (AFElement child in owningElement.Elements)
                    {
                        if (child.Template == requiredTemplate)
                        {
                            _count++;
                        }
                    }
                    return new AFValue(_count);
                    //return new AFValue(_count, (AFTime)DateTime.Now);
                }
                else if ((restrictByCategory) && (requiredCategory != null))
                {
                    foreach (AFElement child in owningElement.Elements)
                    {
                        if (child.Categories.Contains(requiredCategory))
                            _count++;
                    }
                    return new AFValue(_count);
                }
                else
                {
                    return new AFValue(owningElement.Elements.Count, (AFTime)DateTime.Now);
                }
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputValues"></param>
        /// <param name="defaultUOM"></param>
        /// <param name="comparisonValue"></param>
        /// <param name="comparisonOperator"></param>
        /// <returns></returns>
        public AFValue Count(AFValues inputValues, UOM defaultUOM, string comparisonValue, string comparisonOperator)
        {
            AFTime time = new AFTime();
            time = inputValues[0].Timestamp;
            try
            {
                long _count = 0;
                double dComparisonValue;
                if (Double.TryParse(comparisonValue, out dComparisonValue))
                {
                    foreach (AFValue inputVal in inputValues)
                    {
                        if (inputVal.IsGood && inputVal != null)
                        {
                            double dCurrVal;

                            // make sure we do all operations in same UOM, if applicable
                            if (inputVal.UOM != null && defaultUOM != null && inputVal.UOM != defaultUOM)
                                dCurrVal = defaultUOM.Convert(inputVal.Value, inputVal.UOM);
                            else
                                dCurrVal = Convert.ToDouble(inputVal.Value);

                            switch (comparisonOperator)
                            {
                                case "LT":
                                    if (dCurrVal < dComparisonValue)
                                        _count++;
                                    break;
                                case "LE":
                                    if (dCurrVal <= dComparisonValue)
                                        _count++;
                                    break;
                                case "EQ":
                                    if (dCurrVal == dComparisonValue)
                                        _count++;
                                    break;
                                case "GE":
                                    if (dCurrVal >= dComparisonValue)
                                        _count++;
                                    break;
                                case "GT":
                                    if (dCurrVal > dComparisonValue)
                                        _count++;
                                    break;
                                case "NE":
                                    if (dCurrVal != dComparisonValue)
                                        _count++;
                                    break;
                                case "Is Good":
                                    if (inputVal.IsGood)
                                        _count++;
                                    break;
                                case "Is Bad":
                                    if (!inputVal.IsGood)
                                        _count++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    // comparison value is not numeric, only valid options should be equality or inequality, and value quality
                    foreach (AFValue inputVal in inputValues)
                    {
                        if (inputVal.IsGood && inputVal != null)
                        {
                            switch (comparisonOperator)
                            {
                                case "EQ":
                                    if (String.Compare(inputVal.ToString(), comparisonValue, false) == 0)
                                        _count++;
                                    break;
                                case "NE":
                                    if (String.Compare(inputVal.ToString(), comparisonValue, false) != 0)
                                        _count++;
                                    break;
                                case "Is Good":
                                    if (inputVal.IsGood)
                                        _count++;
                                    break;
                                case "Is Bad":
                                    if (!inputVal.IsGood)
                                        _count++;
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }
                return new AFValue(_count, time);
                //return new AFValue(_count, (AFTime)DateTime.Now);
            }
            catch
            {
                return new AFValue(Double.NaN, (AFTime)DateTime.Now, null, AFValueStatus.Bad);
            }
        }
    }
}
