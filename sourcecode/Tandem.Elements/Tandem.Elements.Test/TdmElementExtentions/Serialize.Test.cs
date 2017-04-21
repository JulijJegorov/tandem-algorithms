using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class SerializeTest
    {
        private StringBuilder _testJsonPath;
        public SerializeTest()
        {    
            string path = Assembly.GetAssembly(typeof(SerializeTest)).Location;
            _testJsonPath = new StringBuilder();
            _testJsonPath.Append(Path.GetDirectoryName(path));
            _testJsonPath.Append("\\UnitTest.json");
        }

        [TestMethod]
        public void SerializeSingleValueToJsonPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            string actual = tdmElement.SerializeToJSON();
            string expected = TestHelper.jsonTdmSingleValue();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Serialize2DArrayToJsonPass()
        {
            var tdmElement = TestHelper.Tdm2DArray();
            string actual = tdmElement.SerializeToJSON();
            string expected = TestHelper.jsonTdm2DArray();
            Assert.AreEqual(expected, actual);
        }        
        [TestMethod]
        public void SerializeDictionaryToJsonPass()
        {
            var tdmElement = TestHelper.TdmDictionary();
            string expected = TestHelper.jsonTdmDictionary();   
            string actual = tdmElement.SerializeToJSON();
            Assert.AreEqual(expected.ToString(), actual);
        }

        [TestMethod]
        public void SerializeTdmThreeLevelsNestedToJsonPass()
        {
            var tdmElement = TestHelper.TdmThreeLevelsNested();
            string expected = TestHelper.jsonTdmThreeLevelsNested();
            string actual = tdmElement.SerializeToJSON();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SerializeSingleValueToJsonFilePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            string json = tdmElement.SerializeToJSON(_testJsonPath.ToString());
            Assert.AreEqual(_testJsonPath.ToString(), json);
        }

        [TestMethod]
        public void DeserializeStringPass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            string json = TestHelper.jsonTdmSingleValue();

            var sut = new TdmElement();
            sut.DeserializeJSON(json);

            Assert.AreEqual(sut.Key, tdmElement.Key);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void DeserializeDictionaryPass()
        {
            var tdmElement = TestHelper.TdmDictionary();
            var sut = new TdmElement();
            sut.DeserializeJSON(TestHelper.jsonTdmDictionary());

            bool areEqual = true;
            for (int i = 0; i < sut.TdmElementList.Count; i++)
            {
                areEqual = areEqual & (sut.TdmElementList[i].Key == sut.TdmElementList[i].Key);
                areEqual = areEqual & (sut.TdmElementList[i].Value[0,0] == sut.TdmElementList[i].Value[0,0]);
            }
            Assert.IsTrue(areEqual);
        }

        [TestMethod]
        public void DeserializeSingleValueFromJsonFilePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = new TdmElement();
            sut.DeserializeJSON(_testJsonPath.ToString(), true);
            Assert.AreEqual(sut.Key, tdmElement.Key);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }
    }
}
