using Newtonsoft.Json;
using System.IO;
using Tandem.Elements.Serialization;

namespace Tandem.Elements.Utilities
{
    public static class Serialize
    {
        /// <summary>
        /// Serializes <see cref="ITdmElement"/>to JSON string></summary>
        /// <param name="address">Optional: file path to write JSON string to</param>
        /// <returns>Full file path if address is provided and JSON string otherwise</returns>
        public static string ElementToJson(ITdmElement tdmElement, string address = null)
        {
            string jsonString;

            if (address != null)
            {
                using (var streamWriter = new StreamWriter(address))
                using (var jsonWriter = new JsonTextWriter(streamWriter))
                {
                    jsonWriter.Formatting = Formatting.Indented;

                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, tdmElement);
                    jsonString = address;
                }
            }
            else
            {
                jsonString = JsonConvert.SerializeObject(tdmElement, Formatting.None);
            }
            return jsonString;
        }

        /// <summary>
        /// Serializes JSON string to <see cref="ITdmElement"/></summary>
        /// <param name="json">JSON string or file path that contains JSON string </param>
        /// <param name="isFilePath">Indicates if 'json' is a  file path containing JSON string </param> 
        public static ITdmElement JsonToElement(string json, bool isFilePath = false)
        {
            string jsonString = null;
            if (isFilePath)
            {
                using (StreamReader reader = File.OpenText(json))
                {
                    while (!reader.EndOfStream)
                        jsonString += reader.ReadLine();
                }
            }
            else
            {
                jsonString = json;
            }

            var converter = new TdmJsonConverter();
            ITdmElement tdmElement = JsonConvert.DeserializeObject<ITdmElement>(jsonString, converter);

            return tdmElement;
        }
    }
}
