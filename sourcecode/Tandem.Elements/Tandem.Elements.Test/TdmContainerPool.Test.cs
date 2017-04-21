using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Specialized;
using Tandem.Elements.Fakes;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmContainerPoolTest
    {
        StubITdmContainer _tdmContainer_1;
        StubITdmContainer _tdmContainer_2;
        TdmContainerPool _tdmPoolTable;

        public TdmContainerPoolTest()
        {
            _tdmContainer_1 = new StubITdmContainer()
            {
                NameGet = () => { return "tdmContainer_1"; },
            };

            _tdmContainer_2 = new StubITdmContainer()
            {
                NameGet = () => { return "tdmContainer_2"; }
            };

            _tdmPoolTable = TdmContainerPool.Instance;
        }

        [TestMethod]
        public void AddToContainerPoolPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            Assert.IsTrue(_tdmPoolTable.Contains("tdmContainer_1"));
        }

        [TestMethod]
        public void AddTwoSameItemsToContainerPoolPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_1);
            Assert.IsTrue(_tdmPoolTable.Contains("tdmContainer_1"));
        }

        [TestMethod]
        public void AddTwoDifferentItemsToContainerPoolPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_2);
            Assert.IsTrue(_tdmPoolTable.Contains("tdmContainer_1"));
            Assert.IsTrue(_tdmPoolTable.Contains("tdmContainer_2"));
        }

        [TestMethod]
        public void ContainsNotEmptyByElementPass()
        {
            _tdmPoolTable.Add(_tdmContainer_2);
            bool contains = _tdmPoolTable.Contains(_tdmContainer_2);
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void ContainsNotEmptyByElementFail()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            bool contains = _tdmPoolTable.Contains(_tdmContainer_2);
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void ContainsNoneEmptyByElementPass()
        {
            bool contains = _tdmPoolTable.Contains(_tdmContainer_2);
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void ContainsNotEmptyByStringPass()
        {
            _tdmPoolTable.Add(_tdmContainer_2);
            bool contains = _tdmPoolTable.Contains("tdmContainer_2");
            Assert.IsTrue(contains);
        }

        [TestMethod]
        public void ContainsNotEmptyByStringFail()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            bool contains = _tdmPoolTable.Contains("tdmContainer_2");
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void ContainsNoneEmptyByStringPass()
        {
            bool contains = _tdmPoolTable.Contains("tdmContainer_2");
            Assert.IsFalse(contains);
        }

        [TestMethod]
        public void RemoveItemByStringPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_2);
            _tdmPoolTable.Remove("tdmContainer_1");
            int count = _tdmPoolTable.Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void RemoveItemByElementPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_2);
            _tdmPoolTable.Remove(_tdmContainer_1);
            int count = _tdmPoolTable.Count;
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void NotifyAddItemPass()
        {
            var CollectionAction = new NotifyCollectionChangedAction();
            _tdmPoolTable.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionAction = e.Action;
            };
            _tdmPoolTable.Add(_tdmContainer_1);
            Assert.AreEqual(NotifyCollectionChangedAction.Add, CollectionAction);
        }

        [TestMethod]
        public void NotifyAddItemWhenRemovedFail()
        {
            var CollectionAction = new NotifyCollectionChangedAction();
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionAction = e.Action;
            };
            _tdmPoolTable.Remove(_tdmContainer_1);
            Assert.AreNotEqual(NotifyCollectionChangedAction.Add, CollectionAction);
        }

        [TestMethod]
        public void NotifyRemoveItemPass()
        {
            var CollectionAction = new NotifyCollectionChangedAction();
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionAction = e.Action;
            };
            _tdmPoolTable.Remove(_tdmContainer_1);
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, CollectionAction);
        }

        [TestMethod]
        public void NotifyRemoveItemWhenAddFail()
        {
            var CollectionAction = new NotifyCollectionChangedAction();
            
            _tdmPoolTable.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                CollectionAction = e.Action;
            };
            _tdmPoolTable.Add(_tdmContainer_1);
            Assert.AreNotEqual(NotifyCollectionChangedAction.Remove, CollectionAction);
        }

        [TestMethod]
        public void AccessExistingInstanceInThePool()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_2);

            var tdmPoolInstance = TdmContainerPool.Instance;
            int count = tdmPoolInstance.Count;
            Assert.AreEqual(2, count);
        }


        [TestMethod]
        public void GetEnumeratorPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            _tdmPoolTable.Add(_tdmContainer_2);

            int count = 0;
            foreach (var item in _tdmPoolTable)
            {
                count++;
            }

            Assert.AreEqual(2, _tdmPoolTable.Count);
        }

        [TestMethod]
        public void GetByStringPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            ITdmContainer tdmContainer = _tdmPoolTable.Get("tdmContainer_1");
            Assert.AreEqual("tdmContainer_1", tdmContainer.Name);
        }

        [TestMethod]
        public void GetByStringFail()
        {
            ITdmContainer tdmContainer = _tdmPoolTable.Get("tdmContainer_1");
            Assert.IsNull(tdmContainer);
        }

        [TestMethod]
        public void PropertyCount_GetPass()
        {
            _tdmPoolTable.Add(_tdmContainer_1);
            Assert.AreEqual(1, _tdmPoolTable.Count);
        }

        [TestCleanup]
        public void CleanUpPoolInstanceAfterEachTest()
        {
            _tdmPoolTable.RemoveAll();
        }
    }
}
