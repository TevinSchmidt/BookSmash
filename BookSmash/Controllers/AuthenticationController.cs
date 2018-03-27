using BookSmash.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace BookSmash.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication
        public ActionResult Index()
        {
            Globals.enumerateUniversities();
            return View("Index");
        }


        //This function is called when there is a logIn request 
        public ActionResult LogIn(string Email, string Password)
        {
            if(Email == "" || Password == "")
            {
                ViewBag.LogInError = "Username or password incorrect. Try again.";
            }

            grabFromDB DB = new grabFromDB();

            List<User> list = DB.getUsers(Email, Password);

            if(list == null)
            {
                ViewBag.LogInError = "Username or password incorrect. Try again.";
                return View("Index");
            }
            else
            {
                Globals.setCurrentUser(Email);
                return RedirectToAction("Index", "Authentication");
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
        public ActionResult CreationSubmittion(string Email, string Password, string confirmPassword, string Fname, string Lname, string Phone_num, string UNI_NAME)
        {
            //check for blank entries
            if (Email == "" || Password == "" || confirmPassword == "" || Fname == "" || Lname == "" || Phone_num == "")
            {
                ViewBag.EmptyFields = "Must not leave any blank. Try again.";
                return View("AccountCreation");
            }

            //check for invalid uni, this is temporary
            if(UNI_NAME == "")
            {
                ViewBag.InvalidUNI = "Please select a University from the drop down.";
                return View("AccountCreation");
            }

            //check for matching passwords
            if (!Password.Equals(confirmPassword))
            {
                ViewBag.PasswordDontMatch = "Passwords did not match. Try again.";
                return View("AccountCreation");
            }

            //Check for invalid email
            try
            {
                MailAddress mail = new MailAddress(Email);
            }
            catch (FormatException)
            {
                ViewBag.InvalidEmail = "This is not a valid email address. Try again.";
                return View("AccountCreation");
            }

            //Check for invalid phone_num - TODO need to fix to make areacode manditory 
            Regex rg = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
            if (!rg.IsMatch(Phone_num))
            {
                ViewBag.InvalidPhone = "This is not a valid phone number. Try again.";
            }

            grabFromDB DB = new grabFromDB();


            //must check to see if user already exists
            if (DB.getUserListByEmail(Email) != null)
            {
                ViewBag.InvalidEmail = "Email already used. Try a different one.";
          
                return View("AccountCreation");
            }

            if (DB.getUserListByPhone(Phone_num) != null)
            {
                ViewBag.InvalidPhone = "This phone number is already linked to an account. Please enter different one.";
                
                return View("AccountCreation");
            }

            
            DB.insertUser(Phone_num, Email, UNI_NAME, Fname, Lname, Password);

            ViewBag.SuccessfullyCreated = "Your account was successfully created. Thanks for joining! Please sign in to continue.";

            return View("LogIn");
           
        }
    }
}