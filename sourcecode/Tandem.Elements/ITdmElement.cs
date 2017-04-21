using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Tandem.Elements
{
    public interface ITdmElement
    {
        string Key { get; set; }
        object Value { get; }
        string Description { get; set; }
        List<ITdmElement> Group { get; }
        TdmElementType Type { get; set; }
        T[,] GetValue<T>();
        T[][] GetJaggedValue<T>();
        BsonValue GetBsonValue();
        void SetValue<T>(T[,] value);
        ITdmElement Lookup(List<string> keys);
        bool Contains(List<string> keys);
        bool IsEqual(ITdmElement tdmElement);
        void Add(ITdmElement tdmElement);
        void Map(ITdmElement tdmElement);
        ITdmElement Merge(ITdmElement tdmElement);
        void Insert<T>(string key, T[,] value);
        void Insert(string key, ITdmElement tdmElement);
        bool IsEmpty { get; }
        bool IsTable { get; }
        bool HasChildren { get; }
        BsonDocument ToBsonDocument(bool writeMasterKey = true);

        void FromBsonDocument(BsonDocument bson);
        void FromBsonValue(string key, BsonValue bsonValue);
    }

    public interface ITdmElement<T> : ITdmElement
    {
        new T[,] Value { get; set; }
    }
}
