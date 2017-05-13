using ExcelDna.Integration;
using System.Collections.Generic;
using System.Linq;
using Tandem.Elements;
using Tandem.Elements.Utilities;
using Tandem.Excel.Functions;
using Tandem.Excel.Utilities;
using YamlDotNet.RepresentationModel;

namespace Tandem.Excel
{
    public class ExcelFunctions
    {
        private const string _functionWizardMsg = "Disabled in FunctionWizard";
        private const string _persist_argument_description = "indicates if element object persists in memory after being used by another object. TRUE if omitted";
        public const string _dateTimeFormat = @"d-mmm-yy hh:mm:ss tt";

        private static readonly UtilityFunctions _utilityFunctions;
        private static readonly JsonFunctions _jsonFunctions;
        private static readonly PythonFunctions _pythonFunctions;
        private static readonly CellUtilities _cellUtilities;
        private static readonly ElementUtilities _elementUtilities;
        private static readonly RangeUtilties _rangeUtilities;

        static ExcelFunctions()
        {
            _utilityFunctions = new UtilityFunctions();
            _jsonFunctions = new JsonFunctions();
            _pythonFunctions = new PythonFunctions();
            _cellUtilities = new CellUtilities();
            _elementUtilities = new ElementUtilities();
            _rangeUtilities = new RangeUtilties();
        }

        [ExcelFunction(Description = "Active version of TdmExcelAddIn", Category = "tdmFunctions")]
        public static object tdmVersion()
        {
            return "Version: 0.00.1";
        }

