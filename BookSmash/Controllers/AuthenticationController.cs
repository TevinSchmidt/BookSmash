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

            if(list.Count == 0)
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
            AccountCreation model = new AccountCreation();

            var universities = GetAllUniversities();

            model.Universities = GetSelectListItems(universities);

            return View("AccountCreation", model);
        }

        //This function creates a new user
        [HttpPost]
        public ActionResult CreationSubmittion(AccountCreation m)
        {
            //check for blank entries
            if (m.Email == "" || m.Password == "" || m.confirmPassword == "" || m.Fname == "" || m.Lname == "" || m.Phone_Num == "")
            {
                ViewBag.EmptyFields = "Must not leave any blank. Try again.";
                return View("AccountCreation");
            }

            //check for invalid uni, this is temporary
            /*
            if(UNI_NAME == "")
            {
                ViewBag.InvalidUNI = "Please select a University from the drop down.";
                return View("AccountCreation");
            }*/

          

            //check for matching passwords
            if (!m.Password.Equals(m.confirmPassword))
            {
                ViewBag.PasswordDontMatch = "Passwords did not match. Try again.";
                return View("AccountCreation");
            }

            //Check for invalid email
            try
            {
                MailAddress mail = new MailAddress(m.Email);
            }
            catch (FormatException)
            {
                ViewBag.InvalidEmail = "This is not a valid email address. Try again.";
                return View("AccountCreation");
            }

            //Check for invalid phone_num - TODO need to fix to make areacode manditory 
            Regex rg = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
            if (!rg.IsMatch(m.Phone_Num))
            {
                ViewBag.InvalidPhone = "This is not a valid phone number. Try again.";
            }

            grabFromDB DB = new grabFromDB();


            //must check to see if user already exists
            if (DB.getUserListByEmail(m.Email).Count != 0)
            {
                ViewBag.InvalidEmail = "Email already used. Try a different one.";
          
                return View("AccountCreation");
            }

            if (DB.getUserListByPhone(m.Phone_Num).Count != 0)
            {
                ViewBag.InvalidPhone = "This phone number is already linked to an account. Please enter different one.";
                
                return View("AccountCreation");
            }

            
            DB.insertUser(m.Phone_Num, m.Email, m.University, m.Fname, m.Lname, m.Password);

            ViewBag.SuccessfullyCreated = "Your account was successfully created. Thanks for joining! Please sign in to continue.";

            return View("LogIn");
           
        }

        public ActionResult AdminPage()
        {

            return View("AdminPage");
        }

        public ActionResult AdminEntry(string UserEmail, string Role)
        {
            //check for blank entries
            if(UserEmail == "" || Role == "")
            {
                ViewBag.EmptyFielsAdnminEntry = "Must not leave any Admin entry fields blank. Try again.";
                return View("AdminPage");
            }

            grabFromDB DB = new grabFromDB();

            //check if valid user
            if(DB.getUserListByEmail(UserEmail).Count == 0)
            {
                ViewBag.UserDoesNotExist = "This email is not associated with a current account. Try again.";
                return View("AdminPage");
            }
            else
            {
                DB.insertAdmin(UserEmail, Role);
                ViewBag.AdminSubmitMessage = "Admin successfully added.";
                return View("AdminPage");
            }
        }

        public ActionResult UniEntry(string UNI_NAME, string City, string Prov_State, string Country)
        {

            //check for blank entries
            if (UNI_NAME == "" || City == "" || Prov_State == "" || Country == "" )
            {
                ViewBag.EmptyFields = "Must not leave any University entry fields blank. Try again.";
                return View("AdminPage");
            }

            //check for two letter province
            if(Prov_State.Length != 2)
            {
                ViewBag.IncorectProv = "Province must be two letters. AB, BC, ect. Try Again.";
                return View("AdminPage");
            }

            grabFromDB DB = new grabFromDB();

            if (DB.getUniversitiesByName(UNI_NAME).Count != 0)
            {
                ViewBag.InvalidUni = "This university has already been entered. Try again.";
                return View("AdminPage");
            }
            else
            {
                DB.insertUniversity(UNI_NAME, City, Prov_State, Country);
                ViewBag.UniSubmitMessage = "University successfully added.";
                return View("AdminPage");
            }

        }

        private IEnumerable<string> GetAllUniversities()
        {
            //This could be a call to University in Database
            grabFromDB grabFromDB = new grabFromDB();
            List<UniData> uni = grabFromDB.getUniversities();
            List<string> output = new List<string>();
            string temp;
            grabFromDB.close();
            foreach (UniData data in uni)
            {
                temp = data.name;
                output.Add(temp);
            }
            return output;
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            foreach (var element in elements)
            {
                selectList.Add(new SelectListItem
                {
                    Value = element,
                    Text = element
                });
            }

            return selectList;
        }
    }
}