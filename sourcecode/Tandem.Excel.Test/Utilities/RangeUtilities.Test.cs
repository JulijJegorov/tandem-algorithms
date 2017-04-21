using ExcelDna.Integration;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Tandem.Excel.Utilities;
using Tandem.Excel.Utilities.Fakes;
using Tandem.Elements;

namespace Tandem.Excel.Test.Utilities
{
    [TestClass]
    public class RangeUtilitiesTest
    {
        private const string placeholder = "{}";

        private RangeUtilties _sut;
        object[,] _testArray;
        object[,] _testArrayWithMissingFields;

        [TestInitialize]
        public void RangeUtilitiesTestInitialize()
        {
            _sut = new RangeUtilties();
            _testArray = new object[,] { { 1.1, 1.2, 1.3 },
                                         { 2.1, 2.2, 2.3 } };

            _testArrayWithMissingFields = new object[,] { { ExcelMissing.Value, ExcelEmpty.Value, ExcelMissing.Value },
                                                          { 2.1, 2.2, 2.3 },
                                                          { ExcelMissing.Value, ExcelEmpty.Value, ExcelMissing.Value } };
        }

        [TestMethod]
        public void return_the_same_array_when_non_empty_array_is_sliced_and_no_dimensions_are_specified()
        {
            object[,] actual = _sut.Slice(_testArray);

            Assert.IsTrue(_testArray.Cast<double>().SequenceEqual(actual.Cast<double>()));
        }

        [TestMethod]
        public void return_two_lower_rhs_values_when_non_empty_array_is_sliced_and_dimensions_1_1_are_specified()
        {
            object[,] expected = { { 2.2, 2.3 } };
            object[,] actual = _sut.Slice(_testArray, 1, 1);
            Assert.IsTrue(expected.Cast<double>().SequenceEqual(actual.Cast<double>()));
    
        }

        [TestMethod]
        public void return_two_upper_rhs_values_when_non_empty_array_is_sliced_and_dimensions_0_1_1_3_are_specified()
        {
            object[,] expected = { { 1.2, 1.3 } };
            object[,] actual = _sut.Slice(_testArray, 0, 1, 1, 3);
            Assert.IsTrue(expected.Cast<double>().SequenceEqual(actual.Cast<double>()));
        }

        [TestMethod]
        public void return_placeholder_when_non_empty_array_is_sliced_and_wrong_dimensions_are_specified()
        {
            object[,] expected = { { placeholder } };
            object[,] actual = _sut.Slice(_testArray, 5, 1, 1, 3);
            Assert.IsTrue(expected.Cast<object>().SequenceEqual(actual.Cast<object>()));
        }

        [TestMethod]
        public void return_non_empty_array_when_array_with_missing_fields_is_sliced()
        {
            object[,] expected = new object[,] { { placeholder, placeholder, placeholder },
                                                { 2.1, 2.2, 2.3 },
                                                { placeholder, placeholder, placeholder } };

            object[,] actual = _sut.Slice(_testArrayWithMissingFields);
            Assert.IsTrue(expected.Cast<object>().SequenceEqual(actual.Cast<object>()));
        }

        [TestMethod]
        public void return_the_same_array_when_non_empty_array_is_cropped_and_no_dimensions_are_specified()
        {
            object[,] actual = _sut.Crop(_testArray);
            Assert.IsTrue(_testArray.Cast<double>().SequenceEqual(actual.Cast<double>()));
        }

        [TestMethod]
        public void return_non_empty_array_when_array_with_missing_fields_is_cropped()
        {
            object[,] expected = new object[,] { { 2.1, 2.2, 2.3 } };

            object[,] actual = _sut.Crop(_testArrayWithMissingFields);
            Assert.IsTrue(expected.Cast<double>().SequenceEqual(actual.Cast<double>()));
        }

        [TestMethod]
        public void indicate_that_array_is_a_table_if_first_row_contains_all_string_fields()
        {
            object[,] array = new object[,] { {"Column_1", "Column_2", "Column_1" }, { 2.1, 2.2, 2.3 } };
            Assert.IsTrue(_sut.IsTable(array));
        }

        [TestMethod]
        public void indicate_that_array_is_not_a_table_if_first_row_does_not_contain_all_string_fields()
        {
            object[,] array = new object[,] { {"Column_1", 24, "Column_1" }, { 2.1, 2.2, 2.3 } };
            Assert.IsFalse(_sut.IsTable(array));
        }

        [TestMethod]
        public void indicate_that_array_is_not_a_table_if_first_row_contains_only_one_field()
        {
            object[,] array = new object[,] { {"Column_1" }, {  2.1  } };
            Assert.IsFalse(_sut.IsTable(array));
        }

        [TestMethod]
        public void return_first_row_as_next_non_empty_row_when_non_empty_array_is_passed()
        {
            int actual = _sut.NextNonEmptyRow(_testArray, 1);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void return_first_row_as_next_non_empty_row_when_array_with_empty_fields_is_passed()
        {
            int actual = _sut.NextNonEmptyRow(_testArrayWithMissingFields, 0);
            Assert.AreEqual(1, actual);
        }

        [TestMethod]
        public void check_if_passed_array_contains_tdmElement()
        {
            Assert.IsInstanceOfType(_sut.IsElement(new object[,] { { 0 } }), typeof(bool));
        }

        [TestMethod]
        public void return_range_when_element_is_simple_array()
        {
            ITdmElement element = new TdmElementItem<int> { Value = new int[,] { { 0 } } };

            
            object[,] actual = _sut.ElementToRange(element, null);

            //Assert.IsInstanceOfType(_sut.IsElement(new object[,] { { 0 } }), typeof(bool));

        }

        [TestMethod]
        public void return_range_when_element_is_simple_array11()
        {
            ITdmElement element = new TdmElementGroup
            {
                Key = "Key",
                Group = { 
                    new TdmElementItem<object> { Key = "Key_1", Value = new object[,] { { 1.1 } } },
                    new TdmElementItem<object> { Key = "Key_2", Value = new object[,] { { 2.2 }, { 2.7 }  } }
                }
            };

            object[,] actual = _sut.ElementToRange(element);

            //Assert.IsInstanceOfType(_sut.IsElement(new object[,] { { 0 } }), typeof(bool));

        }


    }
}
