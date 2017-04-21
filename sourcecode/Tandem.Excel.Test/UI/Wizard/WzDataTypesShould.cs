using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Tandem.Excel.UI.Wizard;

namespace Tandem.Excel.Test.UI.Wizard
{
    [TestClass]
    public class WzDataTypesShould
    {
        [TestMethod]
        public void convert_wz_element_group_to_table()
        {
            IWzElement element_1 = new WzElementGroup
            {
                WzElements = { 
                    new WzElementItem { Key = "Key_1", Value = new object[,] { { 1.1 } } },
                    new WzElementItem { Key = "Key_2", Value = new object[,] { { 1.2 } } }
                }
            };

            IWzElement element_2 = new WzElementGroup
            {
                WzElements = { 
                    new WzElementItem { Key = "Key_1", Value = new object[,] { { 2.1 } } },
                    new WzElementItem { Key = "Key_2", Value = new object[,] { { 2.2 } } }
                }
            };

             IWzElement element= new WzElementGroup
             {
                 WzElements = { 
                    element_1,
                    element_2
                }
             };

           
            DataTable table = WzDataTypes.ConvertToTable(element);
            Assert.AreEqual("Key_1", table.Columns[0].Caption);
            Assert.AreEqual("Key_2", table.Columns[1].Caption);
            Assert.AreEqual("1.1", table.Rows[0].ItemArray[0]);
            Assert.AreEqual("1.2", table.Rows[0].ItemArray[1]);
            Assert.AreEqual("2.1", table.Rows[1].ItemArray[0]);
            Assert.AreEqual("2.2", table.Rows[1].ItemArray[1]);
        }

        [TestMethod]
        public void convert_wz_element_item_to_array()
        {
            IWzElement wzElement = new WzElementItem { Key = "Key_1", Value = new object[,] { { 1.1 }, { 2.1 } } };
            DataView array = WzDataTypes.ConvertToArray(wzElement);
            Assert.AreEqual("Tandem.Excel.UI.Wizard.WzDataTypes+Ref`1[System.Object]", array.Table.Rows[0].ItemArray[0].ToString());
        }
    }
}
