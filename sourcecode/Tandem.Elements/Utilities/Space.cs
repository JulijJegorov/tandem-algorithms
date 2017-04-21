using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tandem.Elements.Utilities
{
    public class Space
    {
        private MongoClient _mongoClient = null;

        private Space()
        {
        }

        /// <summary>
        /// Instance of <see cref="Space"/> class</summary>
        public static Space Instance
        {
            get { return Nested.instance; }
        }

        private class Nested
        {
            static Nested()
            {
            }
            internal static readonly Space instance = new Space();
        }

        public void Connect(string uri)
        {
            _mongoClient = new MongoClient(uri);
        }

        public bool IsConnected
        {
            get { return (_mongoClient == null)? false: true; }
        }

        public async static Task WriteAsync(string database, string collection, ITdmElement element)
        {
            var client = Space.Instance._mongoClient;
            var db = client.GetDatabase(database);
            var data = db.GetCollection<BsonDocument>(collection);

            BsonDocument bson = new BsonDocument();

            bson.AddRange(element.ToBsonDocument(false));

            await data.InsertOneAsync(bson);
        }

        public async static Task WriteManyAsync(string database, string collection, ITdmElement element)
        {
            var client = Space.Instance._mongoClient;
            var db = client.GetDatabase(database);
            var data = db.GetCollection<BsonDocument>(collection);

            var bsonDocuments = new List<BsonDocument>();
            foreach (ITdmElement item in element.Group)
            {
                bsonDocuments.Add(item.ToBsonDocument(false));
            }

            await data.InsertManyAsync(bsonDocuments);
        }

        public async static Task UpdateAsync(string database, string collection, ITdmElement element)
        {
            var client = Space.Instance._mongoClient;
            var db = client.GetDatabase(database);
            var data = db.GetCollection<BsonDocument>(collection);

            var id = element.Lookup(new List<string> { "_id" }).GetValue<string>()[0,0];
            var filter = Builders<BsonDocument>.Filter.Eq("_id", id);
           
            foreach (ITdmElement item in element.Group)
            {
                if (item.HasChildren)
                {
                    var bson  = item.ToBsonDocument(false);
                    var update = Builders<BsonDocument>.Update.Set(item.Key, bson);
                    await data.UpdateOneAsync(filter, update);
                }
                else
                {
                    var bson = item.GetBsonValue();
                    var update = Builders<BsonDocument>.Update.Set(item.Key, bson);
                    await data.UpdateOneAsync(filter, update);
                }
            }
        }

        public async static Task<ITdmElement> ReadAsync(string database, string collection, ITdmElement match, ITdmElement group, ITdmElement project, ITdmElement sort)
        {
            ITdmElement element = new TdmElementGroup();
            var client = Space.Instance._mongoClient;
            var db = client.GetDatabase(database);

            BsonDocument aggregate = new BsonDocument("aggregate", collection);

            BsonArray pipeline = new BsonArray();

            if (!match.IsEmpty)
                pipeline.Add(new BsonDocument("$match", match.ToBsonDocument(false)));

            if (!group.IsEmpty)
                pipeline.Add(new BsonDocument("$group", group.ToBsonDocument(false)));

            if (!project.IsEmpty)
                pipeline.Add(new BsonDocument("$project", project.ToBsonDocument(false)));

            if (!sort.IsEmpty)
                pipeline.Add(new BsonDocument("$sort", sort.ToBsonDocument(false)));

            aggregate.Add("pipeline", pipeline);

            var result = await db.RunCommandAsync<BsonDocument>(aggregate);

            result.Remove("ok");
            element.FromBsonDocument(result);

            return (element.Group.Count == 1) ? element.Group[0] : element;
        }
    }
}
