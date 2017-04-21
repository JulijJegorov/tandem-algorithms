using System;
namespace Tandem.Excel.Utilities
{
    public interface ICellUtilities
    {
        string Address();
        string GetName(object cell);
        bool IsEmpty(object cell);
        bool IsJson(object cell);
        string Placeholder { get; }
        string SplitName(object cell);
    }
}
