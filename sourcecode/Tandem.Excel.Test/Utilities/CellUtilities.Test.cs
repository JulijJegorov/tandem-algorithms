using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Excel.Utilities;
using ExcelDna.Integration;

namespace Tandem.Excel.Test.Utilities
{
    [TestClass]
    public class CellUtilitiesShould
    {
        private CellUtilities _sut;

        [TestInitialize]
        public void CellUtilitiesTestInitialize()
        {
            _sut = new CellUtilities();
        }

        [TestMethod]
        public void not_be_empty_if_cell_contains_value_of_type_object()
        {
            object cell = new object[,] { { 1.1 } };
            Assert.IsFalse(_sut.IsEmpty(cell));
        }

        [TestMethod]
        public void be_empty_if_cell_contains_placeholder()
        {
            object cell = _sut.Placeholder;
            Assert.IsTrue(_sut.IsEmpty(cell));
        }

        [TestMethod]
        public void be_empty_if_cell_contains_value_of_type_excelempty()
        {
            object cell = ExcelEmpty.Value;
            Assert.IsTrue(_sut.IsEmpty(cell));
        }

        [TestMethod]
        public void be_empty_if_cell_contains_value_of_type_excelmissing()
        {
            object cell = ExcelMissing.Value;
            Assert.IsTrue(_sut.IsEmpty(cell));
        }

        [TestMethod]
        public void be_empty_if_cell_contains_value_of_type_excelerror()
        {
            object cell = ExcelError.ExcelErrorNA;
            Assert.IsTrue(_sut.IsEmpty(cell));
        }

        [TestMethod]
        public void check_if_string_is_a_json_string_and_return_boolean_if_true()
        {
            object cell = "{\"Key\":\"Key\",\"Value\":[[1.1,1.2],[2.1,2.2]]}";
            Assert.IsTrue(_sut.IsJson(cell));
        }

        [TestMethod]
        public void check_if_string_is_a_json_string_and_return_boolean_if_false()
        {
            object cell = "not_json";
            Assert.IsFalse(_sut.IsJson(cell));
        }

        [TestMethod]
        public void convert_cell_value_to_string_when_asked_for_name_and_cell_is_not_empty()
        {
            object cell = "name";
            Assert.AreEqual(typeof(string), _sut.GetName(cell).GetType());
        }

        [TestMethod]
        public void split_name_string_from_iterator()
        {
            object cell = "name:0";
            Assert.AreEqual("name", _sut.SplitName(cell));
        }
    }
}
