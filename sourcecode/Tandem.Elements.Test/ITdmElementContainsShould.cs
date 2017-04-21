using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementContainsShould
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
        public void return_true_when_key_exists()
        {
            bool isTrue = _tdmItem_1.Contains(new List<string> { "Key_1" });
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void return_false_when_key_does_not_exist()
        {
            bool isTrue = _tdmItem_1.Contains(new List<string> { "Key_false" });
            Assert.IsFalse(isTrue);
        }

        [TestMethod]
        public void return_true_when_first_key_exists_in_nested_group()
        {
            bool isTrue = _tdmNestedGroup.Contains(new List<string> { "Group_1", "Key_1" });
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void return_true_when_second_key_exists_in_nested_group()
        {
            bool isTrue = _tdmNestedGroup.Contains(new List<string> { "Group_1", "Key_2" });
            Assert.IsTrue(isTrue);
        }

        [TestMethod]
        public void return_false_when_first_key_does_not_exist_in_nested_group()
        {
            bool isTrue = _tdmNestedGroup.Contains(new List<string> { "Group_false", "Key_1" });
            Assert.IsFalse(isTrue);
        }
    
        [TestMethod]
        public void return_false_when_second_key_does_not_exist_in_nested_group()
        {
            bool isTrue = _tdmNestedGroup.Contains(new List<string> { "Group_1", "Key_false" });
            Assert.IsFalse(isTrue);
        }


    }
}
