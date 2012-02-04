using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using FluentAdo.SqlServerCe.Tests.Domain;
using NUnit.Framework;

namespace FluentAdo.SqlServerCe.Tests
{
    [TestFixture]
    public class InsertPerformanceTests
    {
        private List<Product> _productList;

        private const string _sql = @"INSERT INTO Product (id, Code, Name, Manufacturer, Size, Discontinued, Barcode) 
VALUES (@id, @code, @name, @mft, @size, @discontinued, @barcode)";


        public InsertPerformanceTests()
        {
            _productList = CreateProductList();
        }
        
        [SetUp]
        public void SetUp()
        {
            new FluentCommand<int>("DELETE FROM Product").AsNonQuery();
        }


        private List<Product> CreateProductList()
        {
            const int ProductCount = 2000;
            var list = new List<Product>(ProductCount);

            for (int i = 0; i < ProductCount; i++)
            {
                var cust = new Product { ID = i, Name = "TestProduct", Active = true, Manufacturer = "TestMfg", BarCode = i.ToString(), ProductCode = i.ToString(), Size = "Cup" };
                list.Add(cust);
            }

            return list;
        }

        private void SaveProduct(Product product)
        {
            new FluentCommand<Product>(_sql)
                .AddInt("id", product.ID)
                .AddString("code", product.ProductCode)
                .AddString("name", product.Name)
                .AddString("mft", product.Manufacturer)
                .AddString("size", product.Size)
                .AddBoolean("discontinued", !product.Active)
                .AddString("barcode", product.BarCode)
                .AsNonQuery();

        }

        [Test]
        public void FluentCommand()
        {
            foreach (var product in _productList)
            {
                new FluentCommand<Product>(_sql)
                    .AddInt("id", product.ID)
                    .AddString("code", product.ProductCode)
                    .AddString("name", product.Name)
                    .AddString("mft", product.Manufacturer)
                    .AddString("size", product.Size)
                    .AddBoolean("discontinued", !product.Active)
                    .AddString("barcode", product.BarCode)
                    .AsNonQuery();
            }
        }

        [Test]
        public void FluentCommandKeepAlive()
        {
            using (FluentCommand<int> cmd = new FluentCommand<int>(_sql))
            {
                cmd.AddInt("id")
                    .AddString("code", 15)
                    .AddString("name", 255)
                    .AddString("mft", 56)
                    .AddString("size")
                    .AddBoolean("discontinued")
                    .AddString("barcode", 56);

                for (int i = 0; i < _productList.Count; i++)
                {
                    var product = _productList[i];
                    cmd.AsNonQuery(
                        product.ID, product.ProductCode, product.Name, product.Manufacturer, product.Size,
                        !product.Active, product.ProductCode
                        );
                }
            }
        }

        [Test]
        public void FluentCommandKeepAliveWithTransaction()
        {
            var conn = ConnectionFactory.GetConnection();
            using (var trans = conn.BeginTransaction())
            {
                using (FluentCommand<int> cmd = new FluentCommand<int>(_sql))
                {
                    cmd.SetTransaction(trans)
                        .AddInt("id")
                        .AddString("code", 15)
                        .AddString("name", 255)
                        .AddString("mft", 56)
                        .AddString("size")
                        .AddBoolean("discontinued")
                        .AddString("barcode", 56);

                    for (int i = 0; i < _productList.Count; i++)
                    {
                        var product = _productList[i];
                        cmd.AsNonQuery(
                            product.ID, product.ProductCode, product.Name, product.Manufacturer, product.Size,
                            !product.Active, product.ProductCode);
                    }
                }
                trans.Commit();
            }
        }

        [Test]
        public void FluentCommandWithTransaction()
        {
            var conn = ConnectionFactory.GetConnection();
            using (var transaction = conn.BeginTransaction())
            {
                foreach (var product in _productList)
                {
                    new FluentCommand<Product>(_sql)
                        .SetTransaction(transaction)
                        .AddInt("id", product.ID)
                        .AddString("code", product.ProductCode)
                        .AddString("name", product.Name)
                        .AddString("mft", product.Manufacturer)
                        .AddString("size", product.Size)
                        .AddBoolean("discontinued", !product.Active)
                        .AddString("barcode", product.BarCode)
                        .AsNonQuery();
                }
                transaction.Commit();
            }
        }


