﻿using BookSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
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

            Globals.setCurrentPostEmail(post.email);
            Globals.setCurrentPostId(post.ID);
            List<ReviewResults> results = grab.getReviewByEmail(post.email);
            ViewBag.ReviewList = results;

            return View("Textbook");
        }

        public ActionResult MyPosts(string id)
        {
            grabFromDB grab = new grabFromDB();
            List <Result> results = grab.getUserPosts(Globals.getCurrentUserEmail());
            ViewBag.PostList = results;

            return View("MyPosts");

        }

        [HttpPost]
        public ActionResult ReviewUser()
        {
            string email = Globals.getCurrentPostEmail();

            grabFromDB DB = new grabFromDB();
            List<User> user = DB.getUserListByEmail(email);

            ReviewModel model = new ReviewModel();


            if (user.Count != 1)
            {
                //there was an error, must handle
            }
            else
            {
                model.Email = user[0].email;
                model.Phone_Num = user[0].phone;
            }
            model.ratings = new SelectList(Enum.GetValues(typeof(ratings)));

            return View("ReviewUser", model);
        }

        [HttpPost]
        public ActionResult AddReview(ReviewModel m)
        {
            //check for empty fields
            if(m.Email == "" || m.Phone_Num == "" || m.Reviewer_Email == "" || m.Description == "" || m.Rating == "")
            {
                ViewBag.EmptyFields = "Please fill out all feilds.";
                return View("ReviewUser", m);
            }

            //Check for invalid email
            try
            {
                MailAddress mail = new MailAddress(m.Reviewer_Email);
            }
            catch (FormatException)
            {
                ViewBag.InvalidEmail = "This is not a valid email address. Try again.";
                return View("ReviewUser", m);
            }

            //check description length
            if (m.Description.Length > 400)
            {
                ViewBag.DescriptionLengthError = "Review too long. Try again.";
                return View("ReviewUser", m);
            }

            //check email length
            if (m.Reviewer_Email.Length > 100)
            {
                ViewBag.InvalidEmail = "Email too long. Try again.";
                return View("ReviewUser", m);
            }

            int rating;

            if(m.Rating == Enum.GetName(typeof(ratings), 0))
            {
                rating = 1;
            }
            else if(m.Rating == Enum.GetName(typeof(ratings), 1))
            {
                rating = 2;
            }
            else if(m.Rating == Enum.GetName(typeof(ratings), 2))
            {
                rating = 3;
            }
            else if(m.Rating == Enum.GetName(typeof(ratings), 3))
            {
                rating = 4;
            }
            else if(m.Rating == Enum.GetName(typeof(ratings), 4))
            {
                rating = 5;
            }
            else
            {
                //error, should never get here though
                rating = 5;
            }

            grabFromDB DB = new grabFromDB();
            DB.insertReview(m.Phone_Num, m.Email, m.Reviewer_Email, m.Description, rating);

            int id = Globals.getCurrentPostId();

            return RedirectToAction("Textbook/" + id, "Post");
        }
    }
}