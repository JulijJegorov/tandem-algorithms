using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementMapShould
    {
        private ITdmElement _tdmItem_1;
        private ITdmElement _tdmItem_2;
        private ITdmElement _tdmGroup;
        private ITdmElement _tdmNestedGroup;

        [TestInitialize]
        public void ITdmElementIsEqualShouldInitialize()
        {
            _tdmItem_1 = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1 } } };
            _tdmItem_2 = new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { { "1" } } };

            _tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1, _tdmItem_2 } };
            _tdmNestedGroup = new TdmElementGroup { Key = "Group_2", Group = { _tdmGroup } };
        }

        [TestMethod]
        public void copy_value_of_second_item_to_first_item_when_keys_are_equal()
        {
            _tdmItem_1.Map(new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 3.3 } } });
            Assert.AreEqual(((double[,])_tdmItem_1.Value)[0, 0], 3.3);
        }

        [TestMethod]
        public void not_copy_value_of_second_item_to_first_item_when_keys_are_not_equal()
        {
            _tdmItem_1.Map(new TdmElementItem<double> { Key = "Key_2", Value = new double[,] { { 2.2 } } });
            Assert.AreNotEqual(((double[,])_tdmItem_1.Value)[0, 0], 2.2);
        }

        [TestMethod]
        public void not_copy_value_from_item_to_group()
        {
            _tdmGroup.Map(new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 3.3 } } });
            Assert.AreEqual(_tdmGroup.Group[0].Value, _tdmItem_1.Value);
            Assert.AreEqual(_tdmGroup.Group[1].Value, _tdmItem_2.Value);
        }

        [TestMethod]
        public void not_copy_value_of_second_group_to_first_group_when_keys_are_not_equal()
        {
            _tdmItem_1.Map(new TdmElementGroup { Key = "Group_false", Group = { _tdmItem_1, _tdmItem_2 } });
            Assert.AreEqual(_tdmGroup.Group[0].Value, _tdmItem_1.Value);
            Assert.AreEqual(_tdmGroup.Group[1].Value, _tdmItem_2.Value);
        }

        [TestMethod]
        public void copy_value_from_second_group_to_first_item_in_first_group_when_keys_are_equal()
        {

            var tdmSecondGroup = new TdmElementGroup
            {
                Key = "Group_1",
                Group = {
                    new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 3.3 } } } 
                }
            };

            _tdmGroup.Map(tdmSecondGroup);
            Assert.AreEqual(((double[,])_tdmGroup.Group[0].Value)[0, 0], 3.3);
        }

        [TestMethod]
        public void copy_value_from_second_group_to_second_item_in_first_group_when_keys_are_equal()
        {
            var tdmSecondGroup = new TdmElementGroup
            {
                Key = "Group_1",
                Group = {
                    new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { {"test string" } } } 
                }
            };

            _tdmGroup.Map(tdmSecondGroup);
            Assert.AreEqual(((string[,])_tdmGroup.Group[1].Value)[0, 0], "test string");
        }

        [TestMethod]
        public void copy_value_from_second_nested_group_to_second_item_in_first_nested_group_when_keys_are_equal()
        {
            var tdmChildGroup = new TdmElementGroup
            {
                Key = "Group_1",
                Group = {
                    new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { { "test string" } } }
                }
            };

            _tdmNestedGroup.Map(new TdmElementGroup { Key = "Group_2", Group = { tdmChildGroup } });
            Assert.AreEqual(((string[,])_tdmNestedGroup.Group[0].Group[1].Value)[0, 0], "test string");
        }
    }
}
