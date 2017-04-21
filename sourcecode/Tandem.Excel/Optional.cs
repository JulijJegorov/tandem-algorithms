using System;
using ExcelDna.Integration;

namespace Tandem.Excel
{
    class Optional
    {
        internal static string Check(object arg, string defaultValue)
        {
            if (arg is string)
                return (string)arg;
            else if (arg is ExcelMissing)
                return defaultValue;
            else
                return arg.ToString();  // Or whatever you want to do here....

            // Perhaps check for other types and do whatever you think is right ....
            //else if (arg is double)
            //    return "Double: " + (double)arg;
            //else if (arg is bool)
            //    return "Boolean: " + (bool)arg;
            //else if (arg is ExcelError)
            //    return "ExcelError: " + arg.ToString();
            //else if (arg is object[,])
            //    // The object array returned here may contain a mixture of types,
            //    // reflecting the different cell contents.
            //    return string.Format("Array[{0},{1}]", 
            //      ((object[,])arg).GetLength(0), ((object[,])arg).GetLength(1));
            //else if (arg is ExcelEmpty)
            //    return "<<Empty>>"; // Would have been null
            //else if (arg is ExcelReference)
            //  // Calling xlfRefText here requires IsMacroType=true for this function.
			//				return "Reference: " + 
            //                     XlCall.Excel(XlCall.xlfReftext, arg, true);
			//			else
			//				return "!? Unheard Of ?!";
        }        

        internal static double Check(object arg, double defaultValue)
        {
            if (arg is double)
                return (double)arg;
            else if (arg is ExcelMissing)
                return defaultValue;
            else
                throw new ArgumentException();
        }

        internal static bool Check(object arg, bool defaultValue)
        {
            if (arg is bool)
                return (bool)arg;
            else if (arg is ExcelMissing)
                return defaultValue;
            else
                throw new ArgumentException();
        }

        internal static object[,] Check(object arg, object[,] defaultValue)
        {
            if (arg is object[,])
                return (object[,])arg;
            else if (arg is ExcelMissing)
                return defaultValue;
            else
                throw new ArgumentException();
        }

        // This one is more tricky - we have to do the double->Date conversions ourselves
        internal static DateTime Check(object arg, DateTime defaultValue)
        {
            if (arg is double)
                return DateTime.FromOADate((double)arg);    // Here is the conversion
            else if (arg is string)
                return DateTime.Parse((string)arg);
            else if (arg is ExcelMissing)
                return defaultValue;
                
            else 
                throw new ArgumentException();  // Or defaultValue or whatever
        }
    }
}
