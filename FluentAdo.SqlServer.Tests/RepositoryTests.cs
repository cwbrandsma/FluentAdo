using System;
using System.Collections.Generic;
using System.Linq;
using FluentAdo.SqlServer.Tests.Domain;
using NUnit.Framework;

namespace FluentAdo.SqlServer.Tests
{
    [TestFixture] //[Ignore("I use a non standard SQL Express installation.  These tests will fail if you dont change the connection string.")]
    public class RepositoryTests
    {
        private CustomerRepository _dbo;

        [SetUp]
        public void SetUp()
        {
            _dbo = new CustomerRepository();
            _dbo.Clear();
        }

        [Test]
        public void Add_Test()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris", 
                           LastName = "Brandsma", 
                           Birthday = new DateTime(1975, 4, 2)
                       };

            _dbo.AddCustomer(cust);

            Assert.IsNotNull(cust.ID);
        }

        [Test]
        public void AddFullCustomer_Test()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2),
                           Height = new decimal(6.2),
                           Weight = 210
                       };

            _dbo.AddCustomer(cust);

            Assert.IsNotNull(cust.ID);
        }


        [Test]
        public void AddListOfCustomers()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2),
                           Height = new decimal(6.2),
                           Weight = 210
                       };
            var list = new List<Customer>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(cust);
            }

            _dbo.AddCustomerList(list);
        }

        [Test]
        public void List_Test()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2)
                       };

            _dbo.AddCustomer(cust);
            _dbo.AddCustomer(cust);
            _dbo.AddCustomer(cust);

            var list = _dbo.GetList();
            Assert.AreEqual(3, list.Count);
        }
        [Test]
        public void Enumerator_Test()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2)
                       };

            _dbo.AddCustomer(cust);
            _dbo.AddCustomer(cust);
            _dbo.AddCustomer(cust);

            var list = _dbo.GetEnumerable();
            Assert.AreEqual(3, list.Count());
        }
        
        [Test]
        public void GetById_Test()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2)
                       };

            _dbo.AddCustomer(cust);

            var newCust = _dbo.ByID(cust.ID);
            Assert.AreEqual(cust.ID, newCust.ID);
            Assert.AreEqual(cust.FirstName, newCust.FirstName);
            Assert.AreEqual(cust.LastName , newCust.LastName);
        }

        [Test]
        public void DeleteTest()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2)
                       };

            _dbo.AddCustomer(cust);
            Assert.AreEqual(1, _dbo.GetList().Count);
            _dbo.DeleteCustomer(cust);
            Assert.AreEqual(0, _dbo.GetList().Count);
        }
        
        [Test]
        public void JustIdAndNameTest()
        {
            var cust = new Customer
                       {
                           FirstName = "Chris",
                           LastName = "Brandsma",
                           Birthday = new DateTime(1975, 4, 2),
                           Height = new decimal(6.2),
                           Weight = 210
                       };

            _dbo.AddCustomer(cust);

            var list = _dbo.JustIdAndName();

            Assert.AreEqual(1, list.Count);

        }

        [Test]
        public void SetMapNotSet_SoExceptionShouldBeThrown()
        {
            try
            {
                _dbo.MethodWithoutSetMap();
            }
            catch (NullReferenceException e)
            {
                return;
            }

            Assert.Fail("A exception should have been thrown");
        }
    }
}