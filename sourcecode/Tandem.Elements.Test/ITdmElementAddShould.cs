using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementAddShould
    {
        private ITdmElement _tdmGroup;

        [TestInitialize]
        public void ITdmElementIsEqualShouldInitialize()
        {
            _tdmGroup = new TdmElementGroup
            {
                Key = "Group",
                Group = { new TdmElementItem<double> { } }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_item()
        {
            ITdmElement tdmItem = new TdmElementItem<string> { };
            tdmItem.Add(_tdmGroup);
        }

        [TestMethod]
        public void insert_group_to_the_list_of_existing_items_in_the_group()
        {
            ITdmElement tdmSecondGroup = new TdmElementGroup
            {
                Key = "Group_test",
                Group = { new TdmElementItem<string> { } }
            };

            _tdmGroup.Add(tdmSecondGroup);
            Assert.AreSame(_tdmGroup.Group[1], tdmSecondGroup);
        }

        [TestMethod]
        public void insert_item_to_the_list_of_existing_items_in_the_group()
        {
            ITdmElement tdmItem = new TdmElementItem<string> { Key = "Key", Value = new string[,] { { "test string" } } };
            _tdmGroup.Add(tdmItem);
            Assert.AreSame(_tdmGroup.Group[1], tdmItem);
        }
    }
}
