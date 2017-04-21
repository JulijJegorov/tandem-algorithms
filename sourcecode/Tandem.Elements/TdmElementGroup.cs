using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Tandem.Elements.Utilities;

namespace Tandem.Elements
{
    [Serializable]
    public class TdmElementGroup: ITdmElement
    {
        private List<ITdmElement> _tdmElements = new List<ITdmElement>();
        private string _key;
        private string _description;
        public List<ITdmElement> Group { get { return _tdmElements; } }

        public string Description
        {
            get { return _description; }
            set { this._description = value; }
        }

        public string Key
        {
            get { return _key; }
            set { this._key = value; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonIgnore]
        public object Value { get { return null; } }
        [JsonIgnore]
        public bool IsEmpty { get { return this.Group.Count == 0; } }
        [JsonIgnore]
        public bool IsTable { get { return this._isTable(); } }
        [JsonIgnore]
        public bool HasChildren { get { return this.Group.Count > 0; ; } }
        [JsonIgnore]
        public TdmElementType Type { get; set; }

        public TdmElementGroup()
        {
        }

        public void Add(ITdmElement tdmElement)
        {
            this.Group.Add(tdmElement);
        }

        public void Insert(string key, ITdmElement tdmElement)
        {
            this.Key = key;
            this.Group.Add(tdmElement);
        }

        public void Insert<T>(string key, T[,] value)
        {
            this.Group.Add(new TdmElementItem<T> { Key = key, Value = value });
        }

        public ITdmElement Lookup(List<string> keys)
        {
            if (keys.Count == 0)
                return this;

            foreach (var item in this.Group)
            {
                if (keys[0].ToString().ToUpper() == item.Key.ToUpper())
                {
                    keys.RemoveAt(0);
                    return item.Lookup(keys);
                }
            }
            return null;
        }

        public bool Contains(List<string> keys)
        {
            if (keys.Count == 0)
                return true;

            foreach (var item in this.Group)
            {
                if (keys[0].ToString().ToUpper() == item.Key.ToUpper())
                {
                    keys.RemoveAt(0);
                    return item.Contains(keys);
                }
            }
            return false;
        }

        public void Map(ITdmElement tdmElement)
        {
            foreach (var mapItem in tdmElement.Group)
            {
                foreach (var item in this.Group)
                {
                    if (mapItem.Key.ToString().ToUpper() == item.Key.ToUpper())
                    {
                        item.Map(mapItem);
                    }
                }
            }
        }

        public ITdmElement Merge(ITdmElement tdmElement)
        {
            ITdmElement result = (ITdmElement)this.DeepCopy();
            if (tdmElement.GetType() == typeof(TdmElementGroup))
                tdmElement.Group.ForEach(tdmItem => result.Group.Add(tdmItem));
            else
                result.Group.Add(tdmElement);

            return result;
        }

        public bool IsEqual(ITdmElement tdmElement)
        {
            int count = this.Group.Count;
            if (this._key.ToUpper() != tdmElement.Key.ToUpper()
                                                || count != tdmElement.Group.Count)
                return false;

            for (int i = 0; i < count; i++)
            {
                var thisItem = this.Group[i];
                var tdmItem = tdmElement.Group[i];

                return thisItem.Key.ToUpper() == tdmItem.Key.ToUpper() ?
                                                       thisItem.IsEqual(tdmItem) : false;
            }
            return false;
        }

        public BsonDocument ToBsonDocument(bool writeMasterKey = true)
        {
            var bsonChild = new BsonDocument();
            foreach (var item in this.Group)
            {
                bsonChild.AddRange(item.ToBsonDocument());
            }
            var bsonParent = new BsonDocument();
            return writeMasterKey ?  bsonParent.Add(this.Key, bsonChild) : bsonParent.AddRange(bsonChild);
        }

        public void FromBsonDocument(BsonDocument bson)
        {
            foreach (string key in bson.Names)
            {
                BsonValue value = (BsonValue)bson[key];

                if (value is BsonDocument)
                {
                    _addBsonDocument(key, (BsonDocument)value);
                }
                else if (value is BsonArray)
                {
                    if (value[0] is BsonDocument)
                    {
                        BsonArray temp = (BsonArray)value;
                        for (int i = 0; i < temp.Count; i++)
                        {
                            _addBsonDocument("[ " + i + " ]", (BsonDocument)temp[i]);
                        }
                    }
                    else
                    {
                        _addBsonValue(key, (BsonValue)value);
                    }
                }
                else
                {
                    _addBsonValue(key, (BsonValue)value);
                }
            }
        }

        private void _addBsonDocument(string key, BsonDocument bson)
        {
            ITdmElement element = new TdmElementGroup();
            element.Key = key;
            element.FromBsonDocument((BsonDocument)bson);
            this.Group.Add(element);
        }

        private void _addBsonValue(string key, BsonValue bson)
        {
            var item = new TdmElementItem<object>();
            item.FromBsonValue(key, (BsonValue)bson);
            this.Group.Add(item);
        }

        private bool _isTable()
        {
            List<string> keys_1 = null;
            foreach (var element in this.Group)
            {
                if (!element.HasChildren) return false;

                List<string> keys_2 = new List<string>();
                foreach (var item in element.Group)
                {
                    if (item.HasChildren) return false;

                    object[,] value = item.GetValue<object>();
                    if (value.GetLength(0) != 1 || value.GetLength(1) != 1) return false;
                    keys_2.Add(item.Key);
                }
                if (keys_1 != null)
                {
                    if (!keys_1.SequenceEqual(keys_2)) return false;
                }
                keys_1 = keys_2;
            }
            return true;
        }

        T[,] ITdmElement.GetValue<T>()
        {
            throw new NotImplementedException();
        }

        T[][] ITdmElement.GetJaggedValue<T>()
        {
            throw new NotImplementedException();
        }

        BsonValue ITdmElement.GetBsonValue()
        {
            throw new NotImplementedException();
        }

        void ITdmElement.SetValue<TIn>(TIn[,] value)
        {
            throw new NotImplementedException();
        }

        public void FromBsonValue(string key, BsonValue bsonValue)
        {
            throw new NotImplementedException();
        }
    }
}
