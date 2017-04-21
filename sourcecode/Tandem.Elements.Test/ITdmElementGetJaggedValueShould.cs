using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class ITdmElementGetJaggedValueShould
    {
        [TestMethod]
        public void return_double_with_double_to_double_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };

            double[][] expected = new double[2][];
            expected[0] = new double[] { 1.1, 1.2 };
            expected[1] = new double[] { 2.1, 2.2 };

            var actual = item.GetJaggedValue<double>();

            Assert.IsInstanceOfType(actual, expected.GetType());
            Assert.IsTrue(_arraysAreEqual(expected, actual));
        }


        [TestMethod]
        public void return_string_with_double_to_string_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };

            string[][] expected = new string[2][];
            expected[0] = new string[] { "1.1", "1.2" };
            expected[1] = new string[] { "2.1", "2.2" };

            var actual = item.GetJaggedValue<string>();

            Assert.IsInstanceOfType(actual, expected.GetType());
            Assert.IsTrue(_arraysAreEqual(expected, actual));
        }

        [TestMethod]
        public void return_date_with_double_to_datetime_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "Date", Value = new double[,] { { 42175 } } };

            string[][] expected = new string[1][];
            expected[0] = new string[] { "20/06/2015 00:00:00" };

            var actual = item.GetJaggedValue<DateTime>();
            Assert.IsInstanceOfType(actual, typeof(System.DateTime[][]));
            Assert.AreEqual(expected[0][0], actual[0][0].ToString());
        }

        [TestMethod]
        public void return_datetime_with_double_to_datetime_casting()
        {
            ITdmElement item = new TdmElementItem<double> { Key = "DateTime", Value = new double[,] { { 42175.8624478009 } } };
          
            string[][] expected = new string[1][];
            expected[0] = new string[] { "20/06/2015 20:41:55" };

            var actual = item.GetJaggedValue<DateTime>();
            Assert.IsInstanceOfType(actual, typeof(System.DateTime[][]));
            Assert.AreEqual(expected[0][0], actual[0][0].ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void throw_not_implemented_exception_when_called_on_group()
        {
            ITdmElement tdmChildGroup = new TdmElementGroup
            {
                Key = "group",
                Group = {
                    new TdmElementItem<string> { Key = "key", Value = new string[,] { { "some string" } } }
                }
            };

            var actual = tdmChildGroup.GetValue<string>();
        }


        private bool _arraysAreEqual<T>(T[][] expected, T[][] actual)
        {
            if (expected.Length != actual.Length) { return false; }

            for (int i = 0; i < expected.Length; i++)
            {
                T[] expectedInner = expected[i];
                T[] actualInner = actual[i];

                if (expectedInner.Length != actualInner.Length) { return false; }

                for (int j = 0; j < expectedInner.Length; j++)
                {
                    if (!EqualityComparer<T>.Default.Equals(expectedInner[j], actualInner[j])) 
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
            

        



    

