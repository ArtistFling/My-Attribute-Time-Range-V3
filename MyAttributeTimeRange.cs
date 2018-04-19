using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.Time;
using OSIsoft.AF.UnitsOfMeasure;
using OSIsoft.AF.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using OSIsoft.AF.Asset.DataReference;

namespace OSIsoft.AF.Asset.DataReference
{
    [Guid("EB7D4130-08FA-4C19-BD87-FB590CC552EC"),
    Serializable(),
    Description("My Attribute Time Range; Perform time range query method on the specified attribute")]
    public class MyAttributeTimeRange : AFDataReference
    {
        private string _configString;
        private string _targetAttributeName; // = string.Empty;                 // The attribute to retrieve the data from.
        private string _relativeTimeConfig; // = string.Empty;                  // Relative time as initially configured
        private string _relativeTimeResolved = string.Empty;

        //private string _timeRangeMethod;
        private AFSummaryTypes _timeRangeMethod = AFSummaryTypes.None;      // Method applied to time range.
        private RetrievalMethods _timeMethod = RetrievalMethods.Auto;
        private AFCalculationBasis _timeRangeBasis = AFCalculationBasis.TimeWeighted;
        private AFTimestampCalculation _timestampCalculation = AFTimestampCalculation.Auto;
        private string _UOM;
        private string _ByTime;
        private string _rateConversion = string.Empty;
        private string _timeRange;
        private string _calculationBasis;
        private float _timeRangeMinPercentGood = 0;

        private AFAttributeList paramAttributes = null;
        private DateTime dtLastLoadAttribute = DateTime.UtcNow;
        enum RetrievalMethods { Auto, AtOrBefore, AtOrAfter, Spare1, Exact, Spare2, Before, After, Interpolated, TimeRange, TimeRangeOverride, NotSupported };
        public MyAttributeTimeRange()
            : base()
        {
        }
        private void CheckConfig()
        {
            if (Attribute == null)
            {
                UnloadParameters();
                string msg = String.Format("The attribute has not been set for '{0}' data reference.", Name);
                throw new InvalidOperationException(msg);
            }

            if (ConfigString == null || ConfigString.Length <= 0)
            {
                UnloadParameters();
                string msg = String.Format("The data reference for attribute '{0}' has not been configured", Path);
                throw new ApplicationException(msg);
            }


        }


        #region Configuration Properties

        [Category("Configuration")]
        [Description("All attributes with the specified name are operated on by the rollup.")]
        [DefaultValue("")]
        public string TargetAttributeName
        {
            get { return _targetAttributeName; }
            set
            {
                if (_targetAttributeName != value)
                {
                    _targetAttributeName = value;
                    if (_targetAttributeName != null)
                        _targetAttributeName = _targetAttributeName.Trim();
                    SaveConfigChanges();
                }
            }
        }

        [Category("Configuration")]
        [Description("The Units of Measure to be used")]
        [DefaultValue("")]
        public string SourceUnits
        {
            get { return _UOM; }
            set
            {
                if (_UOM != value)
                {
                    _UOM = value;
                    if (_UOM != null)
                        _UOM = _UOM.Trim();
                    SaveConfigChanges();
                }
            }
        }

        [Category("Configuration")]
        [Description("The time retrieval methods to be used")]
        [DefaultValue("")]
        public string ByTime
        {
            get { return _ByTime; }
            set
            {
                if (_ByTime != value)
                {
                    _ByTime = value;
                    if (_ByTime != null)
                    {

                        _ByTime = _ByTime.Trim();
                        
                        //Enum.TryParse(_ByTime, true, out _timeMethod);
                    }
                    SaveConfigChanges();
                }
            }
        }

