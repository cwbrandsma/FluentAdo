using System.Data.SqlClient;

namespace FluentAdo.SqlServer
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public static class ConnectionFactory
    {

        //NOTE: Feel free to change this line to your connection string, or connection string location
        private static string _connectionString = "Data Source=(local);Initial Catalog=thedatabase;User ID=theuser;Password=thepassword;Trusted_Connection=true;";

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            return conn;
        }
    }
}