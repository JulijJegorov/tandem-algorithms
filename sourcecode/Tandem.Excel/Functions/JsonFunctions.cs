using Tandem.Elements;
using Tandem.Elements.Utilities;
using Tandem.Excel.Utilities;

namespace Tandem.Excel.Functions
{
    public class JsonFunctions
    {
        private readonly CellUtilities _cellUtilities;
        private readonly ElementUtilities _elementUtilities;

        public JsonFunctions(CellUtilities cellUtilities,
                                ElementUtilities elementUtilities
                                )
        {
            _cellUtilities = cellUtilities;
            _elementUtilities = elementUtilities;
        }

        public JsonFunctions()
            : this(new CellUtilities(),
                    new ElementUtilities()) { }

        public string ElementToJson(object[,] value, object[,] jsonPath)
        {
            var tdmElement = _elementUtilities.RangeToGrid(value);
            string path = null;

            if (!_cellUtilities.IsEmpty(jsonPath[0, 0]))
            {
                path = jsonPath[0, 0].ToString();
            }
            return Serialize.ElementToJson(tdmElement, path);
        }

        public ITdmElement JsonToElement(object[,] jsonString)
        {
            ITdmElement tdmElement = null;

            if (_cellUtilities.IsJson(jsonString[0, 0]))
            {
                tdmElement = Serialize.JsonToElement(jsonString[0, 0].ToString());
            }
            else
            {
                tdmElement = Serialize.JsonToElement(jsonString[0, 0].ToString(), true);
            }
            return tdmElement;
        }
    }
}
