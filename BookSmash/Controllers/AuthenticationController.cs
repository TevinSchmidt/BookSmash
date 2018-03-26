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
                return RedirectToAction("Index", "Authentication");
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

        //This function creates a new user
        public ActionResult CreationSubmittion(string Email, string Password, string Fname, string Lname,string Phone_num, string UNI_NAME)
        {
            grabFromDB DB = new grabFromDB();



            //must check to see if user already exists
            if (DB.getUserListByEmail(Email) != null)
            {
                ViewBag.UsernameTaken = "Email already used. Try a different one.";
          
                return View("AccountCreation");
            }

            if (DB.getUserListByPhone(Phone_num) != null)
            {
                ViewBag.PhoneNumTaken = "This phone number is already linked to an account. Please enter different one.";
                
                return View("AccountCreation");
            }

            
            DB.insertUser(Phone_num, Email, UNI_NAME, Fname, Lname, Password);

            ViewBag.SuccessfullyCreated = "Your account was successfully created. Thanks for joining! Please sign in to continue.";

            return View("LogIn");
           
        }
    }
}