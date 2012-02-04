using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace FluentAdo.Oledb.Repository
{
    public partial class FluentCommand<T>
    {
        private Dictionary<string, DbParameter> _paramList = new Dictionary<string, DbParameter>();

        public FluentCommand<T> SetDirection(ParameterDirection direction)
        {
            if (_command.Parameters.Count == 0) return this;

            DbParameter parameter = _command.Parameters[_command.Parameters.Count - 1];
            parameter.Direction = direction;

            return this;
        }

        public object GetParamValue(string paramName)
        {
//            return _command.Parameters[paramName].Value;
            return _paramList[paramName].Value;

        }

        private void AddParam(string name, OleDbParameter parameter)
        {
            _command.Parameters.Add(parameter);
            _paramList.Add(name, parameter);
        }

        public DbParameter CreateParam(string name, OleDbType dataType, object value)
        {
            var param = new OleDbParameter(name, dataType);
            param.Value = SetParamValue(value);
            AddParam(name, param);
            return param;
        }

        public DbParameter CreateParam(string name, OleDbType dataType)
        {
            var param = new OleDbParameter(name, dataType);
            AddParam(name, param);
            return param;
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = new OleDbParameter(name, value);
            AddParam(name, param);
            return this;
        }

        public FluentCommand<T> Add(string name)
        {
            var param = new OleDbParameter();
            param.ParameterName = name;
            AddParam(name, param);
            return this;
        }

        public FluentCommand<T> AddInt(string name)
        {
            CreateParam(name, OleDbType.Integer);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int value)
        {
            CreateParam(name, OleDbType.Integer, value);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int? value)
        {
            CreateParam(name, OleDbType.Integer, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name)
        {
            CreateParam(name, OleDbType.Decimal);
            return this;
        }
        public FluentCommand<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, OleDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, OleDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, OleDbType.Date, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name)
        {
            CreateParam(name, OleDbType.Date);
            return this;
        }
        public FluentCommand<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, OleDbType.Date, value);
            return this;
        }


        public FluentCommand<T> AddString(string name, int size)
        {
            var param = CreateParam(name, OleDbType.VarWChar);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            var param = CreateParam(name, OleDbType.VarWChar, value);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, OleDbType.VarWChar, value);
            return this;
        }
        public FluentCommand<T> AddString(string name)
        {
            CreateParam(name, OleDbType.VarWChar);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            CreateParam(name, OleDbType.Guid, value);
            return this;
        }
        public FluentCommand<T> AddGuid(string name)
        {
            CreateParam(name, OleDbType.Guid);
            return this;
        }

        public FluentCommand<T> AddGuidID(string name)
        {
            CreateParam(name, OleDbType.Guid, Guid.NewGuid());
            return this;
        }

        public FluentCommand<T> AddBoolean(string name)
        {
            CreateParam(name, OleDbType.Boolean);
            return this;
        }
        public FluentCommand<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, OleDbType.Boolean, value);
            return this;
        }

        public DbParameter CreateParam(string name, SqlDbType dataType, byte precision, object value)
        {
            var param = new OleDbParameter(name, dataType);
            param.Precision = precision;
            param.Value = SetParamValue(value);
            _command.Parameters.Add(param);

            return param;
        }

        public DbParameter CreateParam(string name, SqlDbType dataType, byte precision)
        {
            var param = new OleDbParameter(name, dataType);
            param.Precision = precision;
            _command.Parameters.Add(param);
            return param;
        }

        public FluentCommand<T> AddDecimal(string name, byte precision)
        {
            CreateParam(name, SqlDbType.Decimal, precision);
            return this;
        }
        public FluentCommand<T> AddDecimal(string name, decimal value, byte precision)
        {
            CreateParam(name, SqlDbType.Decimal, precision, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, byte precision, decimal? value)
        {
            CreateParam(name, SqlDbType.Decimal, precision, value);
            return this;
        }

    }
}