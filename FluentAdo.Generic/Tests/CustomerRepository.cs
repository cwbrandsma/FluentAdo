using System;
using System.Collections.Generic;
using System.Data.Common;
using FluentAdo.Generic.Repository;
using FluentAdo.Generic.Tests.Domain;

namespace FluentAdo.Generic.Tests
{
    public class CustomerRepository
    {
        public void Clear()
        {
            const string sql = "DELETE FROM Customer";
            new FluentCommand<Customer>(sql)
                .AsNonQuery();
        }

        public void AddCustomer(Customer customer)
        {
            customer.ID = Guid.NewGuid();
            const string sql = @"INSERT INTO Customer
(ID, FirstName, LastName, BirthDay, Height, Weight)
VALUES (@id,@firstName,@lastName,@birthday,@height,@weight)";

            new FluentCommand<string>(sql)
                .AddGuid("id", customer.ID)
                .AddString("firstName", customer.FirstName)
                .AddString("lastName", customer.LastName)
                .AddDateTime("birthday", customer.Birthday)
                .AddDecimal("height", customer.Height)
                .AddInt("weight", customer.Weight)
                .AsNonQuery();
        }

        public void AddCustomerList(List<Customer> list)
        {
            const string sql = @"INSERT INTO Customer
(ID, FirstName, LastName, BirthDay, Height, Weight)
VALUES (@id,@firstName,@lastName,@birthday,@height,@weight)";

            var cmd = new FluentCommand<string>(sql)
                .AddGuid("id")
                .AddString("firstName")
                .AddString("lastName")
                .AddDateTime("birthday")
                .AddDecimal("height")
                .AddInt("weight");

            foreach (var customer in list)
            {
                customer.ID = Guid.NewGuid();
                cmd.AsNonQuery(customer.ID, 
                               customer.FirstName, 
                               customer.LastName, 
                               customer.Birthday, 
                               customer.Height,
                               customer.Weight);
            }
        }
 

        public void DeleteCustomer(Customer customer)
        {
            const string sql = "DELETE FROM Customer WHERE ID = @id";
            new FluentCommand<Customer>(sql)
                .AddGuid("id", customer.ID)
                .AsNonQuery();
        }

        public IList<Customer> GetList()
        {
            const string sql = 
                @"SELECT ID, FirstName, LastName,  Height, Weight, Birthday FROM Customer";
            return new FluentCommand<Customer>(sql)
                .SetMap(Customer_Map)
                .AsList();
        }

        public IList<Customer> GetCustomerByWeight(int weight)
        {
            const string sql =
                @"SELECT ID, FirstName, LastName, BirthDay, Height, Weight FROM Customer WHERE Weight > @weight";
            return new FluentCommand<Customer>(sql)
                .AddInt("weight", weight)
                .SetMap(Customer_Map)
                .AsList();
        }

        private Customer Customer_Map(DataReader reader)
        {
            return new Customer
            {
                ID = reader.GetGuid("ID"),
                FirstName = reader.GetString("FirstName"),
                LastName = reader.GetString("LastName"),
                Birthday = reader.GetDateTime("Birthday"),
                Height = reader.GetIntNullable("Height"),
                Weight = reader.GetIntNullable("Weight")
            };
        }

        public IEnumerable<Customer> GetEnumerable()
        {
            const string sql = "SELECT ID, FirstName, LastName, BirthDay, Height, Weight FROM Customer";
            return new FluentCommand<Customer>(sql)
                .SetMap(Customer_Map)
                .AsEnumerable();
        }

        public DbDataReader GetDataReader()
        {
            const string sql = "SELECT ID, FirstName, LastName, BirthDay, Height, Weight FROM Customer";
            return new FluentCommand<Customer>(sql)
                .SetMap(Customer_Map)
                .AsDataReader();
        }

        public Customer ByID(Guid id)
        {
            const string sql = "SELECT ID, FirstName, LastName, BirthDay, Height, Weight FROM Customer WHERE ID = @id";
            return new FluentCommand<Customer>(sql)
                .AddGuid("id", id)
                .SetMap(reader => new Customer
                {
                    ID = reader.GetGuid("ID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName"),
                    Birthday = reader.GetDateTime("Birthday"),
                    Height = reader.GetIntNullable("Height"),
                    Weight = reader.GetIntNullable("Weight")
                }
                )
                .AsSingle();
        }

        public IList<Customer> JustIdAndName()
        {
            const string sql = "SELECT ID, FirstName, LastName FROM Customer";
            return new FluentCommand<Customer>(sql)
                .SetMap(reader => new Customer
                {
                    ID = reader.GetGuid("ID"),
                    FirstName = reader.GetString("FirstName"),
                    LastName = reader.GetString("LastName")
                })
                .AsList();
        }

        public IList<Customer> MethodWithoutSetMap()
        {
            const string sql = "SELECT ID, FirstName, LastName FROM Customer";
            return new FluentCommand<Customer>(sql)
                .AsList();
        }

    }
}