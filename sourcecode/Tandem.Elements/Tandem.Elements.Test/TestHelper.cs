using System.Collections.Generic;
using System.Text;

namespace Tandem.Elements.Test
{
    public static class TestHelper
    {
        public static TdmElement TdmSingleValue()
        {
            return new TdmElement
            {
                Key = "SingleValue",
                Value = new object[,] { { 1.1 } },
            };
        }

        public static TdmElement Tdm1DArray()
        {
            return new TdmElement
            {
                Key = "1DArray",
                Value = new object[,] { { 1.1, 1.2, 1.3 } },
            };
        }

        public static TdmElement Tdm2DArray()
        {
            return new TdmElement
            {
                Key = "2DArray",
                Value = new object[,] { { 1.1 , 1.2 } ,
                                        { 2.1 , 2.2} },
            };
        }

        public static TdmElement TdmTable()
        {
            return new TdmElement
            {
                Key = "Table",
                TdmElementList = new List<TdmElement>()
                 {
                     new TdmElement
                     {
                         Key = "Column_1",
                         Value = new object[,] { { 2.1 }, { 2.2 } },
                     },
                     new TdmElement
                     {
                         Key = "Column_2",
                         Value = new object[,] { { 2.1 }, { 2.2 } },
                     },
                     new TdmElement
                     {
                         Key = "Column_3",
                         Value = new object[,] { { 3.1 }, { 3.2 } },
                     } 
                 }
            };
        }

        public static TdmElement TdmDictionary()
        {
            return new TdmElement
            {
                Key = "Dictionary",
                TdmElementList = new List<TdmElement>()
                 {
                     new TdmElement
                     {
                         Key = "Element_1",
                         Value = new object[,] { { 2.1 } },
                     },
                     new TdmElement
                     {
                         Key = "Element_2",
                         Value = new object[,] { { 2.1 }, { 2.2 }, { 2.3 } },
                     },
                     new TdmElement
                     {
                         Key = "Element3",
                         Value = new object[,] { { 3.1 }, { 3.2 } },
                     } 
                 }
            };
        }

