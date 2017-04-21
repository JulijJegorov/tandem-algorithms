using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tandem.Elements.Test
{
    [TestClass]
    public class DeepCopyTest
    {
        [TestMethod]
        public void DeepCopySingleValuePass()
        {
            var tdmElement = TestHelper.TdmSingleValue();
            var sut = new TdmElement();
            sut.DeepCopy2(tdmElement);

            Assert.AreEqual(sut.Key, tdmElement.Key);
            Assert.AreEqual(sut.Value[0, 0], tdmElement.Value[0, 0]);
        }

        [TestMethod]
        public void DeepCopyDictionaryPass()
        {
            var tdmElement = TestHelper.TdmDictionary();
            var sut = new TdmElement();
            sut.DeepCopy2(tdmElement);
            
            bool areEqual = true;
            for (int i = 0; i < sut.TdmElementList.Count; i++)
            {
                areEqual = areEqual & (sut.TdmElementList[i].Key == tdmElement.TdmElementList[i].Key);
                areEqual = areEqual & (sut.TdmElementList[i].Value[0, 0] == tdmElement.TdmElementList[i].Value[0, 0]);
            }
            Assert.IsTrue(areEqual);
            Assert.AreEqual(sut.Key, tdmElement.Key);
        }
    }
}
    
