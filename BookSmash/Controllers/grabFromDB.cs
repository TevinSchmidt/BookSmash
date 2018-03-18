using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using BookSmash.Models;
using System.IO;

namespace BookSmash.Controllers
{
    public class Favourites
    {
        public string phoneNumber;
        public string email;
        public int relatedID;
    }

    public class User
    {
        public int ID;
        public int Proirity;
        public string pw;
        public string phone;
        public string email;
        public string fname;
        public string lname;
        public string Uni;
    }

    public class Post
    {
        public int ID;
        public string Phone;
        public string email;
        public string Uni;
        public DateTime date;
        public string bookType;
        public string condition;
        public double price;
        public string description;
        public string Title;
    }

    public class grabFromDB
    {
        string dbname;
        StreamWriter sw;
        LinkDatabase LD;
        public grabFromDB(string indbname, LinkDatabase inLD)
        {
            LD = inLD;
            dbname = indbname;
            string path = "Log.txt";
            if (!File.Exists(path))
            {
                // Create a file to write to.
                sw = File.CreateText(path);
            }
        }

        /// <summary>
        /// Method to get all Posts by Author
        /// </summary>
        /// <param name="Author"></param>
        /// <returns></returns>
        public List<Post> getPostByAuthor(string Author)
        {
            string query = @"SELECT* FROM " + dbname + ".POST AS P, " + dbname + ".AUTHOR AS A WHERE P.Title = A.Title AND A.Name = " + Author;
            return getPostCustom(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Course"></param>
        /// <returns></returns>
        public List<Post> getPostByCourse(string Course)
        {
            //string query = @"SELECT* FROM " + dbname + ".POST WHERE UNI_NAME = " + Course;
            //return getPostCustom(query);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Uni"></param>
        /// <returns></returns>
        public List<Post> getPostByUniversity(string Uni)
        {
            string query = @"SELECT* FROM " + dbname + ".POST WHERE UNI_NAME = " + Uni;
            return getPostCustom(query);
        }

        /// <summary>
        /// Method to get all Posts by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Post> getPostByTitle(string title)
        {
            string query = @"SELECT* FROM " + dbname + ".POST WHERE Title = " + title;
            return getPostCustom(query);
        }

        /// <summary>
        /// Method that will do the generic request. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Post> getPostCustom(string query)
        {
            List<Post> outPost = new List<Post>();
            Post temp;
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                while (reader.Read())
                {
                    temp = new Post();
                    temp.ID = reader.GetInt32("ID");
                    temp.Phone = reader.GetString("Phone_Num");
                    temp.email = reader.GetString("Email");
                    temp.Uni = reader.GetString("UNI_NAME");
                    temp.date = reader.GetDateTime("Date");
                    temp.bookType = reader.GetString("BookType");
                    temp.condition = reader.GetString("Book_Condition");
                    temp.price = reader.GetDouble("Price");
                    temp.description = reader.GetString("Description");
                    temp.Title = reader.GetString("Title");
                    outPost.Add(temp);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                sw.Write("Failure in getPost: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            return outPost;
        }

        public void insertFavourite(string phone, string email, int relatedID)
        {
            string query = @"INSERT INTO " + dbname + ".FAVOURITES(Phone_Num, Email, ID)" + @"VALUES('" + phone + "','" + email + "','" + relatedID + "');";
            try
            {
                LD.executeNonQueryGeneric(query);
            }
            catch (Exception e)
            {
                sw.Write("Failure in insertFavourite: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customQuerry"></param>
        /// <returns></returns>
        public List<Favourites> getFavourites(int userId)
        {
            return getFavourites(@"SELECT * FROM " + dbname + ".FAVOURITES WHERE ID = " + userId.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Favourites> getFavourites(string customQuerry)
        {
            List<Favourites> Favs = new List<Favourites>();
            Favourites temp;
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                while (reader.Read())
                {
                    temp = new Favourites();
                    temp.phoneNumber = reader.GetString("Phone_Num");
                    temp.email = reader.GetString("Email");
                    temp.relatedID = reader.GetInt32("ID");
                    Favs.Add(temp);
                }
                reader.Close();
            }
            catch( Exception e)
            {
                sw.Write("Failure in getFavourites: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            return Favs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<User> getUser()
        {
            return getUser();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customQuery"></param>
        /// <returns></returns>
        public List<User> getUser(string customQuery)
        {
            List<User> outUser = new List<User>();
            return outUser;
        }

        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            sw.Close();
        }
    }
}