        [Category("Configuration")]
        [Description("The relative time to be used.")]
        [DefaultValue("")]
        public string RelativeTime
        {
            get { return _relativeTimeConfig; }
            set
            {
                if (_relativeTimeConfig != value)
                {
                    _relativeTimeConfig = value;
                    if (_relativeTimeConfig != null)
                    {
                        _relativeTimeConfig = _relativeTimeConfig.Trim();
                        _relativeTimeResolved = GetQueryString(_relativeTimeConfig, null, null);
                    }
                      
                    SaveConfigChanges();
                }
            }
        }
        [Category("Configuration")]
        [Description("The time range to be used")]
        [DefaultValue("")]
        public string TimeRange
        {
            get { return _timeRange; }
            set
            {
                if (_timeRange != value)
                {
                    _timeRange = value;
                    if (_timeRange != null)
                    {
                        _timeRange = _timeRange.Trim();
                        //_timeRangeMethod = Enum.TryParse(_timeRange, true, out _timeRangeMethod);
                        //Enum.TryParse(_timeRange, true, out _timeRangeMethod);
                        
                    }
                        
                    SaveConfigChanges();
                }
            }
        }
        [Category("Configuration")]
        [Description("The calculation basis to be used.")]
        [DefaultValue("")]
        public string Calculation
        {
            get { return _calculationBasis; }
            set
            {
                if (_calculationBasis != value)
                {
                    _calculationBasis = value;
                    if (_calculationBasis != null)
                    {


                        _calculationBasis = _calculationBasis.Trim();
                       // Enum.TryParse(_calculationBasis, true, out _timestampCalculation);
                        //_timestampCalculation
                    }

                    SaveConfigChanges();
                }
            }
        }
        [Category("Configuration")]
        [Description("The min percent good to be used")]
        [DefaultValue(false)]
        public float MinPercentGood
        {
            get { return _timeRangeMinPercentGood; }
            set
            {
                if (_timeRangeMinPercentGood != value)
                {
                    _timeRangeMinPercentGood = value;

                    SaveConfigChanges();
                }
            }
        }










        #endregion
        #region Implementation of AFDataReference
        public override AFDataReferenceContext SupportedContexts
        {
            get
            {
                return AFDataReferenceContext.All;
            }
        }

        public override AFDataReferenceMethod SupportedMethods
        {
            get
            {
                return AFDataReferenceMethod.GetValue | AFDataReferenceMethod.GetValues;
            }
        }

        // This is for AF 2.5 and higher only, and is needed for PI DataLink 2013 compatibility
        // AF-SDK versions lower than 2.5 won't throw an exception, as this virtual method doesn't exist
        public override Data.AFDataMethods SupportedDataMethods
        {
            get
            {
                return (AFDataMethods.Summary | AFDataMethods.Summaries);
            }
        }


