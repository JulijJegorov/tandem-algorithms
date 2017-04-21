using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tandem.Elements
{
    [Flags]
    public enum TdmElementType : byte
    {
        Grid, Model, Executable
    }
}
