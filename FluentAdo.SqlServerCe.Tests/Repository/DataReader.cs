using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

//namespace FluentAdoDotNet
//{
    public class DataReader : IEnumerable
    {
        private readonly DbDataReader _reader;
        private readonly Dictionary<string, int> _columnOrdinals = new Dictionary<string, int>();

        public DataReader(DbDataReader reader)
        {
            _reader = reader;
        }

        public int GetColumn(string name)
        {
            if (!_columnOrdinals.ContainsKey(name))
            {
                var i = _reader.GetOrdinal(name);
                _columnOrdinals.Add(name, i);
            }
            return _columnOrdinals[name];
        }

        public T GetValue<T>(string name)
        {
            return (T)_reader.GetValue(GetColumn(name));
        }

        public bool GetBoolean(string name)
        {
            var iCol = GetColumn(name);
            return _reader.GetBoolean(iCol);
        }
        
        public bool GetBoolean(string name, bool defaultValue)
        {
            return GetBooleanNullable(name) ?? defaultValue;
        }

        public bool? GetBooleanNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
                return _reader.GetBoolean(iCol);
            return null;
        }

        public string GetString(string name)
        {
            var iCol = GetColumn(name);
            return _reader.GetString(iCol);
        }

        public string GetStringNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
                return _reader.GetString(iCol);
            return string.Empty;
        }

        public int GetInt(string name)
        {
            var iCol = GetColumn(name);
            return _reader.GetInt32(iCol);
        }

        public int GetInt(string name, int defaultValue)
        {
            return GetIntNullable(name) ?? defaultValue;
        }

        public int? GetIntNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
                return _reader.GetInt32(iCol);
            return null;
        }

        public decimal GetDecimal(string name)
        {
            var iCol = GetColumn(name);
            return _reader.GetDecimal(iCol);
        }

        public decimal GetDecimal(string name, decimal defaultValue)
        {
            return GetDecimalNullable(name) ?? defaultValue;
        }


        public decimal? GetDecimalNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
                return _reader.GetDecimal(iCol);
            return null;
        }

        public Guid GetGuid(string name)
        {
            var iCol = GetColumn(name);
            return _reader.GetGuid(iCol);
        }

        public DateTime GetDateTime(string name)
        {
            var icol = GetColumn(name);
            return _reader.GetDateTime(icol);
        }

        public DateTime? GetDateTimeNullable(string name)
        {
            var iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
                return _reader.GetDateTime(iCol);
            else
                return null;
        }

    public IEnumerator GetEnumerator()
    {
        return _reader.GetEnumerator();
    }
    }
//}