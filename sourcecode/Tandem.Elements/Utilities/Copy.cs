using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Tandem.Elements.Utilities
{
    public static class Copy
    {
        public static ITdmElement DeepCopy(this ITdmElement tdmElement)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, tdmElement);
                ms.Position = 0;

                return (ITdmElement)formatter.Deserialize(ms);
            }
        }
    }
}
