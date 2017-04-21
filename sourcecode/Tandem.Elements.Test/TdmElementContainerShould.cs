using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements;
using Tandem.Elements.Fakes;
using Microsoft.QualityTools.Testing.Fakes;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmElementContainerShould
    {
        [TestMethod]
        public void have_public_set_and_public_get_name_property()
        {
            var sut = new TdmElementContainer { Name = "TestName" };
            Assert.AreEqual("TestName", sut.Name);
        }

        [TestMethod]
        public void return_alias_if_available_through_public_get_name_property()
        {
            var sut = new TdmElementContainer { Alias = "TestAlias" };
            Assert.AreEqual("TestAlias", sut.Name);
        }

        [TestMethod]
        public void have_public_set_and_public_get_count_property()
        {
            var sut = new TdmElementContainer { Count = 123 };
            Assert.AreEqual((uint)123, sut.Count);
        }

        [TestMethod]
        public void have_public_set_and_public_get_alais_property()
        {
            var sut = new TdmElementContainer { Alias = "TestAlias" };
            Assert.AreEqual("TestAlias", sut.Alias);
        }

        [TestMethod]
        public void have_public_set_and_public_get_persist_property()
        {
            var sut = new TdmElementContainer { Persist = true };
            Assert.AreEqual(true, sut.Persist);
        }

        [TestMethod]
        public void have_public_set_and_public_get_element_property()
        {
            ITdmElement tdmElement = new TdmElementGroup();
            var sut = new TdmElementContainer { Element = tdmElement };
            Assert.AreSame(tdmElement, sut.Element);
        }


        [TestMethod]
        public void return_full_name_with_iterator_when_name_is_assigned()
        {
            ITdmElement tdmElement = new TdmElementGroup();
            var sut = new TdmElementContainer { Name = "TestName" };
            string expected = "TestName:0";

            Assert.AreEqual(expected, sut.FullName);
        }

        [TestMethod]
        public void add_itself_to_container_pool()
        {
            bool wasAddCalled = false;
            using (ShimsContext.Create())
            {
                ShimTdmContainerPool.AllInstances.AddTdmElementContainer = (pool, element) => { wasAddCalled = true; };
                var sut = new TdmElementContainer();
                sut.AddToPool();
            }
            Assert.IsTrue(wasAddCalled);
        }

    }
}
