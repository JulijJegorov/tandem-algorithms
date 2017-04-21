using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Tandem.Elements.Utilities;

namespace Tandem.Elements.Test.Utilities
{
    [TestClass]
    public class ITdmElementSerializeShould
    {
        private ITdmElement _tdmItem;
        private ITdmElement _tdmGroup;
        private string _jsonPath;

        [TestInitialize]
        public void ITdmElementSerializeShouldInitialize()
        {
            _tdmItem = new TdmElementItem<double> { Key = "Key", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };
            _tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem } };

            var jsonBuilder = new StringBuilder();
            jsonBuilder.Append(Path.GetDirectoryName(Assembly.GetAssembly(typeof(ITdmElementSerializeShould)).Location));
            jsonBuilder.Append("\\SerializationTest.json");
            _jsonPath = jsonBuilder.ToString();
        }

        [TestMethod]
        public void return_json_string_when_item_is_serialized()
        {
            string expected = "{\"Key\":\"Key\",\"Value\":[[1.1,1.2],[2.1,2.2]]}";
            string actual = Serialize.ElementToJson(_tdmItem);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void return_json_string_when_group_is_serialized()
        {
            string expected = "{\"Group\":[{\"Key\":\"Key\",\"Value\":[[1.1,1.2],[2.1,2.2]]}],\"Key\":\"Group_1\"}";
            string actual = Serialize.ElementToJson(_tdmGroup);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void return_item_when_json_string_is_deserialized()
        {
            string json = "{\"Key\":\"Key\",\"Value\":[[1.1,1.2],[2.1,2.2]]}";
            
            ITdmElement actual = Serialize.JsonToElement(json);

            var expectedValue = (double[,])_tdmItem.Value;
            var actualValue  = (object[,]) actual.Value;
           
            Assert.AreEqual(_tdmItem.Key, actual.Key );
            Assert.IsTrue(expectedValue.Cast<double>().SequenceEqual(actualValue.Cast<double>()));
        }

        [TestMethod]
        public void return_group_when_json_string_is_deserialized()
        {
            string json = "{\"Group\":[{\"Key\":\"Key\",\"Value\":[[1.1,1.2],[2.1,2.2]]}],\"Key\":\"Group_1\"}";

            ITdmElement actual = Serialize.JsonToElement(json);

            var expectedValue = (double[,])_tdmItem.Value;
            var actualValue = (object[,])actual.Group[0].Value;

            Assert.AreEqual(_tdmGroup.Key, actual.Key);
            Assert.IsNull(actual.Value);
            Assert.AreEqual(_tdmItem.Key, actual.Group[0].Key);
            Assert.IsTrue(expectedValue.Cast<double>().SequenceEqual(actualValue.Cast<double>()));
        }

        [TestMethod]
        public void write_to_json_file_when_item_is_serialized()
        {
            delete_json_file_if_exists();
            Serialize.ElementToJson(_tdmItem, _jsonPath);
            Assert.IsTrue(File.Exists(_jsonPath));
        }

        [TestMethod]
        public void read_from_json_file_when_file_is_deserialized()
        {
            delete_json_file_if_exists();
            Serialize.ElementToJson(_tdmItem, _jsonPath);
            ITdmElement actual = Serialize.JsonToElement(_jsonPath, isFilePath: true);

            var expectedValue = (double[,])_tdmItem.Value;
            var actualValue = (object[,])actual.Value;

            Assert.AreEqual(_tdmItem.Key, actual.Key);
            Assert.IsTrue(expectedValue.Cast<double>().SequenceEqual(actualValue.Cast<double>()));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void throw_invalid_operation_exception_when_bad_json_token_is_provided()
        {
            string json = "{\"WrongKey\":\"Key\",\"WrongValue\":[[1.1,1.2],[2.1,2.2]]}"; ;
            ITdmElement actual = Serialize.JsonToElement(json);
        }

        private void delete_json_file_if_exists()
        {
            if (File.Exists(_jsonPath))
                File.Delete(_jsonPath);
        }

    }
}
