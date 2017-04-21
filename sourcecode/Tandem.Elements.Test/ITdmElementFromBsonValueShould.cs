using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using System;
using System.Linq;
namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementFromBsonValueShould
    {
        [TestMethod]
        public void return_array_when_bsonarray_is_deserialized()
        {
            object[,] expected = new object[,] { { 1.1, 1.2 }, { 2.1, 2.2 } };
            BsonValue bsonValue = new BsonArray().Add(BsonValue.Create(expected));

            ITdmElement sut = new TdmElementItem<object>();
            sut.FromBsonValue("key", bsonValue);
            object[,] actual = (object[,])sut.Value;

            Assert.AreEqual("key", sut.Key);
            Assert.IsTrue(actual.Cast<double>().SequenceEqual(expected.Cast<double>()));
        }

        [TestMethod]
        public void return_array_when_bsonvalue_is_deserialized()
        {
            object[,] expected = new object[,] { { 1.1 } };

            ITdmElement sut = new TdmElementItem<object>();
            sut.FromBsonValue("key", 1.1);
            object[,] actual = (object[,])sut.Value;

            Assert.AreEqual("key", sut.Key);
            Assert.IsTrue(actual.Cast<double>().SequenceEqual(expected.Cast<double>()));
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_group()
        {
            var sut = new TdmElementGroup();
            sut.FromBsonValue("key", new BsonArray());
        }
    }
}