        [ExcelFunction(Description = "Returns cell address: [workbook]sheet!cell", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmAddress()
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            return _cellUtilities.Address();
        }

        [ExcelFunction(Description = "Creates framework-compatible object", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmArray([ExcelArgument(Description = "array")]object[,] array,
                                        [ExcelArgument(Description = _persist_argument_description)]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            ITdmElement element = _elementUtilities.RangeToArray(array);
            return _addToPoolAndReturnFullName(element, persist);
        }

        [ExcelFunction(Description = "Creates framework-compatible object", Category = "tdmFunctions", IsThreadSafe = false) ]
        public static object tdmTable( [ ExcelArgument(Description = "optional: Column Names" ) ] object[,] columnNames,
                                        [ ExcelArgument(Description = "optional: Row Names") ] object[,] rowNames,
                                        [ ExcelArgument(Description = "array") ] object[,] array,
                                        [ ExcelArgument(Description = _persist_argument_description ) ] object persist )
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            object[,] columnNamesCheck = Optional.Check(columnNames, new object[,] { { null } });
            object[,] rowNamesCheck = Optional.Check(rowNames, new object[,] { { null } });
            ITdmElement element = _elementUtilities.RangeToTable(columnNamesCheck, rowNamesCheck, array);
            return _addToPoolAndReturnFullName(element, persist);
        }

        [ExcelFunction(Description = "Creates framework-compatible key-value pair", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmElement([ExcelArgument(Description = "key of the element")]object[,] key,
                                        [ExcelArgument(Description = "array or element object")]object[,] array,
                                        [ExcelArgument(Description = _persist_argument_description)]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            ITdmElement element = _elementUtilities.RangeToElement(key, array);
            return _addToPoolAndReturnFullName(element, persist);
        }

        [ExcelFunction(Description = "Creates framework-compatible nested key-value pairs", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmGrid([ExcelArgument(Description = "range of key-value pairs (arrays and element objects)")] object[,] grid,
                                     [ExcelArgument(Description = _persist_argument_description)]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            ITdmElement element = _elementUtilities.RangeToGrid(grid);

            return _addToPoolAndReturnFullName(element, persist);
        }

        [ExcelFunction(Description = "Creates framework-compatible nested key-value pairs with specified key", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmNestedGrid([ExcelArgument(Description = "keys of grid items")]object[,] key,
                                            [ExcelArgument(Description = "range of key-value pairs (arrays and element objects)")] object[,] grid,
                                            [ExcelArgument(Description = _persist_argument_description)]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;

            ITdmElement element = new TdmElementGroup();
            ITdmElement elementGrid = _elementUtilities.RangeToGrid(grid);
            elementGrid.Key = key[0, 0].ToString();
            element.Add(elementGrid);
            return _addToPoolAndReturnFullName(element, persist);
        }

        [ ExcelFunction( Description = "Finds element in a framework-compatible nested key-value pair", Category = "tdmFunctions", IsThreadSafe = false ) ]
        public static object tdmLookup( [ ExcelArgument( Description = "name of the element object" ) ]object[,] name,
                                        [ ExcelArgument( Description = "'/' delimited key address('Key1/SubKey2/SubSubKey3')" ) ] object keyAddress,
                                        [ ExcelArgument( Description = _persist_argument_description ) ] object persist )
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;

            var element = _elementUtilities.ReturnElementFromPool(name);
            string keys = (string)keyAddress;
            var lookupItem = element.Lookup(keys.Split('/').ToList());

            return lookupItem.HasChildren ?
                            _addToPoolAndReturnFullName(lookupItem, true) : lookupItem.Value;
        }

        [ ExcelFunction( Description = "Flattens framework-compatible nested key-value pair", Category = "tdmFunctions", IsThreadSafe = false ) ]
        public static object tdmFlatten( [ ExcelArgument(Description = "name of the element object" ) ] object[,] name,
                                         [ ExcelArgument(Description = "optional: Column names of the table to be returned" ) ] object[,] columnNames )
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            var element = _elementUtilities.ReturnElementFromPool(name);

            List<List<string>> lookupItems = null;
            if (!(columnNames[0, 0] is ExcelMissing))
            {
                lookupItems = new List<List<string>>();

                int rows = columnNames.GetLength(0);
                int columns = columnNames.GetLength(1);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        List<string> lookupItem = columnNames[i, j].ToString().Split('/').ToList();
                        lookupItems.Add(lookupItem);
                    }
                }
            }
            return _rangeUtilities.ElementToRange(element, lookupItems);
        }


        [ ExcelFunction( Description = "Merges up to five framework-compatible nested key-value pairs", Category = "tdmFunctions", IsThreadSafe = false ) ]
        public static object tdmMerge( [ ExcelArgument(Description = "optional: 1st element object" ) ] object[,] element1,
                                       [ ExcelArgument(Description = "optional: 2nd element object" ) ]object[,] element2,
                                       [ ExcelArgument(Description = "optional: 3d element object" )  ]object[,] element3,
                                       [ ExcelArgument(Description = "optional: 4th element object" ) ]object[,] element4,
                                       [ ExcelArgument(Description = "optional: 5th element object" ) ]object[,] element5,
                                       [ ExcelArgument(Description = _persist_argument_description)]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;

            ITdmElement element = new TdmElementGroup();

            ITdmElement element_1 = _elementUtilities.ReturnElementFromPool(element1);
            if (!element_1.IsEmpty)
                element = element.Merge(element_1);

            ITdmElement element_2 = _elementUtilities.ReturnElementFromPool(element2);
            if (!element_2.IsEmpty)
                element = element.Merge(element_2);

            ITdmElement element_3 = _elementUtilities.ReturnElementFromPool(element3);
            if (!element_3.IsEmpty)
                element = element.Merge(element_3);

            ITdmElement element_4 = _elementUtilities.ReturnElementFromPool(element4);
            if (!element_4.IsEmpty)
                element = element.Merge(element_4);

            ITdmElement element_5 = _elementUtilities.ReturnElementFromPool(element5);
            if (!element_5.IsEmpty)
                element = element.Merge(element_5);
  
            return _addToPoolAndReturnFullName(element, persist);
        }


        [ExcelFunction(Description = "Maps two framework-compatible nested key-value pairs", Category = "tdmFunctions", IsThreadSafe = false)]
        public static object tdmMap( [ ExcelArgument( Description = "1st element object" ) ] object[,] element1,
                                         [ ExcelArgument( Description = "2nd element object" )] object[,] element2,
                                         [ ExcelArgument( Description = _persist_argument_description )]object persist)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;

            ITdmElement element_1 = _elementUtilities.ReturnElementFromPool(element1).DeepCopy();
            ITdmElement element_2 = _elementUtilities.ReturnElementFromPool(element2);

            element_1.Map(element_2);

            return _addToPoolAndReturnFullName(element_1, persist);
        }

        [ExcelFunction( Description = "Overrides current name of a tandem object", Category = "tdmFunctions", IsThreadSafe = false ) ]
        public static object tdmAlias( [ ExcelArgument( Description = "name of existing element object" ) ] object[,] element,
                                       [ ExcelArgument( Description = "alias of element object" ) ] object[,] alias)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;

            return _elementUtilities.AssignAlias(element, alias);
        }


        [ExcelFunction( Description = "Runs Pyhton scripts with input arguments 'args'", Category = "tdmFunctions", IsThreadSafe = false ) ]
        public static object tdmPyScript( [ ExcelArgument( Description = "full path of the python .py script" ) ] object[,] fullPath, 
                                            [ ExcelArgument( Description = "arguments of the python function" ) ] object[,] args,
                                            [ ExcelArgument(Description = "Set to TRUE to save input arguments file (can be used to debug your Python code). FALSE if omitted")]object saveInputFile)
        {
            if (_IsInFunctionWizard()) return _functionWizardMsg;
            bool saveFile = Optional.Check(saveInputFile, false);
            var tdmElement = _pythonFunctions.tdmPyScript(fullPath[0, 0].ToString(), args, saveFile);

            return _addToPoolAndReturnFullName(tdmElement, true);
        }

      

        //[ExcelFunction(Description = "Writes nested key-value pairs to Space", Category = "tdmFunctions", IsThreadSafe = false)]
        //public static object tdmWriteToSpace([ExcelArgument(Description = "Database name")] string database,
        //                                     [ExcelArgument(Description = "Collection name")]string collection,
        //                                     [ExcelArgument(Description = "Name of the element object")] object[,] name,
        //                                     [ExcelArgument(Description = "Set to TRUE to write to  Space. FALSE if omitted")]object update)
        //{
        //    if (_IsInFunctionWizard()) return _functionWizardMsg;

        //    bool writeToSpace = Optional.Check(update, false);
        //    if (!writeToSpace)
        //        return "Set 'update' to TRUE to write to Space";

        //    _establishDefaultConnection();
        //    object asyncRun = ExcelAsyncUtil.Run("tdmWriteToSpace", new object[] { database, collection, name }, delegate
        //    {
        //        var element = _elementUtilities.ReturnElementFromPool(name);
        //        if (element.IsTable)
        //        {
        //            Space.WriteManyAsync(database, collection, element).Wait();
        //        }
        //        else
        //        {
        //            Space.WriteAsync(database, collection, element).Wait();
        //        }
        //        return String.Format("Inserted on {0}", DateTime.Now.ToString(_dateTimeFormat));
        //    });

        //    if (asyncRun.Equals(ExcelError.ExcelErrorNA))
        //    {
        //        return String.Format("Inserting into {0} : {1} ...", database, collection);
        //    }
        //    return asyncRun;
        //}


        //[ExcelFunction(Description = "Updates nested key-value pairs in Space", Category = "tdmFunctions", IsThreadSafe = false)]
        //public static object tdmUpdateSpace([ExcelArgument(Description = "Database name")] string database,
        //                                     [ExcelArgument(Description = "Collection name")]string collection,
        //                                     [ExcelArgument(Description = "Name of the element object")] object[,] name,
        //                                     [ExcelArgument(Description = "Set to TRUE to update  Space. FALSE if omitted")]object update)
        //{
        //    if (_IsInFunctionWizard()) return _functionWizardMsg;

        //    bool writeToSpace = Optional.Check(update, false);
        //    if (!writeToSpace)
        //        return "Set 'update' to TRUE to update Space";

        //    _establishDefaultConnection();
        //    object asyncRun = ExcelAsyncUtil.Run("tdmUpdateSpace", new object[] { database, collection, name }, delegate
        //    {
        //        var element = _elementUtilities.ReturnElementFromPool(name);
        //        if (element.IsTable)
        //        {
        //            foreach (var item in element.Group)
        //            {
        //                Space.UpdateAsync( database, collection, item).Wait();
        //            }
        //        }
        //        else
        //        {
        //            Space.UpdateAsync(database, collection, element).Wait();
        //        }
        //        return String.Format("Updated on {0}", DateTime.Now.ToString(_dateTimeFormat));
        //    });

        //    if (asyncRun.Equals(ExcelError.ExcelErrorNA))
        //    {
        //        return String.Format("Updating {0} : {1} ...", database, collection);
        //    }
        //    return asyncRun;
        //}

        //[ExcelFunction(Description = "Reads nested key-value pairs from Space", Category = "tdmFunctions", IsThreadSafe = false)]
        //public static object tdmReadFromSpace(  [ExcelArgument(Description = "Database name")] string database,
        //                                        [ExcelArgument(Description = "Collection name")]string collection,
        //                                        [ExcelArgument(Description = "Optional: 'Match' element object")] object[,] match,
        //                                        [ExcelArgument(Description = "Optional: Group")] object[,] group,
        //                                        [ExcelArgument(Description = "Optional: Project")] object[,] project,
        //                                        [ExcelArgument(Description = "Optional: Sort")] object[,] sort,
        //                                        [ExcelArgument(Description = "Set to TRUE to read from Space. FALSE if omitted")] object update,
        //                                        [ExcelArgument(Description = _persist_argument_description)]object persist)
        //{
        //    if (_IsInFunctionWizard()) return _functionWizardMsg;

        //    bool readFromSpace = Optional.Check(update, false);

        //    if (!readFromSpace)
        //        return "Set 'update' to TRUE to read from Space";

        //    _establishDefaultConnection();

        //    var elementMatch = _elementUtilities.ReturnElementFromPool(match);
        //    var elementSort = _elementUtilities.ReturnElementFromPool(sort);
        //    var elementGroup = _elementUtilities.ReturnElementFromPool(group);
        //    var elementProject = _elementUtilities.ReturnElementFromPool(project);

        //    ITdmElement result = Space.ReadAsync(database, collection, elementMatch, elementGroup, elementProject, elementSort).Result;
        //    return _addToPoolAndReturnFullName(result, true);
        //}



        private static string _addToPoolAndReturnFullName(ITdmElement tdmElement, object persist)
        {
            var container = new TdmElementContainer
            {
                Persist = Optional.Check(persist, true),
                Name = _cellUtilities.Address(),
                Element = tdmElement
            };

            container.AddToPool();
            return container.FullName;
        }

        private static bool _IsInFunctionWizard()
        {
            bool isInWizard;
            try
            {
                isInWizard = ExcelDnaUtil.IsInFunctionWizard();
            }
            catch
            {
                isInWizard = false;
            }
            return isInWizard;
        }

        /// <summary>
        /// If not connected to 'Space', establish default connection as specified in the configuration file. </summary>
        private static void _establishDefaultConnection()
        {
            if (!Space.Instance.IsConnected)
            {
                var connection = ConfigFile.Instance.SpaceDefaultConnection;
                Space.Instance.Connect(connection.Children[new YamlScalarNode("uri")].ToString());
            }
        }
    }
}