        public static TdmElement TdmTwoLevelsNestedTypeOne()
        {
            return new TdmElement
            {
                Key = "TwoLevelsNestedTypeOne",
                TdmElementList = new List<TdmElement>()
                 {
                     new TdmElement
                     {
                         Key = "TypeOneElement_1",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.TdmTable()
                         }
                     },
                     new TdmElement
                     {
                         Key = "TypeOneElement_2",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.TdmSingleValue(),
                         },
                     },
                     new TdmElement
                     {
                         Key = "TypeOneElement3",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.Tdm1DArray(),
                              TestHelper.Tdm2DArray(),
                         },
                     } 
                 }
            };
        }

        public static TdmElement TdmTwoLevelsNestedTypeTwo()
        {
            return new TdmElement
            {
                Key = "TwoLevelsNestedTypeTwo",
                TdmElementList = new List<TdmElement>()
                 {
                     new TdmElement
                     {
                         Key = "TypeTwo_Element_1",
                         TdmElementList = new List<TdmElement>()
                         {
                             TestHelper.TdmSingleValue(),
                         }
                     },
                     new TdmElement
                     {
                         Key = "TypeTwo_Element 3",
                         TdmElementList = new List<TdmElement>()
                         {
                             TestHelper.TdmTable()
                         },
                     } 
                 }
            };
        }

        public static TdmElement TdmThreeLevelsNested()
        {
            return new TdmElement
            {
                Key = "ThreeLevelsNested",
                TdmElementList = new List<TdmElement>()
                 {
                     new TdmElement
                     {
                         Key = "Element_1",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.Tdm1DArray(),
                              TestHelper.TdmTwoLevelsNestedTypeOne(),
                         }
                     },
                     new TdmElement
                     {
                         Key = "Element_2",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.TdmValueEmpty(),
                              TestHelper.TdmTwoLevelsNestedTypeTwo(),
                         },
                     },
                     new TdmElement
                     {
                         Key = "Element3",
                         TdmElementList = new List<TdmElement>()
                         {
                              TestHelper.TdmValueAndListEmpty(),
                              TestHelper.Tdm2DArray(),
                         },
                     } 
                 }
            };
        }

        public static TdmElement TdmListEmpty()
        {
            return new TdmElement
            {
                Key = "ListEmpty",
                Value = new object[,] { { 1.1 } },
            };
        }

        public static TdmElement TdmValueEmpty()
        {
            return new TdmElement
            {
                Key = "ValueEmpty",
                TdmElementList = new List<TdmElement>()
                {
                    new TdmElement
                    {
                        Key = "ListEmpty",
                        Value = new object[,] { { 1.1 }},
                    }
                }
            };
        }

        public static TdmElement TdmValueAndListEmpty()
        {
            return new TdmElement
            {
                Key = "ValueAndListEmpty",
            };
        }

        public static string jsonTdmSingleValue()
        {
            string json = "{\"Key\":\"SingleValue\",\"Array\":[[1.1]]}";
            return json;
        }

        public static string jsonTdm2DArray()
        {
            string json = "{\"Key\":\"2DArray\",\"Array\":[[1.1,1.2],[2.1,2.2]]}";
            return json;
        }

        public static string jsonTdmDictionary()
        {
            var json = new StringBuilder();
            json.Append("{\"Key\":\"Dictionary\",\"Dictionary\":[{\"Key\":\"Element_1\",\"Array\":[[2.1]]},");
            json.Append("{\"Key\":\"Element_2\",\"Array\":[[2.1],[2.2],[2.3]]},");
            json.Append("{\"Key\":\"Element3\",\"Array\":[[3.1],[3.2]]}]}");
            return json.ToString();
        }


        public static string jsonTdmThreeLevelsNested()
        {
            var json = new StringBuilder();
            json.Append("{\"Key\":\"ThreeLevelsNested\",\"Dictionary\":[{\"Key\":\"Element_1\",\"Dictionary\":");
            json.Append("[{\"Key\":\"1DArray\",\"Array\":[[1.1,1.2,1.3]]},{\"Key\":\"TwoLevelsNestedTypeOne\",\"Dictionary\":");
            json.Append("[{\"Key\":\"TypeOneElement_1\",\"Dictionary\":[{\"Key\":\"Table\",\"Dictionary\":");
            json.Append("[{\"Key\":\"Column_1\",\"Array\":[[2.1],[2.2]]},{\"Key\":\"Column_2\",\"Array\":[[2.1],[2.2]]},");
            json.Append("{\"Key\":\"Column_3\",\"Array\":[[3.1],[3.2]]}]}]},{\"Key\":\"TypeOneElement_2\",\"Dictionary\":");
            json.Append("[{\"Key\":\"SingleValue\",\"Array\":[[1.1]]}]},{\"Key\":\"TypeOneElement3\",\"Dictionary\":");
            json.Append("[{\"Key\":\"1DArray\",\"Array\":[[1.1,1.2,1.3]]},{\"Key\":\"2DArray\",\"Array\":[[1.1,1.2],[2.1,2.2]]}]}]}]},");
            json.Append("{\"Key\":\"Element_2\",\"Dictionary\":[{\"Key\":\"ValueEmpty\",\"Dictionary\":");
            json.Append("[{\"Key\":\"ListEmpty\",\"Array\":[[1.1]]}]},{\"Key\":\"TwoLevelsNestedTypeTwo\",\"Dictionary\":");
            json.Append("[{\"Key\":\"TypeTwo_Element_1\",\"Dictionary\":[{\"Key\":\"SingleValue\",\"Array\":[[1.1]]}]},");
            json.Append("{\"Key\":\"TypeTwo_Element 3\",\"Dictionary\":[{\"Key\":\"Table\",\"Dictionary\":");
            json.Append("[{\"Key\":\"Column_1\",\"Array\":[[2.1],[2.2]]},{\"Key\":\"Column_2\",\"Array\":[[2.1],[2.2]]},");
            json.Append("{\"Key\":\"Column_3\",\"Array\":[[3.1],[3.2]]}]}]}]}]},{\"Key\":\"Element3\",\"Dictionary\":");
            json.Append("[{\"Key\":\"ValueAndListEmpty\"},{\"Key\":\"2DArray\",\"Array\":[[1.1,1.2],[2.1,2.2]]}]}]}");

            return json.ToString();
        }
    }
}



