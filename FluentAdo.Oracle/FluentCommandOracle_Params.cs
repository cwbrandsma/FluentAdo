using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace FluentAdo.Oracle
{
    public partial class FluentCommand<T>
    {
        public DbParameter CreateParam(string name, OracleType dataType, object value)
        {
            var param = new OracleParameter(name, dataType);
            param.Value = SetParamValue(value); 
            _command.Parameters.Add(param);
            return param;
        }

        public DbParameter CreateParam(string name, OracleType dataType)
        {
            var param = new OracleParameter(name, dataType);
            _command.Parameters.Add(param);
            return param;
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = new OracleParameter(name, value);
            _command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int value)
        {
            CreateParam(name, OracleType.Int32, value);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int? value)
        {
            CreateParam(name, OracleType.Int32, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, OracleType.Number, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, OracleType.Number, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, OracleType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, OracleType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            CreateParam(name, OracleType.NVarChar, size);
            return this;
        }

        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, OracleType.NVarChar, value);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            var param = CreateParam(name, OracleType.Char);
            param.Value = SetGuidParamValue(value);
            return this;
        }

        public FluentCommand<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, OracleType.Int16, value);
            return this;
        }


    }
}