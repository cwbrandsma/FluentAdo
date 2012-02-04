using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FluentAdo.SqlServerCe.Tests.Domain;
using iAnywhere.Data.UltraLite;
using NUnit.Framework;

namespace FluentAdo.Ultalite
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

        private void SetAddCommandParameters(ULCommand cmd, Product product)
        {
            cmd.Parameters[0].Value = product.ID;
            cmd.Parameters[1].Value = product.ProductCode;
            cmd.Parameters[2].Value = product.Name;
            cmd.Parameters[3].Value = product.Manufacturer;
            cmd.Parameters[4].Value = product.Size;
            cmd.Parameters[5].Value = !product.Active;
            cmd.Parameters[6].Value = product.BarCode;
        }

        private string ConvertToUltraLiteQuery(string command)
        {
            Regex exp = new Regex(@"@\w+");
            string replacedLine = exp.Replace(command, "?");
            replacedLine = replacedLine.Replace("\n", "");
            replacedLine = replacedLine.Replace("\t", "");

            return replacedLine;
        }


        public ULCommand CreateAddProductCommand(ULConnection conn)
        {
            var cmd = new ULCommand(ConvertToUltraLiteQuery(_sql), conn);
            CreateParameters(cmd);

            return cmd;
        }
        public ULCommand CreateAddProductCommand(ULConnection conn, ULTransaction transaction)
        {
            var cmd = new ULCommand(ConvertToUltraLiteQuery(_sql), conn, transaction);
            CreateParameters(cmd);

            return cmd;
        }

        private void CreateParameters(ULCommand cmd)
        {
            cmd.Parameters.Add(new ULParameter("id", ULDbType.Integer));
            cmd.Parameters.Add(new ULParameter("code", ULDbType.VarChar, 15));
            cmd.Parameters.Add(new ULParameter("name", ULDbType.VarChar, 255));
            cmd.Parameters.Add(new ULParameter("mft", ULDbType.VarChar, 56));
            cmd.Parameters.Add(new ULParameter("size", ULDbType.VarChar, 15));
            cmd.Parameters.Add(new ULParameter("discontinued", ULDbType.Bit));
            cmd.Parameters.Add(new ULParameter("barcode", ULDbType.VarChar, 56));
        }


    }
}