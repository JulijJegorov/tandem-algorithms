using System;
using System.Collections.Generic;
using Tandem.Elements;

namespace Tandem.Excel.UI.Wizard
{
    public class UserControlUtils
    {
        private readonly TdmContainerPool _tdmContainerPool = TdmContainerPool.Instance;

        public IWzElement _getWzElement(string name)
        {
            TdmElementContainer tdmContainer = _tdmContainerPool.Get(name);
            ITdmElement tdmElement = tdmContainer.Element;
            IWzElement wzElement = WzElementFactory.GetElementClass(tdmElement.GetType());

            wzElement.CopyElement(tdmElement);

            wzElement.Key = tdmContainer.Name;
            wzElement.IsSelected = true;
            wzElement.IsExpanded = true;

            return new WzElementGroup { WzElements = new List<IWzElement> { wzElement } };
        }
    }
}
