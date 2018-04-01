using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using BookSmash.Models;
using System.IO;

namespace BookSmash.Models
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

    public class Admin
    {
        public string email;
        public string role;
    }

    public class UniData
    {
        public string name;
        public string city;
        public string prov_state;
        public string country;
    }

    public class Post
    {
        public int ID;
        public string Phone;
        public string email;
        public string Uni;
        public DateTime date;
        public string condition;
        public double price;
        public string description;
        public string Title;
        public string department;
        public string code;
        public int edition;
    }

    public class Result
    {
        public string ID;
        public string title;
    }

    public class ReviewResults
    {
        public string Reviewer_Email;
        public string Description;
        public Int32 Rating;
    }

    public class grabFromDB
    {
        StreamWriter sw;
        LinkDatabase LD;
        public grabFromDB()
        {

            string path = @"C:\BookSmash\Log.txt";
            string directory = @"C:\BookSmash";

            if (!File.Exists(path))
            {
                // Create a file to write to.
                if (!Directory.Exists(directory))
                {
                    DirectoryInfo di = Directory.CreateDirectory(directory);
                }
                sw = File.CreateText(path);
            }
            
        }

        /// <summary>
        /// Method to get all Posts by Author
        /// </summary>
        /// <param name="Author"></param>
        /// <returns></returns>
      /**  public List<Post> getPostByAuthor(string Author)
        {
            string query = @"SELECT* FROM " + LD.databaseName + ".POST AS P, " + LD.databaseName + ".AUTHOR AS A WHERE P.Title = A.Title AND A.Name = '" + Author + "';";
            return getPostCustom(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Course"></param>
        /// <returns></returns>
        public List<Post> getPostByCourse(string Course)
        {
            string query = @"SELECT* FROM " + LD.databaseName + ".POST AS P, " + LD.databaseName + ".USED_FOR AS U, " + LD.databaseName + ".COURSE AS C WHERE C.Course_Title LIKE '" + Course + "' AND C.CourseNum = U.CourseNum AND U.Title = P.Title;" ;
            return getPostCustom(query);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Uni"></param>
        /// <returns></returns>
        public List<Post> getPostByUniversity(string Uni)
        {
            string query = @"SELECT* FROM " + LD.databaseName + ".POST WHERE UNI_NAME = '" + Uni + "';";
            return getPostCustom(query);
        }



        /// <summary>
        /// Method to get all Posts by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Post> getPostByTitle(string title)
        {
            string query = @"SELECT* FROM " + LD.databaseName + ".POST WHERE Title = '" + title + "';";
            return getPostCustom(query);
        }*/


        /// <summary>
        /// Method to get all the universites in a list
        /// </summary>
        /// <returns></returns>
        public List<UniData> getUniversities()
        {
            LD = LinkDatabase.getInstance();
            string query = @"SELECT* FROM " + LD.databaseName + ".UNIVERSITY";
            List<UniData> output = new List<UniData>();
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                UniData temp;
                while (reader.Read())
                {
                    temp = new UniData();
                    temp.name = reader.GetString("UNI_NAME");
                    temp.city = reader.GetString("City");
                    temp.prov_state = reader.GetString("Prov_State");
                    temp.country = reader.GetString("Country");
                    output.Add(temp);
                }
            }
            catch (ArgumentException e)
            {
                LD.doClose();
                return null;
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return output;
        }

        /// <summary>
        /// Method to get all the universites in a list
        /// </summary>
        /// <returns></returns>
        public List<UniData> getUniversitiesByName(string UNI_NAME)
        {
            LD = LinkDatabase.getInstance();
            string query = @"SELECT * FROM " + LD.databaseName + @".UNIVERSITY WHERE UNI_NAME = '" + UNI_NAME + @"';";
            List<UniData> output = new List<UniData>();
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                UniData temp;
                while (reader.Read())
                {
                    temp = new UniData();
                    temp.name = reader.GetString("UNI_NAME");
                    temp.city = reader.GetString("City");
                    temp.prov_state = reader.GetString("Prov_State");
                    temp.country = reader.GetString("Country");
                    output.Add(temp);
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return output;
        }

        /// <summary>
        /// Method to add a new university to the db
        /// </summary>
        /// <param name="name"></param>
        /// <param name="city"></param>
        /// <param name="prov"></param>
        /// <param name="Country"></param>
        public void insertUniversity(string name, string city, string prov, string Country)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + ".UNIVERSITY(UNI_NAME, City, Prov_State, Country)" + @"VALUES('" + name + "','" + city + "','" + prov + "','" + Country + "');";
            try
            {
                LD.executeNonQueryGeneric(query);
            }
            catch (Exception e)
            {
                sw.Write("Failure in insertFavourite: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
        }

        public List<Result> getSearchTitles(string title, string department, string code, string university)
        {
            LD = LinkDatabase.getInstance();
            //string query = @"SELECT TITLE FROM POST WHERE TITLE = '" + title +  @"';";
           string query = @"SELECT P.TITLE, P.ID FROM " + LD.databaseName + @".POST AS P, " + LD.databaseName + @".USED_FOR AS U" +
                            @" WHERE P.Title = U.Title AND P.Title LIKE '%" + title + @"%' AND U.Department = '" + department + @"' AND U.CourseNum = '"
                            + code + @"' AND P.UNI_Name = '" + university + @"';";
            List<Result> search = new List<Result>();
            try
            {
                Result temp;
                MySqlDataReader reader = LD.executeGenericSQL(query);
                while (reader.Read())
                {
                    temp = new Result();
                    temp.ID = reader.GetInt32("ID").ToString();
                    temp.title = reader.GetString("Title");
                    search.Add(temp);
                }
            } catch (MySqlException d)
            {
                sw.Write("Sql Error:" + d.Message);
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return search;


        }

            /// <summary>
            /// Main method used to get posts from front page search
            /// </summary>
            /// <param name="courseName"></param>
            /// <param name="title"></param>
            /// <param name="courseCode"></param>
            /// <param name="UniName"></param>
            //public List<Post> getPost(string department, string title, string courseCode, string UniName)
            public Post getPost(string id)
        {
            LD = LinkDatabase.getInstance();

            string query = @"SELECT * FROM " + LD.databaseName + @".POST NATURAL JOIN " + LD.databaseName +
                @".USED_FOR WHERE " + LD.databaseName + @".POST.ID = '" + id + @"';";
            //string query = @"SELECT * FROM " + LD.databaseName + @".POST WHERE ID = '" + id + @"';";
            //string query = @"SELECT * FROM " + LD.databaseName + @".POST AS P, " + LD.databaseName + @".USED_FOR AS U" +
               // @" WHERE P.Title = U.Title AND P.Title = '" + title + @" AND U.Department = '" + department + @"' AND U.CourseNum = '" 
                //+ courseCode + @"' AND P.UNI_Name = '" + UniName + @"';" ;

            //List<Post> outPost = new List<Post>();
            Post temp = new Post();
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                if (reader.Read())
                {
                    
                    temp.ID = reader.GetInt32("ID");
                    temp.Phone = reader.GetString("Phone_Num");
                    temp.email = reader.GetString("Email");
                    temp.Uni = reader.GetString("UNI_NAME");
                    temp.date = reader.GetDateTime("Date");
                   // temp.bookType = reader.GetString("BookType");
                    temp.condition = reader.GetString("Book_Condition");
                    temp.price = reader.GetDouble("Price");
                    temp.description = reader.GetString("Description");
                    temp.Title = reader.GetString("Title");
                    temp.department = reader.GetString("Department");
                    temp.code = reader.GetString("CourseNum");


                   // outPost.Add(temp);
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getPost: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return temp;
        }

  
        /// <summary>
        /// Method to insert a new post into the db
        /// </summary>
        /// <param name="post"></param>
        public void insertPost(Post post)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + @".POST(Phone_Num, Email, UNI_NAME, Date, BookType, Book_Condition, Price, Description, Title)" + 
                @"VALUES('" + post.Phone + @"','" + post.email + @"','" + post.Uni + @"','" + post.date + @"','" + post.condition + @"','" + post.price + @"','" + post.description + 
                @"','" + post.Title +  @"');";
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
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="relatedID"></param>
        public void insertFavourite(string phone, string email, int relatedID)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + ".FAVOURITES(Phone_Num, Email, ID)" + @"VALUES('" + phone + "','" + email + "','" + relatedID + "');";
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
            return getFavourites(@"SELECT * FROM " + LD.databaseName + ".FAVOURITES WHERE ID = '" + userId.ToString() + "';");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Favourites> getFavourites(string customQuerry)
        {
            LD = LinkDatabase.getInstance();
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
            }
            catch( Exception e)
            {
                sw.Write("Failure in getFavourites: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return Favs;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="customQuery"></param>
        /// <returns></returns>
        public List<User> getUsers(string username, string password)
        {
            LD = LinkDatabase.getInstance();
            List<User> users = new List<User>();
            User temp;
            string customQuerry = @"SELECT * FROM " + LD.databaseName + @".USER WHERE Email = '" + username + @"' AND Password = '" + password + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                while (reader.Read())
                {
                    temp = new User();
                    temp.phone = reader.GetString("Phone_Num");
                    temp.email = reader.GetString("Email");
                    temp.Uni = reader.GetString("UNI_NAME");
                    temp.fname = reader.GetString("Fname");
                    temp.lname = reader.GetString("Lname");
                    users.Add(temp);
                }
            }
            catch (ArgumentException e)
            {
                LD.doClose();
                return null;
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUsers: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return users;
        }

        /// <summary>
        /// This is design to check if a username is already taken, should return empty if username is taken already.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public List<User> getUserListByEmail(string username)
        {
            LD = LinkDatabase.getInstance();
            List<User> users = new List<User>();
            User temp;
            string customQuerry = @"SELECT * FROM " + LD.databaseName + @".USER WHERE Email = '" + username + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                if (reader != null)
                {
                    while (reader.Read())
                    {


                        temp = new User();
                        temp.phone = reader.GetString("Phone_Num");
                        temp.email = reader.GetString("Email");
                        temp.Uni = reader.GetString("UNI_NAME");
                        temp.fname = reader.GetString("Fname");
                        temp.lname = reader.GetString("Lname");
                        users.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUsers: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return users;
        }

        public List<User> getUserListByPhone(string Phone_Num)
        {
            LD = LinkDatabase.getInstance();
            List<User> users = new List<User>();
            User temp;
            string customQuerry = @"SELECT * FROM " + LD.databaseName + @".USER WHERE Phone_Num = '" + Phone_Num + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                if (reader != null)
                {
                    while (reader.Read())
                    {


                        temp = new User();
                        temp.phone = reader.GetString("Phone_Num");
                        temp.email = reader.GetString("Email");
                        temp.Uni = reader.GetString("UNI_NAME");
                        temp.fname = reader.GetString("Fname");
                        temp.lname = reader.GetString("Lname");
                        users.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUsers: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="uni"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="PW"></param>
        public void insertUser(string phone, string email, string uni, string fname, string lname, string PW)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + ".USER(Phone_Num, Email, UNI_NAME, Fname, Lname, Password)" + 
                @"VALUES('" + phone + "','" + email + "','" + uni + "','" + fname + "','" + lname + "','" + PW + "');";
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
        /// This retrieves an admin by its email address
        /// </summary>
        /// <param name="Phone_Num"></param>
        /// <returns></returns>
        public List<Admin> getAdminByEmail(string Email)
        {
            LD = LinkDatabase.getInstance();
            List<Admin> users = new List<Admin>();
            Admin temp;
            string customQuerry = @"SELECT * FROM " + LD.databaseName + @".ADMIN WHERE Email = '" + Email + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        temp = new Admin();
                     
                        temp.email = reader.GetString("Email");
                        temp.role = reader.GetString("Role");

                        users.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUsers: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return users;
        }

        /// <summary>
        /// This function inserts an admin into the database
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <param name="uni"></param>
        /// <param name="fname"></param>
        /// <param name="lname"></param>
        /// <param name="PW"></param>
        public void insertAdmin(string email, string role)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + ".ADMIN(Email, Role)" +
                @"VALUES('" + email + "','"  + role + "');";
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
        /// <param name="email"></param>
        /// <param name="role"></param>
        public void insertReview(string Phone_Num, string Email,  string Reviewer_Email, string Description, int Rating)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + ".REVIEW(Phone_Num, Email, Reviewer_Email, Description, Rating)" +
                @"VALUES('" + Phone_Num + "','" + Email + "','" + Reviewer_Email + "','" + Description + "','"  + Rating + "');";
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
        /// <param name="Email"></param>
        /// <returns></returns>
        public List<ReviewResults> getReviewByEmail(string Email)
        {
            LD = LinkDatabase.getInstance();
            List<ReviewResults> reviews = new List<ReviewResults>();
            ReviewResults temp;
            string customQuerry = @"SELECT * FROM " + LD.databaseName + @".REVIEW WHERE Email = '" + Email + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(customQuerry);
                if (reader != null)
                {
                    while (reader.Read())
                    {
                        temp = new ReviewResults();

                        temp.Reviewer_Email = reader.GetString("Reviewer_Email");
                        temp.Description = reader.GetString("Description");
                        temp.Rating = reader.GetInt32("Rating");

                        reviews.Add(temp);
                    }
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUsers: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return reviews;
        }


        /// <summary>
        /// 
        /// </summary>
        public void close()
        {
            if(sw != null)
            sw.Close();
        }
    }
}