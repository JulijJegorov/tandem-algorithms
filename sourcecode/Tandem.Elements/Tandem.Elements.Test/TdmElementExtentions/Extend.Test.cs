using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void AddSimpleElementToElementSimpleElementPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmSingleValue();

            sut.Add(tdmElement);
            Assert.AreEqual(sut.TdmElementList[0].Key, tdmElement.Key);
            Assert.AreEqual(sut.TdmElementList[0].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void AddSimpleElementToDictionaryPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmDictionary();
            int count = sut.TdmElementList.Count;

            sut.Add(tdmElement);
            Assert.AreEqual(sut.TdmElementList[count].Key, tdmElement.Key);
            Assert.AreEqual(sut.TdmElementList[count].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void AppendSimpleElementToElementSimpleElementPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmSingleValue();

            sut.Append(tdmElement.Key, tdmElement.Value);
            Assert.AreEqual(sut.TdmElementList[0].Key, tdmElement.Key);
            Assert.AreEqual(sut.TdmElementList[0].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void AppendSimpleElementToDictionaryPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmDictionary();
            int count = sut.TdmElementList.Count;

            sut.Append(tdmElement.Key, tdmElement.Value);
            Assert.AreEqual(sut.TdmElementList[count].Key, tdmElement.Key);
            Assert.AreEqual(sut.TdmElementList[count].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void InsertSimpleElementToElementSimpleElementPass()
        {
            var sut = new TdmElement();
            var tdmElement = TestHelper.TdmSingleValue();
            
            sut.Insert(tdmElement.Key, tdmElement.Value);
            Assert.AreEqual(sut.Key, tdmElement.Key);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }
    }
}


