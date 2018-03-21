using BookSmash.Models;

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
            bool VALID_LOGIN = true;


            if (VALID_LOGIN)
            {
                //This is temp
                Globals.setCurrentUser(Email);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Index");
            }

            
        }

        //This function is called when there is a LogOut request
        public ActionResult LogOut()
        {
            //This is also temp
            Globals.logOutCurrentUser();
            return View("LogOut");
        }

        //This function is called when the user wants to create a new account
        public ActionResult AccountCreation()
        {
            return View("AccountCreation");
        }
    }
}