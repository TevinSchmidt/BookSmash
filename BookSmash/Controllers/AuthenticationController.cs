using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookSmash.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            return View("Index");
        }


        //This function is called when there is a logIn request 
        public ActionResult LogIn(string Email, string Password)
        {
            return View("Index");
        }

        //This function is called when there is a LogOut request
        public ActionResult LogOut()
        {
            return View("LogOut");
        }

        //This function is called when the user wants to create a new account
        public ActionResult AccountCreation()
        {
            return View("AccountCreation");
        }
    }
}