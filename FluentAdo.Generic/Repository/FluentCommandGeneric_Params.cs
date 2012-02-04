using System;
using System.Data;
using System.Data.Common;

namespace FluentAdo.Generic.Repository
{
    public partial class FluentCommand<T> 
    {
        public FluentCommand<T> SetDirection(ParameterDirection direction)
        {
            if (_command.Parameters.Count == 0) return this;

            DbParameter parameter = _command.Parameters[_command.Parameters.Count - 1];
            parameter.Direction = direction;

            return this;
        }

        public object GetParamValue(string paramName)
        {
            return _command.Parameters[paramName].Value;
        }

        public DbParameter CreateParam(string name, DbType dataType, object value)
        {
            var param = _command.CreateParameter();
            param.ParameterName = name;
            param.DbType = dataType;
            param.Value = SetParamValue(value); 
            _command.Parameters.Add(param);
            return param;
        }

        public DbParameter CreateParam(string name, DbType dataType)
        {
            var param = _command.CreateParameter();
            param.ParameterName = name;
            param.DbType = dataType;
            _command.Parameters.Add(param);
            return param;
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = _command.CreateParameter();
            param.ParameterName = name;
            param.Value = value;
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


        public FluentCommand<T> AddDateTime(string name)
        {
            CreateParam(name, DbType.DateTime);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, DbType.DateTime, value);
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

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            CreateParam(name, DbType.String, size);
            return this;
        }

        public FluentCommand<T> AddString(string name)
        {
            CreateParam(name, DbType.String);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size)
        {
            var param = CreateParam(name, DbType.String);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, DbType.String, value);
            return this;
        }

        public FluentCommand<T> AddGuid(string name)
        {
            CreateParam(name, DbType.Guid);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            var param = CreateParam(name, DbType.Guid);
            param.Value = SetGuidParamValue(value);
            return this;
        }
        public FluentCommand<T> AddGuidID(string name)
        {
            var param = CreateParam(name, DbType.Guid);
            param.Value = SetGuidParamValue(Guid.NewGuid());
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