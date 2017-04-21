using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class MapTest
    {
        [TestMethod]
        public void MapSimpleElementToSimpleElementPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmSingleValue();
            sut.Value = null;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void MapSimpleElementToSimpleElementWhenKeyIsUpperCasePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmSingleValue();
            sut.Key = tdmElement.Key.ToUpper();
            sut.Value = null;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void MapSimpleElementToSimpleElementWhenKeyIsLowerCasePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmSingleValue();
            sut.Key = tdmElement.Key.ToLower();
            sut.Value = null;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.Key, tdmElement.Key.ToLower());
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void MapDictionaryElementToSimpleElementPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmDictionary();
            sut.TdmElementList[1].Key = tdmElement.Key;
            sut.TdmElementList[1].Value = tdmElement.Value;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.TdmElementList[1].Key, tdmElement.Key);
            Assert.AreEqual(sut.TdmElementList[1].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void MapDictionaryElementToSimpleElementWhenKeyIsUpperCasePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmDictionary();
            sut.TdmElementList[1].Key = tdmElement.Key.ToUpper();
            sut.TdmElementList[1].Value = tdmElement.Value;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.TdmElementList[1].Key, tdmElement.Key.ToUpper());
            Assert.AreEqual(sut.TdmElementList[1].Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void MapDictionaryElementToSimpleElementWhenKeyIsLowerCasePassPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = TestHelper.TdmDictionary();
            sut.TdmElementList[1].Key = tdmElement.Key.ToLower();
            sut.TdmElementList[1].Value = tdmElement.Value;

            sut.Map(tdmElement);
            Assert.AreEqual(sut.TdmElementList[1].Key, tdmElement.Key.ToLower());
            Assert.AreEqual(sut.TdmElementList[1].Value[0, 0], tdmElement.Value[0, 0]);
        }
    }
}






































//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace Tandem.Elements.Test
//{
//    [TestClass]
//    public class TdmElementMapTest
//    {
//        private TdmElement _sut;
//        private TdmElement _sutEmpty;

//        public TdmElementMapTest()
//        {
//            object[,] value = {
//                               {1.1},
//                              };
//            object[,] valueEmpty = {
//                                    {0.0}
//                                    };

//            var sut_1_1 = new TdmElement { Key = "Key_1_1", Value = value };
//            var sut_1 = new TdmElement();
//            sut_1.Key = "Key_1";
//            sut_1.TdmElementList.Add(sut_1_1);

//            _sut = new TdmElement();
//            _sut.TdmElementList.Add(sut_1_1);

//            var sutEmpty_1_1 = new TdmElement { Key = "Key_1_1", Value = valueEmpty };
//            var sutEmpty_1 = new TdmElement();
//            sutEmpty_1.Key = "Key_1";

//            _sutEmpty = new TdmElement();
//            _sutEmpty.TdmElementList.Add(sutEmpty_1_1);
//        }


//        [TestMethod]
//        public void MapAreEqual()
//        {
//            _sutEmpty.Map(_sut);

//            string expected = _sutEmpty.SerializeToJSON();
//            string actual = _sutEmpty.SerializeToJSON();

//            Assert.AreEqual(expected, actual);
//        }
//    }
//}
