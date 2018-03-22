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
        public string bookType;
        public string condition;
        public double price;
        public string description;
        public string Title;
    }

    public class grabFromDB
    {
        StreamWriter sw;
        LinkDatabase LD;
        public grabFromDB()
        {
            string path = @"C:\BookSmash\Log.txt";
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
        }


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

        /// <summary>
        /// Main method used to get posts from front page search
        /// </summary>
        /// <param name="courseName"></param>
        /// <param name="title"></param>
        /// <param name="courseCode"></param>
        /// <param name="UniName"></param>
        public List<Post> getPost(string department, string title, int courseCode, string UniName)
        {
            string query = @"SELECT * FROM " + LD.databaseName + @".POST AS P, " + LD.databaseName + @".USED_FOR AS U" +
                @" WHERE P.Title = U.Title AND P.Title = '" + title + @" AND U.Department = '" + department + @"' AND U.CourseNum = '" 
                + courseCode + @"' AND P.UNI_Name = '" + UniName + @"';" ;
            return getPostCustom(query);
        }

        /// <summary>
        /// Method that will do the generic request. 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public List<Post> getPostCustom(string query)
        {
            LD = LinkDatabase.getInstance();
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
            }
            catch (Exception e)
            {
                sw.Write("Failure in getPost: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return outPost;
        }

        public List<string> getSearch(string query)
        {
            LD = LinkDatabase.getInstance();
            List<string> searchResults = new List<string>();


            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                while (reader.Read())
                {
                    s

                }
            } catch (Exception e)
            {
                sw.Write("Failure in getSearch: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return searchResults;
        }    
        /// <summary>
        /// Method to insert a new post into the db
        /// </summary>
        /// <param name="post"></param>
        public void insertPost(Post post)
        {
            LD = LinkDatabase.getInstance();
            string query = @"INSERT INTO " + LD.databaseName + @".POST(Phone_Num, Email, UNI_NAME, Date, BookType, Book_Condition, Price, Description, Title)" + 
                @"VALUES('" + post.Phone + @"','" + post.email + @"','" + post.Uni + @"','" + post.date + @"','" + post.bookType + @"','" + post.condition + @"','" + post.price + @"','" + post.description + 
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
            catch(Exception e)
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
        /// 
        /// </summary>
        public void close()
        {
            if(sw != null)
            sw.Close();
        }
    }
}