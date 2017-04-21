using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementIsEqualShould
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
        public void return_true_for_two_equal_simple_items()
        {   
            var tdmItem = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1 } } };
            Assert.IsTrue(tdmItem.IsEqual(_tdmItem_1));
        }

        [TestMethod]
        public void return_false_for_two_simple_items_with_different_keys()
        {
            var tdmItem = new TdmElementItem<double> { Key = "Key_2", Value = new double[,] { { 1.1 } } };
            Assert.IsFalse(tdmItem.IsEqual(_tdmItem_1));
        }

        [TestMethod]
        public void return_false_for_one_group_and_one_simple_item()
        {
            Assert.IsFalse(_tdmGroup.IsEqual(_tdmItem_1));
        }

        [TestMethod]
        public void return_false_for_one_simple_item_and_one_group()
        {
            Assert.IsFalse(_tdmItem_1.IsEqual(_tdmGroup));
        }

        [TestMethod]
        public void return_false_for_two_simple_items_with_different_types()
        {
            var tdmItem = new TdmElementItem<double> { Key = "Key_2", Value = new double[,] { { 1 } } };
            Assert.IsFalse(tdmItem.IsEqual(_tdmItem_2));
        }

        [TestMethod]
        public void return_true_for_two_equal_groups()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1, _tdmItem_2 } };
            Assert.IsTrue(tdmGroup.IsEqual(_tdmGroup));
        }

        [TestMethod]
        public void return_false_for_two_groups_with_unequal_keys()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group", Group = { _tdmItem_1, _tdmItem_2 } };
            Assert.IsFalse(tdmGroup.IsEqual(_tdmGroup));
        }

        [TestMethod]
        public void return_false_for_two_groups_with_different_group_elements()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1 } };
            Assert.IsFalse(tdmGroup.IsEqual(_tdmGroup));
        }

        [TestMethod]
        public void return_false_for_two_groups_with_group_elements_in_different_order()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_2, _tdmItem_1 } };
            Assert.IsFalse(tdmGroup.IsEqual(_tdmGroup));
        }

        [TestMethod]
        public void return_true_for_two_groups_with_nested_group_elements()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group_2", Group = { new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1, _tdmItem_2 } } } };
            Assert.IsTrue(tdmGroup.IsEqual(_tdmNestedGroup));
        }

        [TestMethod]
        public void return_false_for_two_groups_with_unequal_nested_group_elements()
        {
            var tdmGroup = new TdmElementGroup { Key = "Group_2", Group = { new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1 } } } };
            Assert.IsFalse(tdmGroup.IsEqual(_tdmNestedGroup));
        }

        [TestMethod]
        public void return_false_if_two_elements_are_empty()
        {
            var tdmGroup_1 = new TdmElementGroup { Key = "Group_1" };
            var tdmGroup_2 = new TdmElementGroup { Key = "Group_1" };
            Assert.IsFalse(tdmGroup_1.IsEqual(tdmGroup_2));
        }
    }
}
