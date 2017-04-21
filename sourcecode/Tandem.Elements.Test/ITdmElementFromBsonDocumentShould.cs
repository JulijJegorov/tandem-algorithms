using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Linq;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementFromBsonDocumentShould
    {
        [TestMethod]
        public void return_array_when_bsonarray_is_deserialized()
        {
            string json = "{ \"key\" : [[1.1, 1.2], [2.1, 2.2]] }";
            object[,] expected = new object[,] { { 1.1, 1.2 }, { 2.1, 2.2 } };

            var sut = new TdmElementGroup();
            sut.FromBsonDocument(BsonSerializer.Deserialize<BsonDocument>(json));

            object[,] actual = (object[,])sut.Group[0].Value;

            Assert.AreEqual("key", sut.Group[0].Key);
            Assert.IsTrue(actual.Cast<double>().SequenceEqual(expected.Cast<double>()));
        }


        [TestMethod]
        public void return_array_when_bosnvalue_is_deserialized()
        {
            string json = "{ \"date\" : ISODate(\"2015-06-20T00:00:00Z\") }";
            object[,] expected = new object[,] { { Convert.ToDateTime("2015/06/20") } };

            var sut = new TdmElementGroup();
            sut.FromBsonDocument(BsonSerializer.Deserialize<BsonDocument>(json));

            object[,] actual = (object[,])sut.Group[0].Value;

            Assert.AreEqual("date", sut.Group[0].Key);
            Assert.IsTrue(actual.Cast<DateTime>().SequenceEqual(expected.Cast<DateTime>()));

        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_item()
        {
            ITdmElement sut = new TdmElementItem<string> { };
            sut.FromBsonDocument(new BsonDocument());
        }
    }

}
