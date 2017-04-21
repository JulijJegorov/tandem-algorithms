using System.Collections.Generic;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    public interface IWzElement
    {
        string Name { get; set; }
        string Key { get; set; }
        object[,] Value { get; set; }
        WzElementType ElementType { get; }
        List<IWzElement> WzElements { get; set; }
        bool IsSelected { get; set; }
        bool IsExpanded { get; set; }
        bool IsLinked { get; }
        void CopyElement(ITdmElement element);
    }
}
