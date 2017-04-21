using System;
using Tandem.Elements;

namespace Tandem.Excel.Utilities
{
    public interface IElementUtilities
    {
        ITdmElement GridToElement(object[,] value);
        ITdmElement ValueToElement(object[,] value);
    }
}
