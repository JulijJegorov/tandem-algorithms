using System;
using System.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Excel.UI.Wizard;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.QualityTools.Testing.Fakes;
using Tandem.Elements.Fakes;

namespace Tandem.Excel.Test.UI.Wizard
{
    [TestClass]
    public class WzElementItemShould
    {
        [TestMethod]
        public void have_public_set_and_public_get_name_property()
        {
            var sut = new WzElementItem { Name = "Name" };
            Assert.AreEqual("Name", sut.Name);
        }

        [TestMethod]
        public void have_public_set_and_public_get_key_property()
        {
            var sut = new WzElementItem { Key = "Key" };
            Assert.AreEqual("Key", sut.Key);
        }

        [TestMethod]
        public void have_public_set_and_public_get_value_property()
        {
            var sut = new WzElementItem { Value = new object[,] {{ 1.2 }}};
            Assert.AreEqual(1.2, ((object[,])sut.Value)[0, 0]);
        }
        [TestMethod]
        public void have_public_get_value_to_grid_property()
        {
            ///DataView dataView
        }


    }
}
