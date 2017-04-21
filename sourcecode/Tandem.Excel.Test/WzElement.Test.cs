using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements;
using Tandem.Excel.UI.Wizard;

namespace Tandem.Excel.Test
{
    [TestClass]
    public class WzElementTest
    {
        private TdmElementItem<double> _tdmItem_1;
        private TdmElementGroup _tdmGroup;

        [TestInitialize]
        public void TestClassInitialize()
        {
            _tdmItem_1 = new TdmElementItem<double> { Key = "Key_1", Value = new double[,] { { 1.1, 5.3242 }, { 432, 53566 } } };
            _tdmGroup = new TdmElementGroup { Group = { _tdmItem_1 } };
        }


        [TestMethod]
        public void CopyElementItem()
        {
            var sut = new WzElementItem();
            sut.CopyElement(_tdmItem_1);
        }

        [TestMethod]
        public void CopyElementGroup()
        {
            var sut = new WzElementGroup();
            sut.CopyElement(_tdmGroup);
        }

    }
}
