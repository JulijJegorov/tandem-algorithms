using Tandem.Elements;
using ExcelDna.Integration;
using System.Collections.Generic;

namespace Tandem.Excel.Utilities
{
    public class ElementUtilities
    {
        private readonly CellUtilities _cellUtilities;
        private readonly RangeUtilties _rangeUtilities;
        private readonly TdmContainerPool _containerPool;

        public ElementUtilities(CellUtilities cellUtilites, RangeUtilties rangeUtilities, TdmContainerPool containerPool)
        {
            _cellUtilities = cellUtilites;
            _rangeUtilities = rangeUtilities;
            _containerPool = containerPool;
        }

        public ElementUtilities()
            : this(new CellUtilities(), new RangeUtilties(), TdmContainerPool.Instance) { }

        public ITdmElement RangeToGrid(object[,] grid)
        {
            int upperLimit = 0;
            int lowerLimit = 0;

            ITdmElement element = new TdmElementGroup();

            while (lowerLimit < grid.GetLength(0))
            {
                upperLimit = _rangeUtilities.NextNonEmptyRow(grid, lowerLimit);
                lowerLimit = _rangeUtilities.NextNonEmptyRow(grid, upperLimit + 1);

                object[,] croppedRange = _rangeUtilities.Crop(grid, upperLimit, 1, lowerLimit);

                object cell = (object)croppedRange[0, 0];
                string elementName = _cellUtilities.SplitName(cell).ToString();

                ITdmElement item = null;
                if (_containerPool.Contains(elementName))
                    item = _getElementFromPool(elementName);
                else
                    item = _rangeToArray(croppedRange);

                item.Key = grid[upperLimit, 0].ToString();

                element.Add(item);
            }
            return element;
        }

        public ITdmElement ReturnElementFromPool(object[,] elementName)
        {
            object cell = (object)elementName[0, 0];
            string name = _cellUtilities.SplitName(cell).ToString();

            return _containerPool.Contains(name) ?
                            _getElementFromPool(name) : new TdmElementGroup();
        }


        public string AssignAlias(object[,] name, object[,] alias)
        {
            object cell = (object)name[0, 0];
            string splitName = _cellUtilities.SplitName(cell).ToString();

            string aliasToSrt = alias[0,0].ToString();

            var container = _containerPool.Get(splitName);

            if (container != null && alias != null)
            {
                if (container.Name != aliasToSrt)
                {
                    _containerPool.Remove(splitName);
                    container.Alias = aliasToSrt;
                }
                _containerPool.Add(container);
            }
            return container.FullName;
        }


        public ITdmElement RangeToElement_old(object[,] array)
        {
            object[,] cropedArray = _rangeUtilities.Crop(array);
            return _convertRangeToElement(cropedArray);
        }

        public ITdmElement RangeToElement(object[,] key, object[,] array)
        {
            object[,] croppedKey = _rangeUtilities.Crop(key);
            object[,] croppedArray = _rangeUtilities.Crop(array);

            object cell = (object)array[0, 0];
            string elementName = _cellUtilities.SplitName(cell).ToString();

            ITdmElement item = null;
            if (_containerPool.Contains(elementName))
            {
                item = _getElementFromPool(elementName);
            }
            else
            {
                item = _rangeToArray(croppedArray);
            }

            item.Key = croppedKey[0, 0].ToString();
            return new TdmElementGroup { Group = { item } };
        }

        public ITdmElement RangeToArray(object[,] array)
        {
            object[,] cropedArray = _rangeUtilities.Crop(array);
            return _rangeToArray(cropedArray);
        }

        public ITdmElement RangeToTable(object[,] columnNames, object[,] rowNames, object[,] array)
        {
            object[,] croppedColumnNames = _rangeUtilities.Crop(columnNames);
            object[,] croppedRowNames = _rangeUtilities.Crop(rowNames);
            object[,] croppedArray = _rangeUtilities.Crop(array);

            return _rangeToTable(croppedColumnNames, croppedRowNames, croppedArray);
        }

        private ITdmElement _getElementFromPool(string elementName)
        {
            TdmElementContainer container = _containerPool.Get(elementName);
            if (!container.Persist)
            {
                _containerPool.Remove(elementName);
            }
            return container.Element;
        }

        private ITdmElement _rangeToArray( object[,] array)
        {
            return new TdmElementItem<object> { Key = null, Value = array }; 
        }

        private ITdmElement _rangeToTable(object[,] columnNames, object[,] rowNames, object[,] array)
        {
            int columnNamesCount = columnNames.GetLength(1);
            int rowNamesCount = rowNames.GetLength(0);
            if (columnNamesCount == 1 && columnNames[0, 0] is ExcelError) columnNamesCount = 0;
            if (rowNamesCount == 1 && rowNames[0, 0] is ExcelError) rowNamesCount = 0;
         
            ITdmElement result = new TdmElementGroup();
            for (int i = 0; i < array.GetLength(0); i++)
            {
                string rowName = null;
                if (i < rowNamesCount)
                    rowName = (string)rowNames[i, 0];
                else
                    rowName = "[ " + i + " ]";
                    
                ITdmElement group = new TdmElementGroup();
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    string colName = null;
                    if(j < columnNamesCount)
                        colName = (string)columnNames[0, j];
                    else
                        colName = "[ " + j + " ]";
                        
  
                    object[,] row = _rangeUtilities.Slice(array, startRow: i, startColumn: j, endRow: i + 1, endColumn: j + 1);
                    ITdmElement item = new TdmElementItem<object>() { Key = colName, Value = row };
                    group.Insert(rowName, item);
                }
                result.Add(group);
            }
            return result;
        }


        public ITdmElement _convertRangeToElement(object[,] croppedRange)
        {
            object cell = (object)croppedRange[0, 0];
            string elementName = _cellUtilities.SplitName(cell).ToString();

            return _containerPool.Contains(elementName) ?
                                   _getElementFromPool(elementName) : _rangeToElement(croppedRange);
        }


        private ITdmElement _rangeToElement(object[,] range)
        {
            ITdmElement result = null;
            if (!_rangeUtilities.IsTable(range))
            {
                result = new TdmElementItem<object> { Key = null, Value = range };
            }
            else
            {
                result = new TdmElementGroup();
                for (int i = 1; i < range.GetLength(0); i++)
                {
                    
                    ITdmElement group = new TdmElementGroup();
                    for (int j = 0; j < range.GetLength(1); j++)
                    {
                        string itemKey = (string)range[0, j];
                        object[,] itemValue = _rangeUtilities.Slice(range, startRow: i, startColumn: j,  endRow: i+1, endColumn: j+1);
                        ITdmElement item = new TdmElementItem<object>() { Key = itemKey, Value = itemValue };
                        group.Insert("[ " + i + " ]", item);
                    }
                    result.Add(group);
                }
            }
            return result;
        }
    }
}
