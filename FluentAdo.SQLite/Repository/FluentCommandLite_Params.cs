using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace FluentAdo.SQLite
{
    public partial class FluentCommand<T>
    {
        public DbParameter CreateParam(string name, DbType dataType, object value)
        {
            var param = new SQLiteParameter(name, dataType);
            param.Value = SetParamValue(value); 
            _command.Parameters.Add(param);
            return param;
        }

        public DbParameter CreateParam(string name, DbType dataType)
        {
            var param = new SQLiteParameter(name, dataType);
            _command.Parameters.Add(param);
            return param;
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = new SQLiteParameter(name, value);
            _command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> Add(string name)
        {
            var param = new SQLiteParameter();
            param.ParameterName = name;
            _command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> AddInt(string name)
        {
            CreateParam(name, DbType.Int32);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int value)
        {
            CreateParam(name, DbType.Int32, value);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int? value)
        {
            CreateParam(name, DbType.Int32, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name)
        {
            CreateParam(name, DbType.Decimal);
            return this;
        }
        public FluentCommand<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, DbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, DbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name)
        {
            CreateParam(name, DbType.DateTime);
            return this;
        }
        public FluentCommand<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTime2(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTime2, value);
            return this;
        }

        public FluentCommand<T> AddDateTime2(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTime2, value);
            return this;
        }

        public FluentCommand<T> AddDateTimeOffset(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTimeOffset, value);
            return this;
        }

        public FluentCommand<T> AddDateTimeOffset(string name, DateTime? value)
        {
            CreateParam(name, DbType.DateTimeOffset, value);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size)
        {
            var param = CreateParam(name, DbType.String);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            var param = CreateParam(name, DbType.String, value);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, DbType.String, value);
            return this;
        }
        public FluentCommand<T> AddString(string name)
        {
            CreateParam(name, DbType.String);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            CreateParam(name, DbType.Guid, value);
            return this;
        }
        public FluentCommand<T> AddGuid(string name)
        {
            CreateParam(name, DbType.Guid);
            return this;
        }

        public FluentCommand<T> AddGuidID(string name)
        {
            CreateParam(name, DbType.Guid, Guid.NewGuid());
            return this;
        }

        public FluentCommand<T> AddBoolean(string name)
        {
            CreateParam(name, DbType.Boolean);
            return this;
        }
        public FluentCommand<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, DbType.Boolean, value);
            return this;
        }
    }
}