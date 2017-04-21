using ExcelDna.Integration;

namespace Tandem.Excel.Utilities
{
    public class CellUtilities
    {
        //public string Placeholder { get { return "{}"; } }
        public object Placeholder { get { return ExcelError.ExcelErrorNA; } }

        public string Address()
        {
            ExcelReference excelReference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);

            string sheetName = (string)XlCall.Excel(XlCall.xlSheetNm, excelReference);
            string cellAddress = (string)XlCall.Excel(XlCall.xlfAddress, 1 + excelReference.RowFirst, 1 + excelReference.ColumnFirst);
            string fullAddress = sheetName + "!" + cellAddress;

            return fullAddress;
        }

        public string CellValue()
        {
            ExcelReference excelReference = (ExcelReference)XlCall.Excel(XlCall.xlfCaller);
            return XlCall.xlfValue.ToString();
        }

        public bool IsEmpty(object cell)
        {
            return (cell == null) ? true : (
                    //cell.ToString() == this.Placeholder ||
                    cell.GetType() == typeof(ExcelDna.Integration.ExcelEmpty) ||
                    cell.GetType() == typeof(ExcelDna.Integration.ExcelMissing) ||
                    cell.GetType() == typeof(ExcelDna.Integration.ExcelError)
                    );
        }

        public bool IsJson(object cell)
        {
            string input = cell.ToString();
            input = input.Trim();
            return input.StartsWith("{") && input.EndsWith("}")
                                    || input.StartsWith("[") && input.EndsWith("]");
        }

        public string GetName(object cell)
        {
            string name;
            if (IsEmpty(cell))
                name = this.Address();
            else
                name = cell.ToString();
            return name;
        }

        public string SplitName(object cell)
        {
            string name = cell.ToString();

            int separator = name.IndexOf(":");

            if (separator == -1)
                return name;
            else
                return name.Substring(0, separator);
        }
    }
}
