using Newtonsoft.Json.Linq;
using System;

namespace Tandem.Elements.Serialization
{
    public class TdmJsonConverter : AbstractJsonConverter<ITdmElement>
    {
        protected override ITdmElement Create(Type objectType, JObject jObject)
        {
            if (FieldExists(jObject, "Group", JTokenType.Array))
                return new TdmElementGroup();

            if (FieldExists(jObject, "Value", JTokenType.Array))
                return new TdmElementItem<object>();

            throw new InvalidOperationException();
        }
    }
}
