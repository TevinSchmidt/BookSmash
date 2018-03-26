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

            if (ModelState.IsValid)
            {
                Session["UniversityModel"] = model;
                return RedirectToAction("Results");
            }

            return View("FrontPage", model);
        }

        public ActionResult Done()
        {
            // Get University information from the session
            var model = Session["UniversityModel"] as UniversitiesModel;

            return View(model);
        }

        public ActionResult CreatePost()
        {
           // LinkDatabase DB = LinkDatabase.getInstance();

            var universities = GetAllUniversities();
            var model = new UniversitiesModel();
            model.Universities = GetSelectListItems(universities);

            return View(model);
        }

        [HttpPost]
        public ActionResult CreatPost(UniversitiesModel model)
        {
            var universities = GetAllUniversities();
            model.Universities = GetSelectListItems(universities);

            if (ModelState.IsValid)
            {
                Session["UniversityModel"] = model;
                return RedirectToAction("Done");
            }
            return View("FrontPage", model);
        }

        public ActionResult Results()
        {
            var model = Session["UniversityModel"] as UniversitiesModel;
            // grabFromDB grabFromDB = new grabFromDB();
            LinkDatabase database = LinkDatabase.getInstance();
            List<string> results = new List<string>();
            results = database.getSearchTitles(model.Title);

            ViewBag.Textbooklist = results;

            return View("Results");
        }

        private IEnumerable<string> GetAllUniversities()
        {
            //This could be a call to University in Database
            grabFromDB grabFromDB = new grabFromDB();
            List<UniData> uni = grabFromDB.getUniversities();
            List<string> output = new List<string>();
            string temp;
            grabFromDB.close();
            foreach(UniData data in uni)
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