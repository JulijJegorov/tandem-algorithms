using System;
namespace Tandem.Excel.Utilities
{
    public interface IRangeUtilties
    {
        object[,] Crop(object[,] array, int startRow = 0, int startColumn = 0, int endRow = int.MaxValue, int endColumn = int.MaxValue);
        bool IsElement(object[,] array);
        bool IsTable(object[,] array);
        int NextNonEmptyRow(object[,] array, int startRow, int startColumn = 0);
        object[,] Slice(object[,] array, int startRow = 0, int startColumn = 0, int endRow = int.MaxValue, int endColumn = int.MaxValue);
    }
}
