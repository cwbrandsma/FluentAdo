using System;
using System.Data;
using iAnywhere.Data.SQLAnywhere;
using NUnit.Framework;

namespace FluentAdo.IAnywhere
{
    [TestFixture]
    public class SprocTests
    {

        [Test]
        public void FluentExecute()
        {
            var cmd = new FluentCommand<string>("WithOutParams")
                .SetCommandType(CommandType.StoredProcedure)
                .AddInt("out1", 9).SetDirection(ParameterDirection.InputOutput)
                .AddInt("out2", 9).SetDirection(ParameterDirection.InputOutput);
            cmd.AsNonQuery();

            Console.WriteLine(cmd.GetParamValue("out1"));
            Console.WriteLine(cmd.GetParamValue("out2"));
        }

        [Test] 
        public void BasicAdoCode()
        {
            var connection = ConnectionFactory.GetConnection();

            using (var cmd = new SACommand("WithOutParams", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                SAParameter param1 = new SAParameter("out1", SADbType.Integer);
                param1.Direction = ParameterDirection.InputOutput;
                param1.Value = 9;
                cmd.Parameters.Add(param1);

                SAParameter param2 = new SAParameter("out2", SADbType.Integer);
                param1.Direction = ParameterDirection.InputOutput;
                param2.Value = 9;
                cmd.Parameters.Add(param2);

                cmd.ExecuteNonQuery();

                Console.WriteLine(param1.Value);
                Console.WriteLine(param2.Value);

            }

        }
    }
}