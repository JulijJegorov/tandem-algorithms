using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tandem.Elements
{
    [Serializable]
    public class TdmElementItem<T> : ITdmElement<T>
    {
        private T[,] _value;
        private string _key;
        private string _description;

        private List<ITdmElement> _tdmElements = new List<ITdmElement>();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonIgnore]
        public List<ITdmElement> Group { get { return _tdmElements; } }
        [JsonIgnore]
        public bool IsEmpty { get { return _value == null; } }
        public bool IsTable { get { return false; } }

        [JsonIgnore]
        public bool HasChildren { get { return false; } }
        [JsonIgnore]
        public TdmElementType Type { get; set; }

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

        public T[,] Value
        {
            get { return _value; }
            set { this._value = value; }
        }

        object ITdmElement.Value { get { return this.Value; } }

        public TOut[,] GetValue<TOut>()
        {
            if (typeof(T) == typeof(TOut))
                return _value as TOut[,];

            int rows = _value.GetLength(0);
            int cols = _value.GetLength(1);

            TOut[,] array = new TOut[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (typeof(TOut) == typeof(DateTime))
                        array[i, j] = (dynamic)_castToDateTime(_value[i, j]);
                    else
                        array[i, j] = (TOut)Convert.ChangeType(_value[i, j], typeof(TOut));
                }
            }
            return array;
        }

        public TOut[][] GetJaggedValue<TOut>()
        {
            int rowsFirstIndex = _value.GetLowerBound(0);
            int rowsLastIndex = _value.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = _value.GetLowerBound(1);
            int columnsLastIndex = _value.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            TOut[][] arrayJagged = new TOut[numberOfRows][];

            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                arrayJagged[i] = new TOut[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    if (typeof(TOut) == typeof(DateTime))
                    {
                        arrayJagged[i][j] = (dynamic)_castToDateTime(_value[i, j]);
                    }
                    else
                    {
                        arrayJagged[i][j] = (TOut)Convert.ChangeType(_value[i, j], typeof(TOut));
                    }
                }
            }
            return arrayJagged;
        }

        private DateTime _castToDateTime(object value)
        {
            double date = (double)Convert.ChangeType(value, typeof(double));
            return TimeZoneInfo.ConvertTimeToUtc(DateTime.FromOADate(date), TimeZoneInfo.Utc);
        }


        public void SetValue<TIn>(TIn[,] value)
        {
            int rows = _value.GetLength(0);
            int columns = _value.GetLength(1);

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _value[i, j] = (T)Convert.ChangeType(value[i, j], typeof(T));
                }
            }
        }

        public ITdmElement Lookup(List<string> keys)
        {
            if (keys.Count == 0)
                return this;

            return keys[0].ToString().ToUpper() == this.Key.ToUpper() ? this : null;
        }

        public bool Contains(List<string> keys)
        {
            if (keys.Count == 0)
                return true;
            return keys[0].ToString().ToUpper() == this.Key.ToUpper();
        }

        public void Map(ITdmElement tdmElement)
        {
            if (_key.ToUpper() == tdmElement.Key.ToUpper())
            {
                this.Type = tdmElement.Type;
                _value = tdmElement.GetValue<T>();
            }
        }

        public ITdmElement Merge(ITdmElement tdmElement)
        {
            return tdmElement.GetType() == typeof(TdmElementGroup) ?
                    tdmElement.Merge(this) :
                            new TdmElementGroup { Group = { this, tdmElement } };
        }


        public bool IsEqual(ITdmElement tdmElement)
        {
            T[,] tdmValue;

            try
            {
                tdmValue = (T[,])tdmElement.Value;
            }
            catch
            {
                return false;
            }

            return this._key.ToUpper() == tdmElement.Key.ToUpper() && this._value.Rank == tdmValue.Rank &&
                    Enumerable.Range(0, this._value.Rank).All(dimension => this._value.GetLength(dimension) == tdmValue.GetLength(dimension))
                        && this._value.Cast<T>().SequenceEqual(tdmValue.Cast<T>());

        }


        public void FromBsonValue(string key, BsonValue bsonValue)
        {
            BsonTypeMapperOptions options = new BsonTypeMapperOptions();
            options.MapBsonArrayTo = typeof(List<object>);

            this._key = key;
            if (bsonValue is BsonArray)
            {
                List<object> array = (List<object>)BsonTypeMapper.MapToDotNetValue(bsonValue, options);

                int rows = array.Count;
                if (array[0] is List<object>)
                {
                    var temp = (List<object>)array[0];
                    int cols = temp.Count;
                    _value = new T[rows, cols];

                    for (int i = 0; i < rows; i++)
                    {
                        var item = (List<object>)array[i];
                        for (int j = 0; j < cols; j++)
                        {
                            _value[i, j] =(T)item[j];
                        }
                    }
                }
                else
                {
                    _value = new T[rows, 1];
                    for (int i = 0; i < rows; i++)
                    {
                        _value[i, 0] = (T)array[i];
                    }
                }
            }
            else
            {
                _value = new T[1, 1];
                _value[0, 0] = (T)BsonTypeMapper.MapToDotNetValue(bsonValue);
            }
        }

        public BsonDocument ToBsonDocument(bool writeMasterKey = true)
        {
            return new BsonDocument(_key, this.GetBsonValue());
        }

        public BsonValue GetBsonValue()
        {
            BsonValue bsonValue = null;

            int rows = _value.GetLength(0);
            int cols = _value.GetLength(1);

            if (cols == 1)
            {
                if (rows == 1)
                {
                    bsonValue = BsonValue.Create(_value[0,0]);
                }
                else
                {
                    object[] array1D = _value.Cast<object>().ToArray();
                    bsonValue = new BsonArray().AddRange(array1D);
                }
            }
            else
            {
                dynamic jaggedArray = this.GetJaggedValue<object>();
                bsonValue = BsonValue.Create(jaggedArray);
            }

            return bsonValue;
        }


        public void Add(ITdmElement tdmElement)
        {
            throw new NotImplementedException();
        }

        public void Insert<TIn>(string key, TIn[,] value)
        {
            throw new NotImplementedException();
        }

        public void Insert(string key, ITdmElement tdmElement)
        {
            throw new NotImplementedException();
        }

        public void FromBsonDocument(BsonDocument bson)
        {
            throw new NotImplementedException();
        }
    }
}


