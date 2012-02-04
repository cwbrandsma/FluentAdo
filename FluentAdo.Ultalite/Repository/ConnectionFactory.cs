using iAnywhere.Data.UltraLite;

namespace FluentAdo.Ultalite
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public class ConnectionFactory
    {
        private static ConnectionFactory instance;

        internal string _connectionString = "TestDB.udb";

        private readonly ULConnection _connection;

        private ConnectionFactory()
        {
//            var connStr = _connectionString.Replace(@"file:\", "");
            ULConnectionParms parms = new ULConnectionParms();
            parms.DatabaseOnDesktop = _connectionString;
            _connection = new ULConnection(parms.ToString());
            _connection.Open();
        }

        public static ULConnection GetConnection()
        {
            if (instance == null)
            {
                instance = new ConnectionFactory();
            }
            return instance._connection;
        }
    }
}