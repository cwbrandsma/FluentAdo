using System.Data.OleDb;

namespace FluentAdo.Oledb
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public class ConnectionFactory
    {
        private static ConnectionFactory instance;

        private static string _connectionString = @"Provider=SQLOLEDB.1;Data Source=.\sqlserverexpress;Integrated Security=SSPI;Persist Security Info=False;Initial File Name=|DataDirectory|\OleDbTests.mdf;";

        private readonly OleDbConnection _connection;

        private ConnectionFactory()
        {
            _connection = new OleDbConnection(_connectionString);
            _connection.Open();
        }

        public static OleDbConnection GetConnection()
        {
            if (instance == null)
            {
                instance = new ConnectionFactory();
            }
            return instance._connection;
        }
    }
}