        public override string ConfigString
        {

            get
            {
                //// key1=val1;key2=val2;
                //StringBuilder sb = new StringBuilder();
                //sb.AppendFormat("{0}={1};", "TargetAttribute", _targetAttributeName);
                //sb.AppendFormat("{0}={1};", "RelativeTime", _relativeTimeConfig);
                //sb.AppendFormat("{0}={1};", "TimeRangeMethod", _timeRangeMethod);
                //sb.AppendFormat("{0}={1};", "TimeMethod", _timeMethod.ToString());
                ////return sb.ToString();
                //return _configString;
                StringBuilder configBuilder = new StringBuilder();
                // only build statement if configuration of something exist
                if (!string.IsNullOrWhiteSpace(TargetAttributeName))
                {
                    configBuilder.AppendFormat("targetattribute={0}", TargetAttributeName);

                    if (!string.IsNullOrWhiteSpace(ByTime))
                    {
                        configBuilder.AppendFormat(";timerangemethod={0}", ByTime);

                    }
                    if (!string.IsNullOrWhiteSpace(RelativeTime))
                    {
                        configBuilder.AppendFormat(";relativetime={0}", RelativeTime);

                    }
                    if (!string.IsNullOrWhiteSpace(TimeRange))
                    {
                        configBuilder.AppendFormat(";timemethod={0}", TimeRange);

                    }
                    if (!string.IsNullOrWhiteSpace(Calculation))
                    {
                        configBuilder.AppendFormat(";timestampcalculation={0}", Calculation);

                    }


                    if (MinPercentGood != 0)
                    {
                        configBuilder.AppendFormat(";timerangeminpercentgood={0}", MinPercentGood);

                    }

                }
                if (!string.IsNullOrWhiteSpace(SourceUnits))
                {
                    configBuilder.AppendFormat(";uom={0}", SourceUnits);

                }


                return configBuilder.ToString();
            }
            set
            {
                if (ConfigString != value)
                {
                    //defaults
                    _configString = value;
                    _targetAttributeName = ""; //string.Empty;
                    _relativeTimeConfig = "";//string.Empty;
                    _relativeTimeResolved = ""; // string.Empty;
                    _timeRangeMethod = AFSummaryTypes.None;
                    _timeMethod = RetrievalMethods.Auto;
                    _timeRangeBasis  = AFCalculationBasis.TimeWeighted;
                    _timestampCalculation =  AFTimestampCalculation.Auto;
                    _rateConversion = string.Empty;
                    _timeRangeMinPercentGood = 0;

                    var tokens = value.Split(';');
                    foreach (var token in tokens)
                    {
                        //TODO: Better error handling here
                        var keyValue = token.Split('=');
                        switch (keyValue[0].ToLower())
                        {
                            case "targetattribute":
                                _targetAttributeName = keyValue[1];
                                break;
                            case "relativetime":
                                //_relativeTime = keyValue[1];
                                _relativeTimeConfig = keyValue[1];
                                _relativeTimeResolved = GetQueryString(keyValue[1], null, null);
                                break;
                            case "timerangemethod":
                                //AFSummaryTypes summaryType;
                                //Enum.TryParse(keyValue[1], true, out summaryType);
                                //if (Enum.IsDefined(typeof(AFSummaryTypes), summaryType) | summaryType.ToString().Contains(","))
                                //    _timeRangeMethod = summaryType;
                                //else
                                //    _timeRangeMethod = AFSummaryTypes.None;
                                if (!Enum.TryParse(keyValue[1], true, out _timeRangeMethod))
                                    _timeRangeMethod = AFSummaryTypes.None;
                                break;
                            case "timemethod":
                                RetrievalMethods timeMethod;
                                Enum.TryParse(keyValue[1], true, out timeMethod);
                                if (Enum.IsDefined(typeof(RetrievalMethods), timeMethod))
                                    _timeMethod = timeMethod;
                                else
                                    _timeMethod = RetrievalMethods.Auto;
                                //if (!Enum.TryParse(keyValue[1], true, out _timeMethod))
                                //    _timeMethod = RetrievalMethods.Auto;
                                break;
                            case "timerangebasis":
                                AFCalculationBasis timeRangeBasis;
                                Enum.TryParse(keyValue[1], true, out timeRangeBasis);
                                if (Enum.IsDefined(typeof(AFCalculationBasis), timeRangeBasis))
                                    _timeRangeBasis = timeRangeBasis;
                                else
                                    _timeRangeBasis = AFCalculationBasis.TimeWeighted;
                                break;
                            case "timestampcalculation":
                                AFTimestampCalculation timestampCalculation;
                                Enum.TryParse(keyValue[1], true, out timestampCalculation);
                                if (Enum.IsDefined(typeof(AFTimestampCalculation), timestampCalculation))
                                    _timestampCalculation = timestampCalculation;
                                else
                                    _timestampCalculation = AFTimestampCalculation.Auto;
                                break;
                            case "rateconversion":
                                _rateConversion = keyValue[1];
                                break;
                            case "timerangeminpercentgood":
                                float minPercentGood;
                                if (float.TryParse(keyValue[1], out minPercentGood))
                                    _timeRangeMinPercentGood = minPercentGood;
                                else
                                    _timeRangeMinPercentGood = 0;
                                break;
                            case "uom":
                                break;
                            default:
                                if (keyValue.Length > 2)
                                    throw new Exception(String.Format("Invalid ConfigString element: {0}", keyValue[0]));
                                break;
                        }
                    }

                    // Signal the AFSDK that the configuration has changed.
                    SaveConfigChanges();
                }
            }
            #region Old Code
            // The ConfigString property is used to store and load the configuration of this data reference.
            // Unless the string format is trivial, you may want to decompose the string
            // into properties of this data reference during the 'set', and build the string from
            // these properties during the 'get', as is done here.
            // Otherwise, you must store the configstring as a field.
            //get
            //{
            //    StringBuilder configBuilder = new StringBuilder();
            //    // only build statement if configuration of something exist
            //    if (!string.IsNullOrWhiteSpace(TargetAttributeName))
            //    {
            //        configBuilder.AppendFormat("TargetAttribute={0}", TargetAttributeName);

            //        if (!string.IsNullOrWhiteSpace(ByTime))
            //        {
            //            configBuilder.AppendFormat(";TimeMethod={0}", ByTime);

            //        }
            //        if (!string.IsNullOrWhiteSpace(RelativeTime))
            //        {
            //            configBuilder.AppendFormat(";RelativeTime={0}", RelativeTime);

            //        }
            //        if (!string.IsNullOrWhiteSpace(TimeRange))
            //        {
            //            configBuilder.AppendFormat(";TimeRangeMethod={0}", TimeRange);

            //        }
            //        if (!string.IsNullOrWhiteSpace(Calculation))
            //        {
            //            configBuilder.AppendFormat(";TimeRangeBasis={0}", Calculation);

            //        }


            //        if (MinPercentGood != 0)
            //        {
            //            configBuilder.AppendFormat(";TimeRangeMinPercentGood={0}", MinPercentGood);

            //        }

            //    }
            //    if (!string.IsNullOrWhiteSpace(SourceUnits))
            //    {
            //        configBuilder.AppendFormat(";uom={0}", SourceUnits);

            //    }


            //    return configBuilder.ToString();

            //}
            //set
            //{
            //    if (ConfigString != value)
            //    {
            //        // reset to defaults
            //        _targetAttributeName = "";
            //        _UOM = "";
            //        _ByTime = "";
            //        _relativeTimeConfig = "";
            //        _timeRange = "";
            //        _calculationBasis = "";
            //        _timeRangeMinPercentGood = 80;



            //        if (value != null)
            //        {
            //            string[] tokens = value.Split(';', '=');
            //            for (int i = 0; i < tokens.Length; i++)
            //            {
            //                string paramName = tokens[i];
            //                string paramValue = "";

            //                if (++i < tokens.Length)
            //                    paramValue = tokens[i];

            //                switch (paramName.ToUpperInvariant())
            //                {
            //                    case "TARGETATTRIBUTE":
            //                        _targetAttributeName = paramValue;
            //                        break;

            //                    case "UOM":
            //                        _UOM = paramValue;
            //                        break;

            //                    case "TIMEMETHOD":
            //                        _ByTime = paramValue;
            //                        break;
            //                    case "RELATIVETIME":
            //                        _relativeTimeConfig = paramValue;
            //                        break;
            //                    case "TIMERANGEMETHOD":
            //                        _timeRange = paramValue;
            //                        break;
            //                    case "TIMERANGEBASIS":
            //                        _calculationBasis = paramValue;
            //                        break;
            //                    case "TIMERANGEMINPERCENTGOOD":
            //                        _timeRangeMinPercentGood = float.Parse(paramValue);
            //                        break;

            //                    default:
            //                        throw new ArgumentException(String.Format("Unrecognized configuration setting '{0}' in '{1}'.", paramName, value));
            //                }
            //            }

            //        }

            //        // notify clients of change
            //        SaveConfigChanges();
            //        UnloadParameters();
            //    }
            //}
            #endregion Old Code


        }

