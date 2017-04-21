using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementGetValueShould
    {
        [TestMethod]
        public void return_double_with_double_to_double_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };
            var expected = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } };
            var actual = item.GetValue<double>();
            Assert.IsInstanceOfType(actual, typeof(double[,]));
            Assert.IsTrue(actual.Cast<double>().SequenceEqual(expected.Cast<double>()));
        }

        [TestMethod]
        public void return_string_with_double_to_string_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };
            var expected = new string[,] { { "1.1", "1.2" }, { "2.1", "2.2" } };
            var actual = item.GetValue<string>();
            Assert.IsInstanceOfType(actual, typeof(string[,]));
            Assert.IsTrue(actual.Cast<string>().SequenceEqual(expected.Cast<string>()));
        }

        [TestMethod]
        public void return_date_with_double_to_datetime_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Date", Value = new double[,] { { 42175 } } };
            string expected = "20/06/2015 00:00:00";
            var actual = item.GetValue<DateTime>();

            Assert.IsInstanceOfType(actual, typeof(System.DateTime[,]));
            Assert.AreEqual(expected, actual[0, 0].ToString());
        }

        [TestMethod]
        public void return_datetime_with_double_to_datetime_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "DateTime", Value = new double[,] { { 42175.8624478009 } } };
            string expected = "20/06/2015 20:41:55";
            var actual = item.GetValue<DateTime>();

            Assert.IsInstanceOfType(actual, typeof(System.DateTime[,]));
            Assert.AreEqual(expected, actual[0, 0].ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_group()
        {
            ITdmElement group = new TdmElementGroup
            {
                Key = "group",
                Group = {
                    new TdmElementItem<string> { Key = "key", Value = new string[,] { { "some string" } } }
                }
            };

            var actual = group.GetValue<string>();
        }
    }
}
