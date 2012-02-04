using iAnywhere.Data.SQLAnywhere;

namespace FluentAdo.IAnywhere
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public static class ConnectionFactory
    {
        //NOTE: Feel free to change this line to your connection string, or connection string location
        private static string _connectionString = @"UID=dba;PWD=sql;DBN=TestDb;ASTOP=NO;DBF=C:\data\TestDb.db;";
        public static SAConnection GetConnection()
        {
            var conn = new SAConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}