using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmElementGroupShould
    {
        [TestClass]
        public class TdmElementItemShould
        {
            [TestMethod]
            public void have_public_set_and_public_get_key_property()
            {
                var sut = new TdmElementGroup { Key = "Key" };
                Assert.AreEqual("Key", sut.Key);
            }

            [TestMethod]
            public void have_public_get_value_property_and_returns_null()
            {
                var sut = new TdmElementGroup { };
                Assert.IsNull(sut.Value);
            }

            [TestMethod]
            public void have_public_get_group_property()
            {
                var sut = new TdmElementGroup { };
                Assert.IsInstanceOfType(sut.Group, typeof(List<ITdmElement>));
            }

            [TestMethod]
            public void have_public_get_is_empty_property_and_returns_true_when_group_is_empty()
            {
                var sut = new TdmElementGroup { };
                Assert.IsTrue(sut.IsEmpty);
            }

            [TestMethod]
            public void have_public_get_is_empty_property_and_returns_false_when_group_is_not_empty()
            {
                var sut = new TdmElementGroup { Group = { new TdmElementGroup { } } };
                Assert.IsFalse(sut.IsEmpty);
            }

            [TestMethod]
            public void have_public_get_has_children_property_and_returns_false_when_group_is_empty()
            {
                var sut = new TdmElementGroup { };
                Assert.IsFalse(sut.HasChildren);
            }

            [TestMethod]
            public void have_public_get_has_children_property_and_returns_true_when_group_is_not_empty()
            {
                var sut = new TdmElementGroup { Group = { new TdmElementGroup { } } };
                Assert.IsTrue(sut.HasChildren);
            }
        }
    }
}

