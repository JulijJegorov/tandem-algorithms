using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tandem.Elements.Utilities;

namespace Tandem.Elements.Test.Utilities
{
    [TestClass]
    public class ReadAsyncShould
    {
        [TestMethod]
        public void read_bson_documnet_from_mongo()
        {
            //String uri = "mongodb://iamAdminUser:iam2015@ds045948.mongolab.com:45948/iamspace";
            //ITdmElement integerTask = Space.ReadAsync(uri, "iamspace", "ub", "hello").Result;
        }

        [TestMethod]
        public void return_bson_documnet_from_mongo()
        {
            //String uri = "mongodb://iamAdminUser:iam2015@ds045948.mongolab.com:45948/iamspace";
            ITdmElement item = new TdmElementItem<object> { Key = "dummy", Value = new object[,] { { 1.1555, 1.2 } } };
            //Space.UpdateAsync(uri, "iamspace", "ub", "hello", item).Wait();
        }
    }
}
