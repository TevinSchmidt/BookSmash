using BookSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookSmash.Controllers
{
    public class PostController : Controller
    {
        public ActionResult Textbook(string id)
        {
            grabFromDB grab = new grabFromDB();
            Post post = grab.getPost(id);
            ViewBag.Post = post;

            return View("Textbook");
        }
    }
}