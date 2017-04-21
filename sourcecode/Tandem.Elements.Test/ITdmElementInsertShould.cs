using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementInsertShould
    {
        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_item()
        {
            ITdmElement tdmItem = new TdmElementItem<string> { };
            tdmItem.Insert("Key", new int[,] { { 1 } });
        }

        [TestMethod]
        public void insert_item_to_the_list_of_existing_items_in_the_group()
        {
            ITdmElement tdmGroup = new TdmElementGroup
            {
                Key = "Group_test",
                Group = { new TdmElementItem<string> { } }
            };

            tdmGroup.Insert("Key", new int[,] { { 1 } });
            Assert.AreEqual(tdmGroup.Group[1].Key, "Key");
            Assert.AreEqual(((int[,])tdmGroup.Group[1].Value)[0,0], 1);
        }
    }
}
