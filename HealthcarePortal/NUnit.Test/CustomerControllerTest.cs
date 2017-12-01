using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOpsPortal.Repository;
using DevOpsPortal.Models;
using NUnit.Framework;

namespace DevOpsPortal.NUnit.Test
{
    [TestFixture]
    public class CustomerControllerTest
    {

        #region For CRUD Operation

        [Test]
        public void Create()
        {
            // create our test object
            CustomerRepository customer = new CustomerRepository();

            CustomerDetails CustomerDetails = GetCustomerDetails("TestNUnit", "#78, Hexaware, Bangalore", "6754389723", "6723451269", "DevOps", "BTech");

            bool dto = customer.AddCustomer(CustomerDetails);
            Assert.IsTrue(dto, "Fails To Create Customer");
        }

        CustomerDetails GetCustomerDetails(string Name, string Address, string ContactNumber, string AlternateContactNumber, string Specialty, string QualificationSummary)
        {
            return new CustomerDetails
            {
                Name = Name,
                Address = Address,
                ContactNumber = ContactNumber,
                AlternateContactNumber = AlternateContactNumber,
                Specialty = Specialty,
                QualificationSummary = QualificationSummary
            };
        }

        [Test]
        public void Update()
        {
            // create our test object
            CustomerRepository customer = new CustomerRepository();

            CustomerDetails CustomerDetails = new CustomerDetails();

            CustomerDetails.CustomerId = 105;
            CustomerDetails.Name = "TestUpdate";
            CustomerDetails.Address = "#34, Hexaware, Chennai";
            CustomerDetails.ContactNumber = "4523457892";
            CustomerDetails.AlternateContactNumber = "5634235674";
            CustomerDetails.Specialty = "Developer";
            CustomerDetails.QualificationSummary = "BTech";

            bool dto = customer.UpdateCustomer(CustomerDetails);
            Assert.IsTrue(dto, "Fails To Create Customer");
        }

        [Test]
        public void Delete()
        {
            // create our test object
            CustomerRepository customer = new CustomerRepository();

            CustomerDetails CustomerDetails = new CustomerDetails();

            CustomerDetails.CustomerId = 105;

            bool dto = customer.DeleteCustomer(CustomerDetails.CustomerId);
            Assert.IsTrue(dto, "Fails To Create Customer");
        }

        [Test]
        public void Index()
        {
            List<CustomerDetails> customer = new List<CustomerDetails>();

            customer.Add(new CustomerDetails { CustomerId = 101 });
            customer.Add(new CustomerDetails { CustomerId = 102 });
            customer.Add(new CustomerDetails { CustomerId = 104 });
            customer.Add(new CustomerDetails { CustomerId = 105 });
            customer.Add(new CustomerDetails { CustomerId = 111 });
            customer.Add(new CustomerDetails { CustomerId = 112 });

            CustomerRepository CustRepo = new CustomerRepository();

            var model = CustRepo.GetCustomers().Select(a => new { a.CustomerId });

            var customerNew = from a in customer select new { a.CustomerId };

            Assert.AreEqual(customerNew, model);
        }

        [Test]
        public void Details()
        {
            CustomerDetails customerDetails = new CustomerDetails { CustomerId = 106 };

            CustomerRepository CustRepo = new CustomerRepository();
            CustomerDetails customer = CustRepo.GetCustomers().Find(Emp => Emp.CustomerId == 101);

            Assert.AreEqual(customerDetails.CustomerId, customer.CustomerId);
        }

        #endregion

    }
}