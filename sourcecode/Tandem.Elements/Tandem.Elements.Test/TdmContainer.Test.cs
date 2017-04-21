using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements.Fakes;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmContainerTest
    {
        private TdmContainer _sut;

        public TdmContainerTest()
        {
            _sut = new TdmContainer();
        }

        [TestMethod]
        public void PropertyName_GetSetPass()
        {
            _sut.Name = "TestName";
            Assert.AreEqual("TestName", _sut.Name);
        }

        [TestMethod]
        public void PropertyCount_GetSetPass()
        {
            _sut.Count = 123;
            Assert.AreEqual((uint)123, _sut.Count);
        }

        [TestMethod]
        public void PropertyAlias_GetSetPass()
        {
            _sut.Alias = "TestAlias";
            Assert.AreEqual("TestAlias", _sut.Alias);
        }

        [TestMethod]
        public void PropertyPersist_GetSetPass()
        {
            _sut.Persist = true;
            Assert.AreEqual(true, _sut.Persist);
        }

        [TestMethod]
        public void PropertyTdmElement_GetSetPass()
        {
            var tdmElement = new TdmElement();
            _sut.TdmElement = tdmElement;
            Assert.AreSame(tdmElement, _sut.TdmElement);
        }

        [TestMethod]
        public void FullNamePass()
        {
            _sut.Name = "TestName";
            string expected = "TestName:0";
            Assert.AreEqual(expected, _sut.FullName);
        }

        [TestMethod]
        public void FullNameFail()
        {
            _sut.Name = "TestName";
            string expected = "TestName;0";
            Assert.AreNotEqual(expected, _sut.FullName);
        }

        [TestMethod]
        public void AddToPoolPass()
        {
            bool wasAddCalled = false;
            var stubContainerPool = new StubITdmContainerPool
            {
                AddITdmContainer = (x) => { wasAddCalled = true; }
            };
            var sut = new TdmContainer(stubContainerPool);
            sut.AddToPool();

            Assert.IsTrue(wasAddCalled);
        }

        [TestMethod]
        public void AliasPass()
        {
            _sut.Alias = "AliasName";
            Assert.AreEqual(_sut.Alias, _sut.Name);
        }
    }
}
