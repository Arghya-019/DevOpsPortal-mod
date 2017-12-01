using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOpsPortal.Repository;
using DevOpsPortal.Models;

namespace DevOpsPortal.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            CustomerRepository CustRepo = new CustomerRepository();
            ModelState.Clear();
            return View(CustRepo.GetCustomers());
        }

        public ActionResult Details(int id)
        {
            CustomerRepository CustRepo = new CustomerRepository();
            return View(CustRepo.GetCustomers().Find(Emp => Emp.CustomerId == id)); 
        }
        
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(CustomerDetails customerdetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomerRepository CustRepo = new CustomerRepository();

                    if (CustRepo.AddCustomer(customerdetails))
                    {
                        ViewBag.Message = "Customer details added successfully";
                    }
                }

                return RedirectToAction("Create");
            }
            catch (Exception x)
            {
                return View();
            }    
        }
        
        public ActionResult Edit(int id)
        {
            CustomerRepository CustRepo = new CustomerRepository();
            return View(CustRepo.GetCustomers().Find(Emp => Emp.CustomerId == id)); 
        }
        
        [HttpPost]
        public ActionResult Edit(int id, CustomerDetails customerdetails)
        {
            try
            {
                CustomerRepository CustRepo = new CustomerRepository();
                CustRepo.UpdateCustomer(customerdetails);
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                return View();
            }    
        }

        public ActionResult Delete(int id)
        {
            try
            {
                CustomerRepository CustRepo = new CustomerRepository();
                if (CustRepo.DeleteCustomer(id))
                {
                    ViewBag.AlertMsg = "Customer details deleted successfully";

                }
                return RedirectToAction("Index");

            }
            catch (Exception x)
            {
                return RedirectToAction("Index");
            }    
           
        }
    }
}
