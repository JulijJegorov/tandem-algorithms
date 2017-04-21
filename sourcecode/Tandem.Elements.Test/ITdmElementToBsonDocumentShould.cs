using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementToBsonDocumentShould
    {
        private ITdmElement _item_1;
        private ITdmElement _item_2;
        private ITdmElement _group;
        private ITdmElement _nestedGroup;

        [TestInitialize]
        public void ITdmElementToBsonShouldInitialize()
        {
            _item_1 = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };
            _item_2 = new TdmElementItem<string> { Key = "Key_2", Value = new string[,] { { "1" } } };
            _group = new TdmElementGroup { Key = "Group", Group = { _item_1, _item_2 } };
            _nestedGroup = new TdmElementGroup { Key = "NestedGroup", Group = { _item_1, _group } };
        }

        [TestMethod]
        public void return_bson_date_when_item_is_serialized()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "date", Value = new double[,] { { 42175 } } };
            string expected = "{ \"date\" : ISODate(\"2015-06-20T00:00:00Z\") }";
            string actual = item.ToBsonDocument().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void return_bson_when_item_is_serialized()
        {
            string expected = "{ \"Key_1\" : [[1.1, 1.2], [2.1, 2.2]] }";
            string actual = _item_1.ToBsonDocument().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void return_bson_when_group_is_serialized()
        {
            string expected = "{ \"Group\" : { \"Key_1\" : [[1.1, 1.2], [2.1, 2.2]], \"Key_2\" : \"1\" } }";
            string actual = _group.ToBsonDocument().ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void return_bson_when_nested_group_is_serialized()
        {
            string expected = "{ \"NestedGroup\" : { \"Key_1\" : [[1.1, 1.2], [2.1, 2.2]], \"Group\" : { \"Key_1\" : [[1.1, 1.2], [2.1, 2.2]], \"Key_2\" : \"1\" } } }";
            string actual = _nestedGroup.ToBsonDocument().ToString();
            Assert.AreEqual(expected, actual);
        }
    }
}
