using System;
using System.Collections.Generic;
using Tandem.Elements;

namespace Tandem.Excel.Utilities
{
    public class RangeUtilties
    {
        private const int _maxRows = 10000;
        private const int _maxColumns = 100;

        private readonly CellUtilities _cellUtilities;
        private readonly TdmContainerPool _tdmPool;

        public RangeUtilties(CellUtilities cellUtilites, TdmContainerPool tdmContainerPool)
        {
            _cellUtilities = cellUtilites;
            _tdmPool = tdmContainerPool;
        }

        public RangeUtilties()
            : this(new CellUtilities(), TdmContainerPool.Instance){ }

        public object[,] Slice(object[,] array, int startRow = 0, int startColumn = 0, int endRow = int.MaxValue, int endColumn = int.MaxValue)
        {
            if (endRow == int.MaxValue)
                endRow = array.GetLength(0);

            if (endColumn == int.MaxValue)
                endColumn = array.GetLength(1);

            if (endRow <= startRow || endColumn <= startColumn)
            {
                object[,] outArray = new object[1, 1];
                outArray[0, 0] = (object)Convert.ChangeType(_cellUtilities.Placeholder, typeof(object));
                return outArray;
            }
            else
            {
                object[,] outArray = new object[(endRow - startRow), (endColumn - startColumn)];

                for (int i_src = startRow, i_dsc = 0; i_src < endRow; i_src++, i_dsc++)
                {
                    for (int j_src = startColumn, j_dsc = 0; j_src < endColumn; j_src++, j_dsc++)
                    {
                        if (_cellUtilities.IsEmpty(array[i_src, j_src]))
                        {
                            outArray[i_dsc, j_dsc] = (object)Convert.ChangeType(_cellUtilities.Placeholder, typeof(object));
                        }
                        else
                        {
                            outArray[i_dsc, j_dsc] = array[i_src, j_src];
                        }
                    }
                }
                return outArray;
            }
        }

        public object[,] Crop(object[,] array, int startRow = 0, int startColumn = 0, int endRow = int.MaxValue, int endColumn = int.MaxValue)
        {
            if (endRow == int.MaxValue)
                endRow = array.GetLength(0);

            if (endColumn == int.MaxValue)
                endColumn = array.GetLength(1);

            int ULHSRow = _maxRows;
            int ULHSColumn = _maxColumns;
            int LRHSRow = 0; int LRHSColumn = 0;

            for (int i = startRow; i < endRow; i++)
            {
                for (int j = startColumn; j < endColumn; j++)
                {
                    if (!_cellUtilities.IsEmpty(array[i, j]))
                    {
                        ULHSRow = Math.Min(ULHSRow, i);
                        ULHSColumn = Math.Min(ULHSColumn, j);
                        LRHSRow = Math.Max(LRHSRow, i);
                        LRHSColumn = Math.Max(LRHSColumn, j);
                    }
                }
            }

            object[,] outArray = Slice(array, ULHSRow, ULHSColumn, LRHSRow + 1, LRHSColumn + 1);

            return outArray;
        }

        public bool IsTable(object[,] array)
        {
            int rows = array.GetLength(0);
            int cols = array.GetLength(1);

            if (cols == 1 || rows == 1)
                return false;
            for (int j = 0; j < array.GetLength(1); j++)
            {
                if (array[0, j].GetType() != typeof(string))
                    return false;
            }
            return true;
        }

        public int NextNonEmptyRow(object[,] array, int startRow, int startColumn = 0)
        {
            int endRow = array.GetLength(0);
            for (int i = startRow; i < endRow; i++)
            {
                if (!_cellUtilities.IsEmpty(array[i, startColumn]))
                {
                    return i;
                }
            }
            return endRow;
        }

        public bool IsElement(object[,] array)
        {
            object cell = (object)array[0, 0];
            return _tdmPool.Contains(_cellUtilities.SplitName(cell));
        }

        public object[,] ElementToRange(ITdmElement element, List<List<string>> keys = null)
        {
            int row = 0; int col = 0;
            object[,] array = new object[_maxRows, _maxColumns];

            for (int i = 0; i < _maxRows; i++)
            {
                for (int j = 0; j < _maxColumns; j++)
                {
                    array[i, j] = "";
                }
            }
    
            return keys == null ? _elementsToRange(element, ref array, ref row, col) : 
                                    _tableToRangeLookup(element, ref array, ref row, col, keys);
        }

        public object[,] _tableToRangeLookup(ITdmElement element, ref object[,] range, ref int startRow, int startColumn, List<List<string>>keys)
        {
            int row = startRow;

            foreach (ITdmElement items in element.Group)
            {
                int column = startColumn;

                foreach (List<string> key in keys)
                {
                    List<string> copy = new List<string>(key);
                    ITdmElement item = items.Lookup(copy);

                    if (item == null)
                        range[row, column] = " ";
                    else
                        range[row, column] = item.GetValue<object>()[0, 0];

                    column = column + 1;
                }
                row = row + 1;
            }
                

                startRow = row + 1;
            
            return range;
        }


        public object[,] _tableToRange(ITdmElement element, ref object[,] range, ref int startRow, int startColumn)
        {
            int row = startRow + 1;
            foreach (ITdmElement items in element.Group)
            {
                int column = startColumn + 1;
                foreach (ITdmElement item in items.Group)
                {
                    range[startRow, column] = item.Key;
                    range[row, startColumn] = items.Key;
                    range[row, column] = item.GetValue<object>()[0, 0];
                    column = column + 1;
                }
                row = row + 1;
            }
            startRow = row + 1;
            return range;
        }

        public object[,] _elementsToRange(ITdmElement element, ref object[,] range, ref int startRow, int startColumn)
        {
            if (element.IsTable)
            {
                return _tableToRange(element, ref range, ref startRow, startColumn);
            }

            if (element.HasChildren)
            {
                foreach (var item in element.Group)
                {
                    range[startRow, startColumn] = item.Key;
                    startRow = startRow + 1;
                    _elementsToRange(item, ref range, ref startRow, startColumn + 1);
                }
            }
            else
            {
                startRow = Math.Max(startRow - 1, 0);
                object[,] array = element.GetValue<object>();
                for (int i = 0; i < array.GetLength(0); i++)
                {
                    for (int j = 0; j < array.GetLength(1); j++)
                    {
                        range[startRow + i, startColumn + j] = array[i, j]; 
                    }
                }
                startRow = startRow + array.GetLength(0) + 1;
            }
            return range;
        }
    }
}
