using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementMergeShould
    {
        private ITdmElement _tdmItem_1;
        private ITdmElement _tdmItem_2;
        private ITdmElement _tdmGroup;

        [TestInitialize]
        public void ITdmElementMergeShouldInitialize()
        {
            _tdmItem_1 = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1 } } };
            _tdmItem_2 = new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { { "1" } } };
            _tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1, _tdmItem_2 } };
        }

        [TestMethod]
        public void add_two_item_to_the_group_when_item_is_merged_with_another_item()
        {
            var tdmGroup = _tdmItem_1.Merge(_tdmItem_2);
            Assert.AreSame(tdmGroup.Group[0], _tdmItem_1);
            Assert.AreSame(tdmGroup.Group[1], _tdmItem_2);
        }

        [TestMethod]
        public void add_item_to_the_group_when_item_is_merged_with_group()
        {
            var tdmItem = new TdmElementItem<int> { Key = "Key_test", Value = new int[,] { { 25 } } };
            var tdmGroup = tdmItem.Merge(_tdmGroup);
            Assert.AreSame(tdmGroup.Group[2], tdmItem);
        }

        [TestMethod]
        public void add_item_to_the_group_when_group_is_merged_with_item()
        {
            var tdmItem = new TdmElementItem<int> { Key = "Key_test", Value = new int[,] { { 25 } } };
            var tdmGroup = _tdmGroup.Merge(tdmItem);
            Assert.AreSame(tdmGroup.Group[2], tdmItem);
        }

        [TestMethod]
        public void add_item_to_the_first_group_when_group_is_merged_with_another_group()
        {
          
            var tdmGroup = new TdmElementGroup
            {
                Key = "Group_test",
                Group = {
                    new TdmElementItem<string> { Key = "Key_test", Value = new string[,] { { "test string" } } }
                }
            };

            var sut = tdmGroup.Merge(_tdmGroup);

            Assert.AreEqual(sut.Key, "Group_test");
            Assert.AreSame(sut.Group[1], _tdmItem_1);
            Assert.AreSame(sut.Group[2], _tdmItem_2);
        }

    }
}
