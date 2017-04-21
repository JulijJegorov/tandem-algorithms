using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tandem.Elements;
using Tandem.Excel.UI.Wizard;

namespace Tandem.Excel.Test.UI.Wizard
{
    [TestClass]
    public class WzElementFactoryShould
    {
        [TestMethod]
        public void return_wz_element_group_class_for_tdm_element_group_type()
        {
            IWzElement sut = WzElementFactory.GetElementClass(typeof(TdmElementGroup));
            Assert.IsInstanceOfType(sut, typeof(WzElementGroup));
        }

        [TestMethod]
        public void return_wz_element_item_class_for_tdm_element_item_type()
        {
            IWzElement sut = WzElementFactory.GetElementClass(typeof(TdmElementItem<string>));
            Assert.IsInstanceOfType(sut, typeof(WzElementItem));
        }
    }
}
