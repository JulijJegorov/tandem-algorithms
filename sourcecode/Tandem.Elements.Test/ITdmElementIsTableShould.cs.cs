using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementIsTableShould
    {
        private ITdmElement row_1;
        private ITdmElement row_2;
        private ITdmElement row_3;
        private ITdmElement row_4;

        [TestInitialize]
        public void ITdmElementIsTableShouldInitialize()
        {
            row_1 = new TdmElementGroup
            {
                Group = { 
                    new TdmElementItem<object> { Key = "Key_1", Value = new object[,] { { 1.1 } } },
                    new TdmElementItem<object> { Key = "Key_2", Value = new object[,] { { 1.2 } } }
                }
            };

            row_2 = new TdmElementGroup
            {
                Group = { 
                    new TdmElementItem<object> { Key = "Key_1", Value = new object[,] { { 2.1 } } },
                    new TdmElementItem<object> { Key = "Key_2", Value = new object[,] { { 2.2 } } }
                }
            };

            row_3 = new TdmElementGroup
            {
                Group = { 
                    new TdmElementItem<object> { Key = "Key_3", Value = new object[,] { { 2.1 } } },
                    new TdmElementItem<object> { Key = "Key_2", Value = new object[,] { { 2.2 } } }
                }
            };

            row_4 = new TdmElementGroup
            {
                Group = { 
                    new TdmElementItem<object> { Key = "Key_1", Value = new object[,] { { 2.1 } } },
                    new TdmElementItem<object> { Key = "Key_2", Value = new object[,] { { 2.2 }, { 2.7 }  } }
                }
            };
        }


        [TestMethod]
        public void return_false_if_table_contains_only_one_row()
        {
            bool isTable = row_1.IsTable;

            Assert.IsFalse(isTable);
        }

        [TestMethod]
        public void return_true_if_table_contains_one_row()
        {
            ITdmElement table = new TdmElementGroup
            {
                Group = { row_1 }
            };

            bool isTable = table.IsTable;
            Assert.IsTrue(isTable);
        }

        [TestMethod]
        public void return_true_if_table_contains_two_rows()
        {
            ITdmElement table = new TdmElementGroup
            {
                Group = { row_1, row_2 }
            };

            bool isTable = table.IsTable;
            Assert.IsTrue(isTable);
        }

        [TestMethod]
        public void return_false_if_table_contains_two_rows_and_keys_mismatch()
        {
            ITdmElement table = new TdmElementGroup
            {
                Group = { row_1, row_3 }
            };

            bool isTable = table.IsTable;
            Assert.IsFalse(isTable);
        }

        [TestMethod]
        public void return_false_if_table_contains_one_row_and_non_single_vlaue()
        {
            ITdmElement table = new TdmElementGroup
            {
                Group = { row_4 }
            };

            bool isTable = table.IsTable;
            Assert.IsFalse(isTable);
        }

        [TestMethod]
        public void return_false_if_table_contains_three_rows_and_non_single_vlaue()
        {
            ITdmElement table = new TdmElementGroup
            {
                Group = { row_1, row_2, row_4 }
            };

            bool isTable = table.IsTable;
            Assert.IsFalse(isTable);
        }
    }
}
