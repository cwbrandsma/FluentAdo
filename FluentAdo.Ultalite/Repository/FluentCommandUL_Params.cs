using System;
using System.Data.Common;
using iAnywhere.Data.UltraLite;

namespace FluentAdo.Ultalite
{
    public partial class FluentCommand<T>
    {
        public DbParameter CreateParam(string name, ULDbType dataType, object value)
        {
            var param = new ULParameter(name, dataType);
            param.Value = SetParamValue(value); 
            _command.Parameters.Add(param);
            return param;
        }

        public DbParameter CreateParam(string name, ULDbType dataType)
        {
            var param = new ULParameter(name, dataType);
            _command.Parameters.Add(param);
            return param;
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = new ULParameter(name, value);
            _command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> Add(string name)
        {
            var param = new ULParameter();
            param.ParameterName = name;
            _command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> AddInt(string name)
        {
            CreateParam(name, ULDbType.Integer);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int value)
        {
            CreateParam(name, ULDbType.Integer, value);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int? value)
        {
            CreateParam(name, ULDbType.Integer, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name)
        {
            CreateParam(name, ULDbType.Decimal);
            return this;
        }
        public FluentCommand<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, ULDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, ULDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, ULDbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name)
        {
            CreateParam(name, ULDbType.DateTime);
            return this;
        }
        public FluentCommand<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, ULDbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size)
        {
            var param = CreateParam(name, ULDbType.VarChar);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            var param = CreateParam(name, ULDbType.VarChar, value);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, ULDbType.VarChar, value);
            return this;
        }
        public FluentCommand<T> AddString(string name)
        {
            CreateParam(name, ULDbType.VarChar);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            CreateParam(name, ULDbType.UniqueIdentifier, value);
            return this;
        }
        public FluentCommand<T> AddGuid(string name)
        {
            CreateParam(name, ULDbType.UniqueIdentifier);
            return this;
        }

        public FluentCommand<T> AddGuidID(string name)
        {
            CreateParam(name, ULDbType.UniqueIdentifier, Guid.NewGuid());
            return this;
        }

        public FluentCommand<T> AddBoolean(string name)
        {
            CreateParam(name, ULDbType.Bit);
            return this;
        }
        public FluentCommand<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, ULDbType.Bit, value);
            return this;
        }
    }
}