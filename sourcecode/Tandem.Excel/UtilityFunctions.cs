using Tandem.Elements;
using Tandem.Excel.Utilities;
using Tandem.Elements.Utilities;

namespace Tandem.Excel.Functions
{
    public class UtilityFunctions
    {
        private readonly CellUtilities _cellUtilities;
        private readonly ElementUtilities _elementUtilities;
        private readonly RangeUtilties _rangeUtilities;
        private readonly TdmContainerPool _tdmContainerPool;

        public UtilityFunctions(CellUtilities cellUtilities,
                                ElementUtilities elementUtilities,
                                RangeUtilties rangeUtilities,
                                TdmContainerPool tdmContainerPool
                                )
        {
            _cellUtilities = cellUtilities;
            _elementUtilities = elementUtilities;
            _rangeUtilities = rangeUtilities;
            _tdmContainerPool = tdmContainerPool;
        }

        public UtilityFunctions()
                    : this(new CellUtilities(),
                        new ElementUtilities(),
                        new RangeUtilties(),
                        TdmContainerPool.Instance) { }


        public string tdmRename(string name, string alias)
        {
            string splitName = _cellUtilities.SplitName(name);
            string aliasToSrt = alias.ToString();

            var tdmContainer = _tdmContainerPool.Get(splitName);

            if (tdmContainer != null && alias != null)
            {
                if (tdmContainer.Name != aliasToSrt)
                {
                    _tdmContainerPool.Remove(splitName);
                    tdmContainer.Alias = aliasToSrt;
                }
                _tdmContainerPool.Add(tdmContainer);
            }
            return tdmContainer.FullName;
        }
    }
}