        private void UnloadParameters()
        {
            paramAttributes = null;

        }
        public override Type EditorType
        {
            // The EditorType property returns the type of the editor to be used to edit this data reference.  
            // The editor must inherit from Windows.Forms and must have a public constructor that accepts 
            // this data reference and a boolean read-only flag.
            get { return typeof(TimeRangeEntry); }
        }

        // ===================================================================================
        // Override the Various Inputs required to calculate a value for this attribute.
        // ===================================================================================
        public override AFAttributeList GetInputs(object context)
        {
       
            AFAttributeList inputs = new AFAttributeList();

            //// TODO add better error handling here.
            inputs.Add(this.GetAttribute(_targetAttributeName));
            LoadParameters();
            //// Anytime someone asks for values from this data reference we tell the SDK that we also need these attributes.
            return inputs;
        }
        public override AFValue GetValue(object context, object timeContext, AFAttributeList inputAttributes, AFValues inputValues)
        {


            // Declare Variables
            AFTimeRange timeRange = new AFTimeRange();
            AFTimeSpan span = new AFTimeSpan();
            AFTime timeVal = new AFTime();
            AFValue result = new AFValue();
            //AFAttribute targetAttr = null;
           
                 AFAttribute targetAttr = inputAttributes[0];
            
            
            bool useRecordedValue = false;

            if (string.IsNullOrEmpty(ConfigString))
                throw new Exception("ConfigString cannot be null or empty");

            // ---------------------------------------------------------------------------------------------
            // Parse the Time Context 
            // ---------------------------------------------------------------------------------------------
            // Mimic AF Poi Point behaviour
            // if TimeMethod is TimeRange, then apply the RelativeTime setting if the client supplys a single time (not a time range)
            // If TimeMethod is TimeRangeOverride, then apply the relativeTime always
            // If TimeMethod is NotSupported, then return an error message if the client sends a time instead of a timerange
            //_timeMethod
            switch (_timeMethod)
            {
                case RetrievalMethods.TimeRange:
                    if (timeContext is AFTime)
                    {
                        timeRange.EndTime = (AFTime)timeContext;
                        timeRange = ParseTimeRange(timeRange, _relativeTimeResolved);
                    }
                    else if (timeContext == null)
                    {
                        timeRange.EndTime = DateTime.Now;
                        timeRange = ParseTimeRange(timeRange, _relativeTimeResolved);
                    }
                    else
                        timeRange = (AFTimeRange)timeContext;
                    break;
                case RetrievalMethods.TimeRangeOverride:
                    if (timeContext is AFTime)
                        timeRange.EndTime = (AFTime)timeContext;
                    else if (timeContext == null)
                        timeRange.EndTime = DateTime.Now;
                    else
                        // We use the end time of the time range and override the span.
                        timeRange.EndTime = ((AFTimeRange)timeContext).EndTime;

                    timeRange = ParseTimeRange(timeRange, _relativeTimeResolved);
                    break;
                case RetrievalMethods.NotSupported:
                    if (timeContext is AFTime || timeContext == null)
                        throw new Exception("Not Supported");
                    else
                        timeRange = (AFTimeRange)timeContext;
                    break;
                default:
                    // This is not a time range query. Just use recordedvalue
                    useRecordedValue = true;
                    if (timeContext is AFTime)
                        timeRange.EndTime = (AFTime)timeContext;
                    else if (timeContext == null)
                        timeRange.EndTime = DateTime.Now;
                    else
                        timeRange.EndTime = ((AFTimeRange)timeContext).EndTime;
                    break;

            }
            //------------------------------------------------------------------------------------------
            //Query the archive
            //------------------------------------------------------------------------------------------
            if (useRecordedValue)
            {
                result = targetAttr.Data.RecordedValue(timeRange.EndTime, (AFRetrievalMode)_timeMethod, targetAttr.DefaultUOM);
            }
            else
            {
                //IDictionary<AFSummaryTypes, AFValue> value = targetAttr.Data.Summary(timeRange, AFSummaryTypes.All, _timeRangeBasis, _timestampCalculation);
                IDictionary<AFSummaryTypes, AFValue> value = targetAttr.Data.Summary(timeRange, _timeRangeMethod | AFSummaryTypes.PercentGood, _timeRangeBasis, _timestampCalculation);

                if (value[_timeRangeMethod].IsGood)
                {
                    //Check the quality meets the min percent good rule
                    if (value[AFSummaryTypes.PercentGood].ValueAsSingle() >= _timeRangeMinPercentGood)
                    {
                        result = value[_timeRangeMethod];
                        //rate conversion for totals
                        if ((_timeRangeMethod == AFSummaryTypes.Total || _timeRangeMethod == AFSummaryTypes.TotalWithUOM) && _rateConversion != string.Empty)
                        {
                            result.Value = targetAttr.PISystem.UOMDatabase.UOMs[_rateConversion].Convert(value[_timeRangeMethod].ValueAsDouble(), targetAttr.PISystem.UOMDatabase.UOMs["day"]);
                        }
                    }
                    else
                        throw new Exception(String.Format("Value does not have the required percent good ({0}) over the time range in {1}|{2}. Parameter name: minPercentGood", _timeRangeMinPercentGood, this.Attribute.Element.Name, this.Attribute.Name));
                }
                else throw new Exception((string)value[_timeRangeMethod].Value.ToString());
            }

            return result;
        }
        private AFTimeRange ParseTimeRange(AFTimeRange timeRange, String relativeTime)
        {
            AFTime timeVal;

            if (AFTime.TryParse(relativeTime, timeRange.EndTime, out timeVal))
                timeRange.StartTime = timeVal;
            else
                throw new Exception("Invalid time string for RelativeTime");

            return timeRange;
        }
        public override AFValues GetValues(object context, AFTimeRange timeContext, int numberOfValues, AFAttributeList inputAttributes, AFValues[] inputValues)
        {
            //if (!bChecked)
            //CheckConfig();
            // Evaluate
            try
            {
                // base implementation is sufficient for all calculation data references.
                return base.GetValues(context, timeContext, numberOfValues, inputAttributes, inputValues);
            }
            catch
            {
                // For any exception, unload parameters and set flag so parameters
                //  are rechecked on the next call.
                UnloadParameters();
                throw;
            }
        }

