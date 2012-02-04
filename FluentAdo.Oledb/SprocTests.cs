using System;
using System.Data;
using System.Data.OleDb;
using FluentAdo.Oledb.Repository;
using NUnit.Framework;

namespace FluentAdo.Oledb
{
    [TestFixture]
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

            Assert.AreEqual(1, cmd.GetParamValue("out1"));
            Assert.AreEqual(2, cmd.GetParamValue("out2"));
        }

        [Test] 
        public void BasicAdoCode()
        {
            var connection = ConnectionFactory.GetConnection();

            using (var cmd = new OleDbCommand("WithOutParams", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                var param1 = new OleDbParameter("out1", SqlDbType.Int);
                param1.Direction = ParameterDirection.InputOutput;
                param1.Value = 9;
                cmd.Parameters.Add(param1);

                var param2 = new OleDbParameter("out2", SqlDbType.Int);
                param2.Direction = ParameterDirection.InputOutput;
                param2.Value = 9;
                cmd.Parameters.Add(param2);

                cmd.ExecuteNonQuery();

                Assert.AreEqual(1, param1.Value);
                Assert.AreEqual(2, param2.Value);

            }

        }
    }
}