using BookSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult AdminEnrty(AdminModel m)
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
                ViewBag.AdminSubmitMessage = "Admin successfully added.";
                AdminModel model = new AdminModel();
                return View("AdminPage", model);
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
                ViewBag.AdminSubmitMessage = "Admin successfully removed.";           
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
                ViewBag.UniSubmitMessage = "University successfully added.";
                
                return View("AddUni");
            }
        }

        [HttpPost]
        public ActionResult RemoveUni(UniModel m)
        {
            if (m.UNI_NAME == "")
            {
                ViewBag.EmptyFields = "Must not leave any University entry fields blank. Try again.";
                return View("RemoveUni", m);
            }

            grabFromDB DB = new grabFromDB();

            if(DB.getUniversities().Count == 1)
            {
                ViewBag.InvalidUni = "Cannot delete the last university in the database. Try Again.";
                return View("RemoveUni", m);
            }

            if(DB.getUniversitiesByName(m.UNI_NAME).Count != 1)
            {
                ViewBag.InvalidUni = "This university does not exist in the database. Try Again.";
                return View("RemoveUni", m);
            }
            else
            {
                

                ViewBag.AdminSubmitMessage = "Admin successfully removed.";
                return View("AdminPage");
            }



            
        }

        #endregion

        #region User data modification

        public ActionResult SearchUsers()
        {
            return View("SearchUsers");
        }

        #endregion
    }
}