        // ===================================================================================
        // Override of the Summary function. Provides a dictionary of summary results
        // ===================================================================================
        public override IDictionary<AFSummaryTypes, AFValue> Summary(AFTimeRange timeRange, AFSummaryTypes summaryType, AFCalculationBasis calcBasis, AFTimestampCalculation timeType)
        {
            AFAttribute targetAttr = this.GetAttribute(_targetAttributeName);
            return targetAttr.Data.Summary(timeRange, summaryType, calcBasis, timeType);
        }
        public override IDictionary<AFSummaryTypes, AFValues> Summaries(AFTimeRange timeRange, AFTimeSpan summaryDuration, AFSummaryTypes summaryType, AFCalculationBasis calcBasis, AFTimestampCalculation timeType)
        {
            AFAttribute targetAttr = this.GetAttribute(_targetAttributeName);
            return targetAttr.Data.Summaries(timeRange, summaryDuration, summaryType, calcBasis, timeType);
        }
        #endregion
        #region Internal methods for performing Calcuation



        private void LoadParameters()
        {
            if (Attribute == null) return; // must be a template
            if (Attribute.Element == null) return; // must be a dynamic attribute
            if (paramAttributes == null)
            {
                paramAttributes = new AFAttributeList();

                // Look for attributes matching the target name in all child elements.
                AFElement element = Attribute.Element as AFElement;
                
                dtLastLoadAttribute = DateTime.UtcNow;
            }
        }


        #endregion Internal methods for performing Calcuation

        private string GetQueryString(string token, object context, object timeContext)
        {
            string substitutedConfig = SubstituteParameters(token, this, context, timeContext).Trim();
            if (substitutedConfig.StartsWith("%@") && substitutedConfig.EndsWith("%"))
            {
                string attributeName = substitutedConfig.Substring(2, substitutedConfig.Length - 3);
                var referencedAttribute = base.GetAttribute(attributeName);
                if (!ReferenceEquals(referencedAttribute, null))
                {
                    var value = referencedAttribute.GetValue(context, timeContext, null);
                    if (value != null && value.Value != null)
                    {
                        return value.Value.ToString();
                    }
                }
            }


            return substitutedConfig;
        }

    }
}

