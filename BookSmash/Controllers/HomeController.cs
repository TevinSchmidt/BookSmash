using BookSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookSmash.Controllers
{
    public class ResultsList
    { 
            public List<string> resultsList;
    }


public class HomeController : Controller
    {
        public ActionResult FrontPage()
        {

            var universities = GetAllUniversities();
            var model = new UniversitiesModel();
            model.Universities = GetSelectListItems(universities);

            return View(model);
            //return View("FrontPage");
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

        public ActionResult Results()
        {
            var model = Session["UniversityModel"] as UniversitiesModel;
            grabFromDB grabFromDB = new grabFromDB();

            List<Result> results = grabFromDB.getSearchTitles(model.Title, model.Department, model.Code, model.University);

            ViewBag.Textbooklist = results;

            return View("Results");
        }

        public ActionResult CreatePost()
        {
           
            var universities = GetAllUniversities();
            var model = new CreatePostModel();
            model.Universities = GetSelectListItems(universities);

            return View(model);
        }

        [HttpPost]
        public ActionResult CreatePost(CreatePostModel model)
        {
            //var universities = GetAllUniversities();
           // model.Universities = GetSelectListItems(universities);

            if (ModelState.IsValid)
            {
                Session["CreatePostModel"] = model;
                return RedirectToAction("Post");
            }
            return View("Error");
        }

        public ActionResult Post()
        {
            var model = Session["CreatePostModel"] as CreatePostModel;
            grabFromDB grab = new grabFromDB();
            UserInfo temp = grab.getUserInfo(Globals.getCurrentUserEmail());
            //string date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");
            Post post = new Post();
            post.code = model.Code;
            post.condition = model.Condition;
            post.coursename = model.CourseName;
            post.date = DateTime.Now;
            post.department = model.Department;
            post.description = model.Description;
            post.edition = model.Edition;
            post.email = Globals.getCurrentUserEmail();
            post.Phone = temp.phone;
            post.price = model.Price;
            post.Title = model.Title;
            post.Uni = temp.university;
            post.author = model.Author;            

            ViewBag.Success = grab.insertPost(post);           

            return View("Success");
        }

        public ActionResult Success()
        {
            return View();
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


        #region default

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

        #endregion
    }
}