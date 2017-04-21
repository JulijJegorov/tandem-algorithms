using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementLookupShould
    {
        private ITdmElement _tdmItem_1;
        private ITdmElement _tdmItem_2;
      
        private ITdmElement _tdmGroup;
        private ITdmElement _tdmNestedGroup;

        [TestInitialize]
        public void ITdmElementLookupShouldInitialize()
        {
            _tdmItem_1 = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1 } } };
            _tdmItem_2 = new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { { "1" } } };

            _tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem_1, _tdmItem_2 } };
            _tdmNestedGroup = new TdmElementGroup { Key = "Group_2", Group = { _tdmGroup } };
        }

        [TestMethod]
        public void return_value_from_item_when_key_exists()
        {
            var tdmItem = _tdmItem_1.Lookup(new List<string> { "Key_1" });
            Assert.AreEqual(tdmItem.Key, _tdmItem_1.Key);
            Assert.AreEqual(tdmItem.Value, _tdmItem_1.Value);
        }

        [TestMethod]
        public void not_return_value_from_item_when_key_does_not_exist()
        {
            var tdmItem = _tdmItem_1.Lookup(new List<string> { "Key_false" });
            Assert.IsNull(tdmItem);
        }

        [TestMethod]
        public void return_value_from_group_when_first_key_exists()
        {
            var tdmItem = _tdmGroup.Lookup(new List<string> { "Key_1" });
            Assert.AreEqual(tdmItem.Key, _tdmItem_1.Key);
            Assert.AreEqual(tdmItem.Value, _tdmItem_1.Value);
        }

        [TestMethod]
        public void return_value_from_group_when_second_key_exists()
        {
            var tdmItem = _tdmGroup.Lookup(new List<string> { "Key_2" });
            Assert.AreEqual(tdmItem.Key, _tdmItem_2.Key);
            Assert.AreEqual(tdmItem.Value, _tdmItem_2.Value);
        }

        [TestMethod]
        public void not_return_value_from_group_when_key_does_not_exist()
        {
            var tdmItem = _tdmGroup.Lookup(new List<string> { "Key_false" });
            Assert.IsNull(tdmItem);
        }

        [TestMethod]
        public void return_value_from_nested_group_when_first_key_exists()
        {
            var tdmItem = _tdmNestedGroup.Lookup(new List<string> { "Group_1", "Key_1" });
            Assert.AreEqual(tdmItem.Key, _tdmItem_1.Key);
            Assert.AreEqual(tdmItem.Value, _tdmItem_1.Value);
        }

        [TestMethod]
        public void return_value_from_nested_group_when_second_key_exists()
        {
            var tdmItem = _tdmNestedGroup.Lookup(new List<string> { "Group_1", "Key_2" });
            Assert.AreEqual(tdmItem.Key, _tdmItem_2.Key);
            Assert.AreEqual(tdmItem.Value, _tdmItem_2.Value);
        }

        [TestMethod]
        public void return_this_element_if_key_list_is_empty()
        {
            var tdmItem = _tdmGroup.Lookup(new List<string> { });
            Assert.AreEqual(tdmItem.Key, _tdmGroup.Key);
            Assert.AreEqual(tdmItem.Value, _tdmGroup.Value);
            Assert.AreEqual(tdmItem.Group[0].Key, tdmItem.Group[0].Key);
            Assert.AreEqual(tdmItem.Group[0].Value, tdmItem.Group[0].Value);
            Assert.AreEqual(tdmItem.Group[1].Key, tdmItem.Group[1].Key);
            Assert.AreEqual(tdmItem.Group[1].Value, tdmItem.Group[1].Value);
        }
    }
}