        [Test]
        public void OneCommand()
        {
            var connection = ConnectionFactory.GetConnection();
            using (var cmd = CreateAddProductCommand(connection))
            {
                for (int i = 0; i < _productList.Count; i++)
                {
                    var product = _productList[i];
                    SetAddCommandParameters(cmd, product);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        [Test]
        public void OneCommandAndATransaction()
        {
            var connection = ConnectionFactory.GetConnection();
            {
                using (var transaction = connection.BeginTransaction())
                {
                    using (var cmd = CreateAddProductCommand(connection, transaction))
                    {
                        for (int i = 0; i < _productList.Count; i++)
                        {
                            var product = _productList[i];
                            SetAddCommandParameters(cmd, product);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
            }
        }
        [Test]
        public void OneCommandAndATransactionAndDroppedIndexes()
        {
            var connection = ConnectionFactory.GetConnection();
            {
                DropProductIndexes(connection);
                using (var transaction = connection.BeginTransaction())
                {
                    using (var cmd = CreateAddProductCommand(connection, transaction))
                    {
                        for (int i = 0; i < _productList.Count; i++)
                        {
                            var product = _productList[i];
                            SetAddCommandParameters(cmd, product);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                CreateProductIndexes(connection);
            }
        }


        private void SetAddCommandParameters(SqlCeCommand cmd, Product product)
        {
            cmd.Parameters[0].Value = product.ID;
            cmd.Parameters[1].Value = product.ProductCode;
            cmd.Parameters[2].Value = product.Name;
            cmd.Parameters[3].Value = product.Manufacturer;
            cmd.Parameters[4].Value = product.Size;
            cmd.Parameters[5].Value = !product.Active;
            cmd.Parameters[6].Value = product.BarCode;
        }

        public SqlCeCommand CreateAddProductCommand(SqlCeConnection conn)
        {
            var cmd = new SqlCeCommand(_sql, conn);
            CreateParameters(cmd);

            return cmd;
        }
        public SqlCeCommand CreateAddProductCommand(SqlCeConnection conn, SqlCeTransaction transaction)
        {
            var cmd = new SqlCeCommand(_sql, conn, transaction);
            CreateParameters(cmd);

            return cmd;
        }

        private void CreateParameters(SqlCeCommand cmd)
        {
            cmd.Parameters.Add(new SqlCeParameter("id", SqlDbType.Int));
            cmd.Parameters.Add(new SqlCeParameter("code", SqlDbType.NVarChar, 15));
            cmd.Parameters.Add(new SqlCeParameter("name", SqlDbType.NVarChar, 255));
            cmd.Parameters.Add(new SqlCeParameter("mft", SqlDbType.NVarChar, 56));
            cmd.Parameters.Add(new SqlCeParameter("size", SqlDbType.NVarChar, 15));
            cmd.Parameters.Add(new SqlCeParameter("discontinued", SqlDbType.Bit));
            cmd.Parameters.Add(new SqlCeParameter("barcode", SqlDbType.NVarChar, 56));
        }

        public void CreateProductIndexes(SqlCeConnection connection)
        {
            return;

            const string sqlBarcode = "CREATE INDEX Barcode ON Product (Barcode)";
            new FluentCommand<int>(sqlBarcode, connection).AsNonQuery();

            const string sqlCode = "CREATE INDEX Product_Code ON Product (Code)";
            new FluentCommand<int>(sqlCode, connection).AsNonQuery();

            const string sqlName = "CREATE INDEX ProductName ON Product (Name)";
            new FluentCommand<int>(sqlName, connection).AsNonQuery();

            const string sqlBasket = "CREATE INDEX BasketProduct ON Product (Discontinued)";
            new FluentCommand<int>(sqlBasket, connection).AsNonQuery();

            const string sqlPk = "CREATE UNIQUE INDEX Product__PK ON Product (ID)";
            new FluentCommand<int>(sqlPk, connection);

        }

        public void DropProductIndexes(SqlCeConnection connection)
        {
            return;
            const string sql = "DROP INDEX [Product].Barcode";
            new FluentCommand<int>(sql, connection).AsNonQuery();

            const string sqlCode = "DROP INDEX [Product].Product_Code";
            new FluentCommand<int>(sqlCode, connection).AsNonQuery();

            const string sqlBasket = "DROP INDEX [Product].BasketProduct";
            new FluentCommand<int>(sqlBasket, connection).AsNonQuery();

            const string sqlName = "DROP INDEX [Product].ProductName";
            new FluentCommand<int>(sqlName, connection).AsNonQuery();

            const string sqlPk = "DROP INDEX [Product].PK__Product__000000000000012A";
            new FluentCommand<int>(sqlPk, connection);
        }

    }
}