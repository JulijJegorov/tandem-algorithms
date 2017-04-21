using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void IsDictionaryTypePass()
        {
            var sut = TestHelper.TdmDictionary();
            Assert.AreEqual(TdmElementType.Dictionary, sut.ElementType());
        }

        [TestMethod]
        public void IsArrayTypWhen1DPass()
        {
            var sut = TestHelper.Tdm1DArray();
            Assert.AreEqual(TdmElementType.Array, sut.ElementType());
        }

        [TestMethod]
        public void IsArrayTypWhen2DPass()
        {
            var sut = TestHelper.Tdm2DArray();
            Assert.AreEqual(TdmElementType.Array, sut.ElementType());
        }
        [TestMethod]
        public void IsArrayTypWhenValueFail()
        {
            var sut = TestHelper.TdmSingleValue();
            Assert.AreEqual(TdmElementType.Array, sut.ElementType());
        }

        [TestMethod]
        public void IsTableTypePass()
        {
            var sut = TestHelper.TdmTable();
            Assert.AreEqual(TdmElementType.Table, sut.ElementType());
        }

        [TestMethod]
        public void IsTableWhenDictionaryFail()
        {
            var sut = TestHelper.TdmDictionary();
            Assert.IsFalse(sut.IsTable());
        }

        [TestMethod]
        public void IsTableWhenArrayFail()
        {
            var sut = TestHelper.Tdm1DArray();
            Assert.IsFalse(sut.IsTable());
        }

        [TestMethod]
        public void IsDictionaryWhenTableFail()
        {
            var sut = TestHelper.TdmTable();
            Assert.IsFalse(sut.IsDictionary());
        }

        [TestMethod]
        public void IsDictionaryWhenArrayFail()
        {
            var sut = TestHelper.Tdm2DArray();
            Assert.IsFalse(sut.IsDictionary());
        }

        [TestMethod]
        public void IsEmptyValueEmptyAndListEmptyPass()
        {
            var sut = TestHelper.TdmValueAndListEmpty();
            Assert.IsTrue(sut.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyValueNotEmptyAndListEmptyFail()
        {
            var sut = TestHelper.TdmListEmpty();
            Assert.IsFalse(sut.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyValueEmptyAndLisNotEmptyFail()
        {
            var sut = TestHelper.TdmValueEmpty();
            Assert.IsFalse(sut.IsEmpty());
        }

        [TestMethod]
        public void IsEmptyFail()
        {
            var sut = TestHelper.Tdm1DArray();
            Assert.IsFalse(sut.IsEmpty());
        }

        [TestMethod]
        public void HasChildrenWhenDictionaryPass()
        {
            var sut = TestHelper.TdmDictionary();
            Assert.IsTrue(sut.HasChildren());
        }

        [TestMethod]
        public void HasChildrenWhenTablePass()
        {
            var sut = TestHelper.TdmTable();
            Assert.IsTrue(sut.HasChildren());
        }
        [TestMethod]
        public void HasChildrenWhenArrayFail()
        {
            var sut = TestHelper.Tdm2DArray();
            Assert.IsFalse(sut.HasChildren());
        }
    }
}
