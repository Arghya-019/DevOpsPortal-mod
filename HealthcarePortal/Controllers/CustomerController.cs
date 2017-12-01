using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevOpsPortal.Repository;
using DevOpsPortal.Models;
using log4net;
using DevOpsPortal.CustomFilter;

using FeatureSwitcher;
using DevOpsPortal.Features;

namespace DevOpsPortal.Controllers
{
    [SessionExpire]
    [CacheControl]
    public class CustomerController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            try
            {
                logger.Info("Index action requested");
                CustomerRepository CustRepo = new CustomerRepository();
                ModelState.Clear();                
                return View(CustRepo.GetCustomers());
            }
            catch (Exception x)
            {
                logger.Error("Error in Index method", x);
                return View();
            
            }
          }

        public ActionResult Details(int id)
        {
            try
            {
                logger.Info("Details action requested");
                CustomerRepository CustRepo = new CustomerRepository();
                return View(CustRepo.GetCustomers().Find(Emp => Emp.CustomerId == id));
            }
            catch (Exception x)
            {
                logger.Error("Error in Details method", x);
                return View();
            }
        }
        
        public ActionResult Create()
        {
            var myNewFeature = new DevOpsPortal.Features.NewCustomer.NewCustomerCreation(); //for database feched toggle 

            if (Feature<CreateNewCustomer>.Is().Disabled) //for web.config feched toggle 
            {
                return this.HttpNotFound();
            }

            else if (!myNewFeature.FeatureEnabled) //for database feched toggle 
            {
                return this.HttpNotFound();
            }



            logger.Info("Create action requested");
            return View();
        }
   
        
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                        logger.Info("Customer details added successfully");
                    }
                }

                return RedirectToAction("Create");
            }
            catch (Exception x)
            {
                logger.Error("Error in Create method", x);
                return View();
            }    
        }
        
        public ActionResult Edit(int id)
        {
            try
            {
                logger.Info("Edit action requested");
                CustomerRepository CustRepo = new CustomerRepository();
                return View(CustRepo.GetCustomers().Find(Emp => Emp.CustomerId == id));
            }
            catch (Exception x)
            {
                logger.Error("Error in Edit method", x);
                return View();
            }
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CustomerDetails customerdetails)
        {
            try
            {
                CustomerRepository CustRepo = new CustomerRepository();
                CustRepo.UpdateCustomer(customerdetails);
                logger.Info("Customer details updated successfully");
                return RedirectToAction("Index");
            }
            catch (Exception x)
            {
                logger.Error("Error in Edit method", x);
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
                    logger.Info("Customer details deleted successfully");
                }
                return RedirectToAction("Index");

            }
            catch (Exception x)
            {
                logger.Error("Error in Delete method", x);
                return RedirectToAction("Index");
            }    
           
        }
 }
}
