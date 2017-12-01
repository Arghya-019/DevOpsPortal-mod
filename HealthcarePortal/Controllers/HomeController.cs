using DevOpsPortal.CustomFilter;
using DevOpsPortal.Models;
using DevOpsPortal.Repository;
using DevOpsPortal.SessionObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace DevOpsPortal.Controllers
{    
    [CacheControl]
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            // return RedirectToAction("Login", "Home");

            return View("Index");
        }
        
        [HttpGet]
        public ActionResult Registration()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [HttpPost]
        public ActionResult Registration(LoginDetails LoginDetails)
        {
            try
            {
                CustomerRepository CustRepo = new CustomerRepository();
                LoginDetails user = new LoginDetails();
                user = CustRepo.GetUser(LoginDetails.Email);
                if (user.Email == null)
                {
                    if (ModelState.IsValid)
                    {
                        if (CustRepo.UserRegistration(LoginDetails))
                        {
                            TempData["Message"] = "User registered successfully.";
                            logger.Info("User registered successfully");
                        }
                    }
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "User already registered.";
                    return RedirectToAction("Registration");
                }
            }
            catch (Exception x)
            {
                logger.Error("Error in Registration method", x);
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.Error = TempData["Error"];
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginDetails LoginDetails)
        {
            try
            {
                SingletonObject oSingleton = SingletonObject.GetCurrentSingleton();

                if (oSingleton.EMail != "")
                {                    
                    return RedirectToAction("Index", "Customer");
                }
                else
                {

                    if (IsValid(LoginDetails.Email, LoginDetails.Password))
                    {
                        FormsAuthentication.SetAuthCookie(LoginDetails.Email, false);                        
                        oSingleton.EMail = LoginDetails.Email;
                        logger.Info("User loggedin successfully");
                        return RedirectToAction("Index", "Customer");
                    }
                    else
                    {
                        TempData["Error"] = "Login details are wrong.";
                        logger.Info("Login details are wrong");
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception x)
            {
                logger.Error("Error in Login method", x);
                return View();
            }
            
        }

        private bool IsValid(string email, string password)
        {            
            bool IsValid = false;
            CustomerRepository CustRepo = new CustomerRepository();
            LoginDetails user = new LoginDetails();
            user = CustRepo.GetUser(email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    IsValid = true;
                }
            }            
            return IsValid;
        }
        
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            SingletonObject oSingleton = SingletonObject.GetCurrentSingleton();
            oSingleton.EMail = "";
            return RedirectToAction("Login", "Home");
        }


    }
}