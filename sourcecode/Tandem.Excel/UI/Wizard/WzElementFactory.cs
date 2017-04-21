using System;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    public static class WzElementFactory
    {
        public static IWzElement GetElementClass(Type tdmElementType)
        {
            if (tdmElementType == typeof(TdmElementGroup))
                return new WzElementGroup(); 
            else
                return new WzElementItem();
        }
    }
}
