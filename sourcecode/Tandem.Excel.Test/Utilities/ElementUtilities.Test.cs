//using Microsoft.QualityTools.Testing.Fakes;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Linq;
//using Tandem.Elements;
//using Tandem.Excel.Utilities;
//using Tandem.Excel.Utilities.Fakes;

//namespace Tandem.Excel.Test.Utilities
//{
//    [TestClass]
//    public class ElementUtilitiesShould
//    {
//        private const string placeholder = "{}";

//        private ElementUtilities _sut;

//        [TestInitialize]
//        public void CellUtilitiesTestInitialize()
//        {
//            _sut = new ElementUtilities();
//        }

//        [TestMethod]
//        public void convert_simple_range_to_tdmelement()
//        {
//            ITdmElement tdmElement;
//            object[,] cropped_array = new object[,] { { 0.0 }, { 1.1 } };

//            using (ShimsContext.Create())
//            {
//                ShimRangeUtilties.AllInstances.CropObjectMdArray2Int32Int32Int32Int32 = (rangeUtilites, array, startRow, startColumn, endRow, endColumn) => { return cropped_array; };
//                tdmElement = _sut.RangeToElement(new object[,] { });
//            }

//            object[,] actual = tdmElement.GetValue<object>();
//            Assert.IsNull(tdmElement.Key);
//            Assert.IsTrue(cropped_array.Cast<double>().SequenceEqual(actual.Cast<double>()));
//        }

//        [TestMethod]
//        public void convert_table_range_to_tdmelement()
//        {
//            ITdmElement tdmElement;
//            object[,] expected = null;
//            object[,] cropped_array = new object[,] { { "Column_1", "Column_2", "Column_1" }, { 2.1, 2.2, 2.3 }, { 3.1, 3.2, 3.3 } };

//            using (ShimsContext.Create())
//            {
//                ShimRangeUtilties.AllInstances.CropObjectMdArray2Int32Int32Int32Int32 = (rangeUtilites, array, startRow, startColumn, endRow, endColumn) => { return cropped_array; };
//                tdmElement = _sut.RangeToElement(new object[,] { });
//            }

//            Assert.AreEqual(cropped_array[0, 0], tdmElement.Group[0].Key);
//            Assert.AreEqual(cropped_array[0, 1], tdmElement.Group[1].Key);
//            Assert.AreEqual(cropped_array[0, 2], tdmElement.Group[2].Key);
            
//            expected = new object[,] {{ 2.1 }, { 3.1 } };
//            Assert.IsTrue(expected.Cast<double>().SequenceEqual(tdmElement.Group[0].GetValue<double>().Cast<double>()));
//            expected = new object[,] { { 2.2 }, { 3.2 } };
//            Assert.IsTrue(expected.Cast<double>().SequenceEqual(tdmElement.Group[1].GetValue<double>().Cast<double>()));
//            expected = new object[,] { { 2.3 }, { 3.3 } };
//            Assert.IsTrue(expected.Cast<double>().SequenceEqual(tdmElement.Group[2].GetValue<double>().Cast<double>()));
//        }

//        [TestMethod]
//        public void convert_grid_with_two_keys_to_tdmelement()
//        {
//            ITdmElement tdmElement;

//            object[,] expected_value_1 = new object[,] { { 2.1, 2.2 } };
//            object[,] expected_value_2 = new object[,] { { 3.1, 3.2 } };

//            object[,] grid = new object[,] { { "Key_1", 2.1, 2.2}, { "Key_2", 3.1, 3.2 } };
//            using (ShimsContext.Create())
//            {
//                ShimRangeUtilties.AllInstances.CropObjectMdArray2Int32Int32Int32Int32 = (rangeUtilites, array, startRow, startColumn, endRow, endColumn) => { return expected_value_1; };
//                tdmElement = _sut.GridToElement(grid);
//            }
//            Assert.AreEqual(grid[0, 0], tdmElement.Group[0].Key);
//            Assert.IsTrue(expected_value_1.Cast<double>().SequenceEqual(tdmElement.Group[0].GetValue<double>().Cast<double>()));

//            using (ShimsContext.Create())
//            {
//                ShimRangeUtilties.AllInstances.CropObjectMdArray2Int32Int32Int32Int32 = (rangeUtilites, array, startRow, startColumn, endRow, endColumn) => { return expected_value_2; };
//                tdmElement = _sut.GridToElement(grid);
//            }
//            Assert.AreEqual(grid[1, 0], tdmElement.Group[1].Key);
//            Assert.IsTrue(expected_value_2.Cast<double>().SequenceEqual(tdmElement.Group[1].GetValue<double>().Cast<double>()));
//        }
//    }
//}
