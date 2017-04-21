using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmElementItemShould
    {
        [TestMethod]
        public void have_public_set_and_public_get_key_property()
        {
            var sut = new TdmElementItem<int> { Key = "Key" };
            Assert.AreEqual("Key", sut.Key);
        }

        [TestMethod]
        public void have_public_set_and_public_get_value_property()
        {
            var sut = new TdmElementItem<int> { Value = new int[,] {{ 1 }}};
            Assert.AreEqual(1, ((int[,]) sut.Value)[0,0]);
        }

        [TestMethod]
        public void have_public_get_group_property()
        {
            var sut = new TdmElementItem<int> { };
            Assert.IsInstanceOfType(sut.Group, typeof(List<ITdmElement>));
        }

        [TestMethod]
        public void have_public_get_is_empty_property_and_returns_true_if_value_is_null()
        {
            var sut = new TdmElementItem<int> { };
            Assert.IsTrue(sut.IsEmpty);
        }

        [TestMethod]
        public void have_public_get_has_children_property_and_returns_false()
        {
            var sut = new TdmElementItem<int> { };
            Assert.IsFalse(sut.HasChildren);
        }
    }
}
