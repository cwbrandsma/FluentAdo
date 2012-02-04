using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
//using System.Linq;
using System.Data.SQLite;

namespace FluentAdo.SQLite
{
    public partial class FluentCommand<T> :IDisposable
    {
        public delegate T ResultMapDelegate(DataReader reader);

        protected readonly DbCommand _command;
        private readonly string _commandText;
        protected ResultMapDelegate _mapper;


                /// <summary>
        /// Creates a new FluentCommand
        /// </summary>
        private FluentCommand()
        {
            _command = CreateCommand();
            _command.Connection = CreateConnection();
        }

        public FluentCommand(string commandText): this()
        {
            _command.CommandText = commandText;
            _commandText = commandText;
        }

        public FluentCommand(string commandText, DbConnection connection)
        {
            _command = CreateCommand();
            _command.Connection = connection;
            _command.CommandText = commandText;
            _commandText = commandText;
        }

        public FluentCommand<T> SetConnection(DbConnection connection)
        {
            _command.Connection = connection;
            return this;
        }


        protected DbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        protected DbConnection CreateConnection()
        {
            return ConnectionFactory.GetConnection();
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

        /// <summary>
        /// Sets the command text for the internal command object
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public FluentCommand<T> SetCommandText(string cmd)
        {
            _command.CommandText = cmd;
            return this;
        }

        public FluentCommand<T> SetCommandType(CommandType value)
        {
            _command.CommandType = value;
            return this;
        }

        /// <summary>
        /// Returns the result in a typed List.  The values are serialized using the SetMap method, which must be set 
        /// to call the AsList method.
        /// </summary>
        /// <returns></returns>
        public IList<T> AsList()
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            Debug.WriteLine("Executing SQL: " + _commandText);
            List<T> returnList = new List<T>();
            using (_command)
            {
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    var myReader = new DataReader(reader);
                    while (reader.Read())
                    {
                        T data = _mapper.Invoke(myReader);
                        returnList.Add(data);
                    }
                }
            }

            return returnList;
        }

        public IList<T> AsList(ResultMapDelegate map)
        {
            _mapper = map;
            return AsList();
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
            {
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
        }

        /// <summary>
        /// Executes the command and returns all of the results
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public IEnumerable<T> AsEnumerable(ResultMapDelegate map)
        {
            _mapper = map;
            return AsEnumerable();
        }

        /// <summary>
        /// Executes the command returns the first result
        /// </summary>
        /// <returns></returns>
        public T AsSingle()
        {
            if (_mapper == null)
                throw new NullReferenceException("You must call SetMap to use AsList");

            Debug.WriteLine("Executing SQL: " + _commandText);
            using (_command)
            {
                using (DbDataReader reader = _command.ExecuteReader())
                {
                    var myReader = new DataReader(reader);
                    if (reader.Read())
                    {
                        T data = _mapper.Invoke(myReader);
                        return data;
                    }
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
        public T AsSingle(ResultMapDelegate map)
        {
            _mapper = map;
            return AsSingle();
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
        /// Returns the result in a DataReader
        /// </summary>
        /// <returns></returns>
        public DbDataReader AsDataReader()
        {
            Debug.WriteLine("Executing SQL: " + _commandText);
            using (DbDataReader reader = _command.ExecuteReader())
            {
                return reader;
            }
        }

        /// <summary>
        /// Executes the query using ExecuteScaler.
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
        /// Executes the query as a NonQuery.  This is typically used for UPDATE, INSERT, and DELETE statements.
        /// </summary>
        /// <returns></returns>
        public int AsNonQuery()
        {
            Debug.WriteLine("Executing SQL: " + _commandText);
            using(_command)
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
        protected object SetGuidParamValue(object value)
        {
            if (value == null)
                return DBNull.Value;
            return value.ToString();
        }

        public void Dispose()
        {
            _command.Dispose();
        }
    }
}