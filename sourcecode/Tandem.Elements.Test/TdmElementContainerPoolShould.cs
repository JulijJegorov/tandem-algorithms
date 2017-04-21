using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements;
using System.Collections.Specialized;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class TdmElementContainerPoolShould
    {
        [TestMethod]
        public void initialize_initialize_only_once()
        {
            var sut_1 = TdmContainerPool.Instance;
            sut_1.Add(new TdmElementContainer() { Name = "TestName" });
            var sut_2 = TdmContainerPool.Instance;

            Assert.IsTrue(sut_1.Count == sut_2.Count);
        }

        [TestMethod]
        public void have_public_get_pool_table_property()
        {
            var sut = TdmContainerPool.Instance;
            Assert.IsInstanceOfType(sut.PoolTable, typeof(Dictionary<string, TdmElementContainer>));
        }

        [TestMethod]
        public void have_two_elements_when_two_containers_with_different_names_are_added()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer(){Name= "TestName_1"});
            sut.Add(new TdmElementContainer() { Name = "TestName_2" });
            Assert.AreEqual(2, sut.Count);
        }

        [TestMethod]
        public void have_one_element_when_two_containers_with_the_same_name_are_added()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.AreEqual(1, sut.Count);
        }

        [TestMethod]
        public void return_true_if_the_pool_contains_a_continer_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.IsTrue(sut.Contains("TestName_1"));
        }

        [TestMethod]
        public void return_false_if_the_pool_does_not_contain_a_continer_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.IsFalse(sut.Contains("TestName_2"));
        }

        [TestMethod]
        public void return_false_if_the_pool_is_empty_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            Assert.IsFalse(sut.Contains("TestName"));
        }

        [TestMethod]
        public void return_true_if_the_pool_contains_a_continer_and_container_object_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            var tdmContainer = new TdmElementContainer() { Name = "TestName_1" };
            sut.Add(tdmContainer);
            Assert.IsTrue(sut.Contains(tdmContainer));
        }

        [TestMethod]
        public void return_false_if_the_pool_does_not_contain_a_continer_and_container_object_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            var tdmContainer = new TdmElementContainer() { Name = "TestName_1" };
            sut.Add(tdmContainer);
            Assert.IsFalse(sut.Contains(new TdmElementContainer() { Name = "TestName_2" }));
        }

        [TestMethod]
        public void return_false_if_the_pool_is_empty_and_container_object_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            Assert.IsFalse(sut.Contains(new TdmElementContainer() { Name = "TestName" }));
        }

        [TestMethod]
        public void remove_container_if_container_is_in_the_pool_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.AreEqual(1, sut.Count);
            sut.Remove("TestName_1");
            Assert.AreEqual(0, sut.Count);
        }

        [TestMethod]
        public void add_container_to_the_pool_when_container_with_unique_name_is_added()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.AreEqual(1, sut.Count);
            sut.Add(new TdmElementContainer() { Name = "TestName_2" });
            Assert.AreEqual(2, sut.Count);
        }

        [TestMethod]
        public void replace_container_in_the_pool_when_container_with_the_same_name_is_added()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.AreEqual(1, sut.Count);
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            Assert.AreEqual(1, sut.Count);
        }

        [TestMethod]
        public void return_container_if_the_pool_contains_a_continer_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            var tdmContainer = sut.Get("TestName_1");
            Assert.IsInstanceOfType(tdmContainer, typeof(TdmElementContainer));
        }

        [TestMethod]
        public void return_null_if_the_pool_does_not_contain_a_continer_and_container_name_is_provided()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName_1" });
            var tdmContainer = sut.Get("TestName");
            Assert.IsNull(tdmContainer);
        }

        [TestMethod]
        public void throw_add_notification_when_new_container_is_added()
        {
            var sut = TdmContainerPool.Instance;
            var collectionAction = new NotifyCollectionChangedAction();
            sut.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                collectionAction = e.Action;
            };
            sut.Add(new TdmElementContainer() { Name = "TestName" });
            Assert.AreEqual(NotifyCollectionChangedAction.Add, collectionAction);
        }

        [TestMethod]
        public void throw_remove_notification_when_container_is_removed()
        {
            var sut = TdmContainerPool.Instance;
            sut.Add(new TdmElementContainer() { Name = "TestName" });

            var collectionAction = new NotifyCollectionChangedAction();
            sut.CollectionChanged += delegate(object sender, NotifyCollectionChangedEventArgs e)
            {
                collectionAction = e.Action;
            };
            sut.Remove("TestName");
            Assert.AreEqual(NotifyCollectionChangedAction.Remove, collectionAction);
        }

        [TestCleanup]
        public void delete_all_containers_from_the_pool_after_every_test()
        {
            TdmContainerPool.Instance.RemoveAll();
        }
    }
}
