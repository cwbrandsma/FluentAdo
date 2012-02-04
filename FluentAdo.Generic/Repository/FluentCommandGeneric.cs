using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;

namespace FluentAdo.Generic.Repository
{
    public partial class FluentCommand<T> : IDisposable
    {
        public delegate T ResultMapDelegate(DataReader reader);

        protected DbCommand _command;
        private readonly string _commandText;
        protected ResultMapDelegate _mapper;

        public FluentCommand()
        {
            _command = CreateCommand();
            _command.Connection = CreateConnection();
        }

        public FluentCommand(string commandText, DbConnection connection) 
        {
            _command = connection.CreateCommand();
            _command.Connection = connection;
            _command.CommandText = commandText;
        }

        public FluentCommand(string commandText) : this()
        {
            _command.CommandText = commandText;
            _commandText = commandText;
        }


        protected DbCommand CreateCommand()
        {
            return CreateConnection().CreateCommand();
        }

        protected DbConnection CreateConnection()
        {
            return ConnectionFactory.GetConnection();
        }

        public FluentCommand<T> SetConnection(DbConnection connection)
        {
            _command = connection.CreateCommand();
            return this;
        }

        /// <summary>
        /// Takes the function that maps the DataReader to an actual object
        /// </summary>
        /// <param name="resultMapDelegate"></param>
        /// <returns></returns>
        public FluentCommand<T> SetMap(ResultMapDelegate resultMapDelegate)
        {
            _mapper = resultMapDelegate;
            return this;
        }

        public FluentCommand<T> SetTransaction(DbTransaction transaction)
        {
            _command.Transaction = transaction;
            return this;
        }

        public FluentCommand<T> SetCommandType(CommandType commandType)
        {
            _command.CommandType = commandType;
            return this;
        }

        /// <summary>
        /// Returns the result in a typed List
        /// </summary>
        /// <returns></returns>
        public List<T> AsList()
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            Debug.WriteLine("Executing SQL: " + _commandText);
            var returnList = new List<T>();
            using (_command)
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    var myReader = new DataReader(reader);
                    while (reader.Read())
                    {
                        T data = _mapper.Invoke(myReader);
                        returnList.Add(data);
                    }
                }
            return returnList;
        }

        /// <summary>
        /// Returns the result as a typed IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> AsEnumerable()
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            Debug.WriteLine("Executing SQL: " + _commandText);

            using (_command)
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    var myReader = new DataReader(reader);
                    while (reader.Read())
                    {
                        T data = _mapper.Invoke(myReader);
                        yield return data;
                    }
                }
        }

        public IList<T> AsList(ResultMapDelegate map)
        {
            _mapper = map;
            return AsList();
        }

        public IEnumerable<T> AsEnumerable(ResultMapDelegate map)
        {
            _mapper = map;
            return AsEnumerable();
        }


        /// <summary>
        /// Returns the result in a DataReader
        /// </summary>
        /// <returns></returns>
        public DbDataReader AsDataReader()
        {
            Debug.WriteLine("Executing SQL: " + _commandText);
            using (_command)
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    return reader;
                }
        }

        /// <summary>
        /// Returns the command results as a Scalar value
        /// </summary>
        /// <returns></returns>
        public T AsScalar()
        {
            Debug.WriteLine("Executing SQL: " + _commandText);
            using (_command)
            {
                return (T)_command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the query using ExecuteScaler.  
        /// This version of AsScalar does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.  
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public T AsScalar(params object[] list)
        {
            ResetParams(list);
            return (T)_command.ExecuteScalar();
        }

        /// <summary>
        /// Returns the first result
        /// </summary>
        /// <returns></returns>
        public T AsSingle()
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            Debug.WriteLine("Executing SQL: " + _commandText);

            using (_command)
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    var myReader = new DataReader(reader);
                    if (reader.Read())
                    {
                        return _mapper.Invoke(myReader);
                    }
                }
            return default(T);
        }

        /// <summary>
        /// Executes the command and returns the first (and only the first) result.   
        /// This version of AsSingle does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.  
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public T AsSingle(params object[] list)
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            ResetParams(list);
            Debug.WriteLine("Executing SQL: " + _commandText);

            using (DbDataReader reader = _command.ExecuteReader())
            {
                var myReader = new DataReader(reader);
                if (reader.Read())
                {
                    T data = _mapper.Invoke(myReader);
                    return data;
                }
            }
            return default(T);
        }


        /// <summary>
        /// Executes the query as a NonQuery
        /// </summary>
        /// <returns></returns>
        public int AsNonQuery()
        {
            Debug.WriteLine("Executing SQL: " + _commandText);
            using (_command)
            {
                return _command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the query as a NonQuery. This is typically used for UPDATE, INSERT, and DELETE statements.
        /// This version of AsNonQuery does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list">New parameter values.</param>
        /// <returns></returns>
        public int AsNonQuery(params object[] list)
        {
            //            Debug.WriteLine("Executing SQL: " + _commandText);

            ResetParams(list);
            return _command.ExecuteNonQuery();
        }


        /// <summary>
        /// ResetParams is to be used with AsNonQuery and AsScalar to reset the command parameter values.
        /// The values must be passed in the same order that they were created.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void ResetParams(params object[] list)
        {
            Debug.Assert(list.Length == _command.Parameters.Count);
            for (int i = 0; i < list.Length; i++)
            {
                _command.Parameters[i].Value = SetParamValue(list[i]);
            }
        }


        /// <summary>
        /// SetParamValue is used when setting parameter values 
        /// and you want to ensure that a null value is set to DBNull.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object SetParamValue(object value)
        {
            return value ?? DBNull.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object SetGuidParamValue(Guid value)
        {
            if (value == Guid.Empty)
                return DBNull.Value;
            return value;
        }

        public void Dispose()
        {
            _command.Dispose();
        }

    }
}