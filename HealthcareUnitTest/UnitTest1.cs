using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevOpsPortal.Repository;
using DevOpsPortal.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MSunitTest
{
    [TestClass]
    public class UnitTest2
    {


        [TestMethod]
        public void TestCreateMethod_Moq()
        {
            //-- 1. Arrange ----------------------
            CustomerDetails CustomerDetails = GetCustomerDetails("Arghya Das", "Ramaniyam, Chennai, 603103", "9054389723", "8123451269", "Pulmonologist", "MBBS");

            //-- Creating a fake ICustomerRepository object
            var repositoryMock = new Mock<ICustomerRepositoryMoq>();
            //-- Setting up the repository
            repositoryMock.Setup(m => m.AddCustomer(CustomerDetails)).Returns(true); //mocked to true to always pass

            var service = new CustomerService(repositoryMock.Object);

            //-- 2. Act ----------------------
            var result = service.AddCustomer(CustomerDetails);
            Console.WriteLine(result);


            //-- Assert ---------------------
            Assert.IsTrue(result, "Fails To add Customer");
            repositoryMock.Verify(m => m.AddCustomer(CustomerDetails), Times.Once());

        }

        CustomerDetails GetCustomerDetails(string Name, string Address, string ContactNumber, string AlternateContactNumber, string Specialty, string QualificationSummary)
        {
            return new DevOpsPortal.Models.CustomerDetails
            {
                Name = Name,
                Address = Address,
                ContactNumber = ContactNumber,
                AlternateContactNumber = AlternateContactNumber,
                Specialty = Specialty,
                QualificationSummary = QualificationSummary
            };
        }

        [TestMethod]
        public void TestViewMethod_Moq()
        {
            // Arrange
            var faqs = new List<CustomerDetails>();
            faqs.Add(new CustomerDetails { Name = "Arghya", Address = "West Bengal", ContactNumber = "9087654332", AlternateContactNumber = "7965432781", Specialty = "Cardiologist", QualificationSummary = "MBBS" });
            faqs.Add(new CustomerDetails { Name = "Pasha", Address = "Hyderabad", ContactNumber = "9087654332", AlternateContactNumber = "8965432781", Specialty = "Neurologist", QualificationSummary = "MBBS" });


            //-- Creating a fake ICustomerRepository object
            var repositoryMock = new Mock<ICustomerRepositoryMoq>();
            //-- Setting up the repository
            repositoryMock.Setup(m => m.GetCustomers()).Returns(faqs); //

            var service = new CustomerService(repositoryMock.Object);

            // Act 
            var result = service.GetCustomers();
            Console.WriteLine(result.Count);

            // Assert
            repositoryMock.Verify(m => m.GetCustomers(), Times.Once());
            Assert.IsNotNull(result);
        }


    }
}
