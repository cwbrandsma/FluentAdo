using System.Data.SqlServerCe;

namespace FluentAdo.SqlServerCe
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public class ConnectionFactory
    {
        private static ConnectionFactory instance;

        internal string _connectionString = "Data Source=" +
                                            System.IO.Path.GetDirectoryName(
                                                System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) +
                                            "\\TestDB.sdf;Persist Security Info=False;";

        private readonly SqlCeConnection _connection;

        private ConnectionFactory()
        {
            var connStr = _connectionString.Replace(@"file:\", "");
            _connection = new SqlCeConnection(connStr);
            _connection.Open();
        }

        public static SqlCeConnection GetConnection()
        {
            if (instance == null)
            {
                instance = new ConnectionFactory();
            }
            return instance._connection;
        }
    }
}