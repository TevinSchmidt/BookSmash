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
    public class AdminController : Controller
    {
        public ActionResult AdminPage()
        {

            return View("AdminPage");
        }

        #region Admin

        public ActionResult AddAdmin()
        {
            AdminModel model = new AdminModel();
            return View("AddAdmin", model);
        }

        public ActionResult DeleteAdmin()
        {
            AdminModel model = new AdminModel();
            return View("DeleteAdmin", model);
        }

        [HttpPost]
        public ActionResult AdminEntry(AdminModel m)
        {
            //check for blank entries
            if (m.UserEmail == "" || m.Role == "")
            {
                ViewBag.EmptyFielsAdnminEntry = "Must not leave any Admin entry fields blank. Try again.";
                return View("AddAdmin", m);
            }

            if (m.UserEmail.Length > 100)
            {
                ViewBag.InvalidEmail = "Email too long. Try again.";
                return View("AddAdmin", m);
            }

            if (m.Role.Length > 200)
            {
                ViewBag.InvalidRole = "Role too long. Try again.";
                return View("AddAdmin", m);
            }

            grabFromDB DB = new grabFromDB();

            if(DB.getAdminByEmail(m.UserEmail).Count != 0)
            {
                ViewBag.InvalidEmail = "The user associated with this account is already an admin. Try again.";
                return View("AddAdmin", m);
            }

            //check if valid user
            if (DB.getUserListByEmail(m.UserEmail).Count == 0)
            {
                ViewBag.InvalidEmail = "This email is not associated with a current account. Try again.";
                return View("AddAdmin", m);
            }
            else
            {
                DB.insertAdmin(m.UserEmail, m.Role);
                ViewBag.ReturnValue = "Admin successfully added.";
                return View("AdminPage");
            }
        }

        [HttpPost]
        public ActionResult RemoveAdmin(AdminModel m)
        {
            if (m.UserEmail == "")
            {
                ViewBag.EmptyFielsAdnminEntry = "Must not leave any Admin entry fields blank. Try again.";
                return View("DeleteAdmin", m);
            }

            if (m.UserEmail == AbstractDatabase.AdminEmail)
            {
                ViewBag.InvalidEmail = "Cannot remove that admin. Try again.";
                return View("DeleteAdmin", m);
            }

            if (m.UserEmail.Length > 100)
            {
                ViewBag.InvalidEmail = "Email too long. Try again.";
                return View("DeleteAdmin", m);
            }

            grabFromDB DB = new grabFromDB();

            //check if valid user
            if (DB.getAdminByEmail(m.UserEmail).Count == 0)
            {
                ViewBag.InvalidEmail = "This email is not associated with a current admin. Try again.";
                return View("DeleteAdmin", m);
            }
            else
            {
                
                DB.removeAdminByEmail(m.UserEmail);
                ViewBag.ReturnValue = "Admin successfully removed.";           
                return View("AdminPage");
            }

           
        }

        #endregion

        #region University

        public ActionResult AddUni()
        {
            UniModel model = new UniModel();
            return View("AddUni", model);
        }

        public ActionResult DeleteUni()
        {
            UniModel model = new UniModel();
            return View("DeleteUni", model);
        }


        [HttpPost]
        public ActionResult UniEntry(UniModel m)
        {
            //check for blank entries
            if (m.UNI_NAME == "" || m.City == "" || m.Prov_State == "" || m.Country == "")
            {
                ViewBag.EmptyFields = "Must not leave any University entry fields blank. Try again.";
                return View("AddUni", m);
            }

            if (m.UNI_NAME.Length > 100)
            {
                ViewBag.InvalidUni = "University name too long. Try again.";
                return View("AddUni", m);
            }

            if (m.City.Length > 100)
            {
                ViewBag.InvalidCity = "City name too long. Try again.";
                return View("AddUni", m);
            }

            //check for two letter province
            if (m.Prov_State.Length != 2)
            {
                ViewBag.IncorectProv = "Province must be two letters. AB, BC, ect. Try Again.";
                return View("AddUni", m);
            }

            if (m.Country.Length > 100)
            {
                ViewBag.IncorrectCountry = "Country name too long.";
                return View("AddUni", m);
            }

            grabFromDB DB = new grabFromDB();

            if (DB.getUniversitiesByName(m.UNI_NAME).Count != 0)
            {
                ViewBag.InvalidUni = "This university has already been entered. Try again.";
                return View("AddUni", m);
            }
            else
            {
                DB.insertUniversity(m.UNI_NAME, m.City, m.Prov_State, m.Country);
                ViewBag.ReturnValue = "University successfully added.";
                
                return View("AdminPage");
            }
        }

        [HttpPost]
        public ActionResult RemoveUni(UniModel m)
        {
            if (m.UNI_NAME == "")
            {
                ViewBag.EmptyFields = "Must not leave any University entry fields blank. Try again.";
                return View("DeleteUni", m);
            }

            grabFromDB DB = new grabFromDB();

            if(DB.getUniversities().Count == 1)
            {
                ViewBag.InvalidUni = "Cannot delete the last university in the database. Try Again.";
                return View("DeleteUni", m);
            }

            if(DB.getUniversitiesByName(m.UNI_NAME).Count != 1)
            {
                ViewBag.InvalidUni = "This university does not exist in the database. Try Again.";
                return View("DeleteUni", m);
            }
            else
            {

                DB.removeUniversityByName(m.UNI_NAME);
                ViewBag.ReturnValue = "University successfully removed.";
                return View("AdminPage");
            }



            
        }

        #endregion

        #region User data modification

        public ActionResult SearchUsers()
        {
            AdminSearchUserModel model = new AdminSearchUserModel();
            return View("SearchUsers", model);
        }

        [HttpPost]
        public ActionResult PerformSearch(AdminSearchUserModel m)
        {
            if(m.Email == "")
            {
                ViewBag.InvalidEmail = "Please do not leave blank. Try Again.";
                return View("SearchUsers", m);
            }

            if(m.Email.Length > 100)
            {
                ViewBag.InvalidEmail = "Email too long. Try Again.";
            }

            grabFromDB DB = new grabFromDB();

            if(DB.getUserListByEmail(m.Email).Count != 1)
            {
                ViewBag.InvalidEmail = "That user does not exist. Try Again.";
                return View("SearchUsers", m);
            }
            else
            {
                User user = new User();
                user = DB.getUserListByEmail(m.Email)[0];

                ViewBag.User = user;
                Globals.setUserToModify(user);

                AdminUserResultModel model = new AdminUserResultModel();
                var universities = GetAllUniversities();
                model.Universities = GetSelectListItems(universities);

                return View("UserEdit", model);
            }

            
        }

        public ActionResult ModifyUser(AdminUserResultModel m)
        {
            User toModify = Globals.getUserToModify();
            grabFromDB DB = new grabFromDB();
            List<string> modifiedReturn = new List<string>();

            if(m.Email != null && toModify.email != m.Email)
            {
                if(m.Email.Length > 100)
                {
                    ViewBag.InvalidEmail = "Email too long. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                if(DB.getUserListByEmail(m.Email).Count == 1)
                {
                    ViewBag.InvalidEmail = "This email is already associated with an account. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                //Check for invalid email
                try
                {
                    MailAddress mail = new MailAddress(m.Email);
                }
                catch (FormatException)
                {
                    ViewBag.InvalidEmail = "This is not a valid email address. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                DB.modifyUserEmail(m.Email, toModify.email);
                toModify.email = m.Email;
                modifiedReturn.Add("Email successfully changed.");
            }

            if(m.Password != null && toModify.pw != m.Password)
            {
                if (m.Password != m.confirmPassword)
                {
                    ViewBag.InvalidPassword = "Password does not match confirmation password. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                if (m.Password.Length > 100 || m.confirmPassword.Length > 100)
                {
                    ViewBag.InvalidPassword = "Password too long. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                DB.modifyUserPassword(m.Password, toModify.pw, toModify.email);
                modifiedReturn.Add("Password successfully changed");
            }

            if(m.Fname != null && toModify.fname != m.Fname)
            {
                if(m.Fname.Length > 100)
                {
                    ViewBag.InvalidFname = "First name too long. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                DB.modifyUserFname(m.Fname, toModify.fname, toModify.email);
                modifiedReturn.Add("First name successfully changed");
            }


            if(m.Lname != null && toModify.lname != m.Lname)
            {
                if(m.Lname.Length > 100)
                {
                    ViewBag.InvalidLname = "Last name too long. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                DB.modifyUserLname(m.Lname, toModify.lname, toModify.email);
                modifiedReturn.Add("Last name successfully changed.");
            }

            if(m.Phone_Num != null && toModify.phone != m.Phone_Num)
            {
                if(m.Phone_Num.Length > 14)
                {
                    ViewBag.InvalidPhone = "Phone number too long. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                Regex rg = new Regex(@"^\s*(?:\+?(\d{1,3}))?[-. (]*(\d{3})[-. )]*(\d{3})[-. ]*(\d{4})(?: *x(\d+))?\s*$");
                if (!rg.IsMatch(m.Phone_Num))
                {
                    ViewBag.InvalidPhone = "This is not a valid phone number. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                if (DB.getUserListByPhone(m.Phone_Num).Count == 1)
                {
                    ViewBag.InvalidPhone = "Phone number already associated with an account. Try again.";
                    ViewBag.User = toModify;
                    var universities = GetAllUniversities();
                    m.Universities = GetSelectListItems(universities);
                    return View("UserEdit", m);
                }

                DB.modifyUserPhone(m.Phone_Num, toModify.phone, toModify.email);
                modifiedReturn.Add("Phone number successfully changed.");
                
            }

            if(m.University != null && m.University != toModify.Uni)
            {
                DB.modifyUserUniversity(m.University, toModify.Uni, toModify.email);
                modifiedReturn.Add("University successfully changed.");
            }



            ViewBag.ModifiedReturn = modifiedReturn;
            return View("AdminPage");
        }
        #endregion

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