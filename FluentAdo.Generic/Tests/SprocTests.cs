using System;
using System.Data;
using System.Data.SqlClient;
using FluentAdo.Generic.Repository;
using NUnit.Framework;

namespace FluentAdo.Generic.Tests
{
    [TestFixture][Ignore]
    public class SprocTests
    {

        [Test]
        public void FluentExecute()
        {
            var cmd = new FluentCommand<string>("WithOutParams")
                .SetCommandType(CommandType.StoredProcedure)
                .Add("out1", 9).SetDirection(ParameterDirection.InputOutput)
                .Add("out2", 9).SetDirection(ParameterDirection.InputOutput);
            cmd.AsNonQuery();

            Console.WriteLine(cmd.GetParamValue("out1"));
            Console.WriteLine(cmd.GetParamValue("out2"));

                
        }

        [Test] 
        public void BasicAdoCode()
        {
            var connection = ConnectionFactory.GetConnection();

            using (var cmd = new SqlCommand("WithOutParams", (SqlConnection) connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter param1 = new SqlParameter("out1", SqlDbType.Int);
                param1.Direction = ParameterDirection.InputOutput;
                param1.Value = 9;
                cmd.Parameters.Add(param1);
                
                SqlParameter param2 = new SqlParameter("out2", SqlDbType.Int);
                param2.Direction = ParameterDirection.InputOutput;
                param2.Value = 9;
                cmd.Parameters.Add(param2);

                cmd.ExecuteNonQuery();

                Console.WriteLine(param1.Value);
                Console.WriteLine(param2.Value);

            }

        }
    }
}