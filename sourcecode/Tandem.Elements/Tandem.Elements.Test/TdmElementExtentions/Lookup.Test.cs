using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class LookupTest
    {
        [TestMethod]
        public void LookupSimpleElmentPass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmSingleValue());

            string expectedKey = tdmParent.TdmElementList[0].Key;
 
            var sut = tdmParent.Lookup(expectedKey);
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupSimpleElmentFail()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmSingleValue());

            string wrongKey = tdmParent.TdmElementList[0].Key + "abc";

            var sut = tdmParent.Lookup(wrongKey);
            Assert.IsNull(sut);
        }

        [TestMethod]
        public void LookupSimpleElmentWhenKeyIsUpperCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmSingleValue());

            string expectedKey = tdmParent.TdmElementList[0].Key;

            var sut = tdmParent.Lookup(expectedKey.ToUpper());
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupSimpleElmentWhenKeyIsLowerCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmSingleValue());

            string expectedKey = tdmParent.TdmElementList[0].Key;

            var sut = tdmParent.Lookup(expectedKey.ToLower());
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupNestedOneLevelElmentPass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTable());

            var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[1];
            string keyHeadElement = tdmParent.TdmElementList[0].Key;
            string expectedKey = tdmLevelOneElement.Key;

            var sut = tdmParent.Lookup(keyHeadElement + '/' + expectedKey);
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupNestedOneLevelElmentWrongHeadKeyFail()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTable());

            var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[1];
            string keyHeadElement = tdmParent.TdmElementList[0].Key + "abc";

            string expectedKey = tdmLevelOneElement.Key;
            var expectedValue = tdmLevelOneElement.Value;

            var sut = tdmParent.Lookup(keyHeadElement + '/' + expectedKey);
            Assert.IsNull(sut);
        }

        [TestMethod]
        public void LookupNestedOneLevelElmentWrongLevelOneKeyFail()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTable());

            var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[1];
            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            string expectedKey = tdmLevelOneElement.Key + "abc";

            var sut = tdmParent.Lookup(keyHeadElement + '/' + expectedKey);
            Assert.IsNull(sut);
        }

        [TestMethod]
        public void LookupNestedOneLevelElmentWhenKeyIsUpperCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTable());

            var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[1];
            string keyHeadElement = tdmParent.TdmElementList[0].Key;
            string expectedKey = tdmLevelOneElement.Key;

            var sut = tdmParent.Lookup((string)(keyHeadElement + '/' + expectedKey).ToUpper());
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupNestedOneLevelElmentWhenKeyIsLowerCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTable());

            var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[1];
            string keyHeadElement = tdmParent.TdmElementList[0].Key;
            string expectedKey = tdmLevelOneElement.Key;

            var sut = tdmParent.Lookup((string)(keyHeadElement + '/' + expectedKey).ToLower());
            Assert.AreEqual(expectedKey, sut.Key);
        }

        [TestMethod]
        public void LookupNestedTwoLevelsElmentPass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTwoLevelsNestedTypeOne());

            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne = tdmLevelOneElement.Key;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string expectedKey = tdmLevelTwoElement.Key;

                    var sut = tdmParent.Lookup(keyHeadElement + '/' + keyElementLevelOne + '/' + expectedKey);
                    Assert.AreEqual(expectedKey, sut.Key);
                }
            }
        }

        [TestMethod]
        public void LookupNestedTwoLevelsElmentLevelOneKeyFail()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTwoLevelsNestedTypeOne());

            string keyHeadElement = tdmParent.TdmElementList[0].Key + "abc";

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne = tdmLevelOneElement.Key ;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string expectedKey = tdmLevelTwoElement.Key;

                    var sut = tdmParent.Lookup(keyHeadElement + '/' + keyElementLevelOne + '/' + expectedKey);
                    Assert.IsNull(sut);
                }
            }
        }

        [TestMethod]
        public void LookupNestedTwoLevelsElmentLevelTwoKeyFail()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTwoLevelsNestedTypeOne());

            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne =  "abc" + tdmLevelOneElement.Key;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string expectedKey = tdmLevelTwoElement.Key;

                    var sut = tdmParent.Lookup(keyHeadElement + '/' + keyElementLevelOne + '/' + expectedKey);
                    Assert.IsNull(sut);
                }
            }
        }

        [TestMethod]
        public void LookupNestedTwoLevelsElmentIsUpperCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTwoLevelsNestedTypeOne());

            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne = tdmLevelOneElement.Key;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string expectedKey = tdmLevelTwoElement.Key;

                    var sut = tdmParent.Lookup((keyHeadElement + '/' + (string)keyElementLevelOne.ToUpper() + '/' + expectedKey).ToUpper());
                    Assert.AreEqual(expectedKey, sut.Key);
                }
            }
        }

        [TestMethod]
        public void LookupNestedTwoLevelsElmentIsLowerCasePass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmTwoLevelsNestedTypeOne());

            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne = tdmLevelOneElement.Key;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string expectedKey = tdmLevelTwoElement.Key;

                    var sut = tdmParent.Lookup((keyHeadElement + '/' + (string)keyElementLevelOne.ToLower() + '/' + expectedKey));
                    Assert.AreEqual(expectedKey, sut.Key);
                }
            }
        }

        [TestMethod]
        public void LookupNestedTheeLevelsElmentPass()
        {
            var tdmParent = new TdmElement();
            tdmParent.Add(TestHelper.TdmThreeLevelsNested());

            string keyHeadElement = tdmParent.TdmElementList[0].Key;

            for (int i = 0; i < tdmParent.TdmElementList[0].TdmElementList.Count; i++)
            {
                var tdmLevelOneElement = tdmParent.TdmElementList[0].TdmElementList[i];
                string keyElementLevelOne = tdmLevelOneElement.Key;

                for (int j = 0; j < tdmLevelOneElement.TdmElementList.Count; j++)
                {
                    var tdmLevelTwoElement = tdmLevelOneElement.TdmElementList[j];
                    string keyElementLevelTwo = tdmLevelTwoElement.Key;

                    for (int z = 0; z < tdmLevelTwoElement.TdmElementList.Count; z++)
                    {
                        var tdmLevelThreeElement = tdmLevelTwoElement.TdmElementList[z];
                        string expectedKey = tdmLevelThreeElement.Key;

                        var sut = tdmParent.Lookup(keyHeadElement + '/' + keyElementLevelOne + '/' + keyElementLevelTwo + '/' + expectedKey);
                        Assert.AreEqual(expectedKey, sut.Key);
                    }
                }
            }
        }
    }
}
