using MongoDB.Bson;
using Tandem.Elements;

namespace Tandem.Excel.Functions
{
    public class MongoDbUtilities
    {
        public static BsonDocument[] _convertToBosomDocument(ITdmElement element)
        {
            int count = 0;
            if(element.HasChildren)
            {
               count = element.Group[0].GetValue<dynamic>().Length;
            }

            BsonDocument[] bsonDocuments = new BsonDocument[count];

            for (int i = 0; i < count; i++)
            {
                var bsonDocument = new BsonDocument();
                foreach (var item in element.Group)
                {
                    bsonDocument[item.Key] = item.GetValue<dynamic>()[i,0];
                }
                bsonDocuments[i] = bsonDocument;
            }

            return bsonDocuments;
        }

       
    }
}
