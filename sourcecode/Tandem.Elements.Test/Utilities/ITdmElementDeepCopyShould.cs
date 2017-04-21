using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements.Utilities;
using System.Linq;

namespace Tandem.Elements.Test.Utilities
{
    [TestClass]
    public class ITdmElementDeepCopyShould
    {
        private ITdmElement _tdmItem;
        private ITdmElement _tdmGroup;

        [TestInitialize]
        public void ITdmElementDeepCopyShouldInitialize()
        {
            _tdmItem = new TdmElementItem<double> { Key = "Key", Value = new double[,] { { 1.1, 1.2 }, { 2.1, 2.2 } } };
            _tdmGroup = new TdmElementGroup { Key = "Group_1", Group = { _tdmItem } };
        }

        [TestMethod]
        public void create_exact_copy_of_an_item()
        {
            ITdmElement actual = _tdmItem.DeepCopy();

            var expectedValue = (double[,])_tdmItem.Value;
            var actualValue = (double[,])actual.Value;

            Assert.AreEqual(_tdmItem.Key, actual.Key);
            Assert.IsTrue(expectedValue.Cast<double>().SequenceEqual(actualValue.Cast<double>()));
        }

        [TestMethod]
        public void create_exact_copy_of_a_group()
        {
            ITdmElement actual = _tdmGroup.DeepCopy();

            var expectedValue = (double[,])_tdmItem.Value;
            var actualValue = (double[,])actual.Group[0].Value;

            Assert.AreEqual(_tdmGroup.Key, actual.Key);
            Assert.IsNull(actual.Value);
            Assert.AreEqual(_tdmItem.Key, actual.Group[0].Key);
            Assert.IsTrue(expectedValue.Cast<double>().SequenceEqual(actualValue.Cast<double>()));
        }
    }
}
