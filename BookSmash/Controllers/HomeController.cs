using BookSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookSmash.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult FrontPage()
        {
            //LinkDatabase DB = LinkDatabase.getInstance();

            var universities = GetAllUniversities();

            var model = new UniversitiesModel();
            model.Universities = GetSelectListItems(universities);


            return View(model);
        }

        [HttpPost]
        public ActionResult FrontPage(UniversitiesModel model)
        {
            var universities = GetAllUniversities();

            model.Universities = GetSelectListItems(universities);

            if (ModelUniversity.IsValid)
            {
                Session["UniversityModel"] = model;
                return RedirectToAction("Done");
            }

            return View("FrontPage", model);
        }

        public ActionResult Done()
        {
            // Get Sign Up information from the session
            var model = Session["UniversityModel"] as UniversityModel;

            // Display Done.html page that shows Name and selected state.
            return View(model);
        }

        private IEnumerable<string> GetAllUniversities()
        {
            return new List<string>
            {
                "University of Calgary",
                "University of Alberta",
                "Univerity of British Columbia",
            };
        }
        private IEnumerable<SelectListItem> GetSelectListItems(IEnumerable<string> elements)
        {
            // Create an empty list to hold result of the operation
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            // This will result in MVC rendering each item as:
            //     <option value="State Name">State Name</option>
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

        public ActionResult Index()
        {          

            return RedirectToAction("FrontPage");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}