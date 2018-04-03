using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using BookSmash.Models;
using System.IO;

namespace BookSmash.Models
{

    public class User
    {
        public int ID;
        public string pw;
        public string phone;
        public string email;
        public string fname;
        public string lname;
        public string Uni;
    }

    public class UserInfo
    {
        public string phone;
        public string university;
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
        public string date;
        public string condition;
        public double price;
        public string description;
        public string Title;
        public string department;
        public string code;
        public int edition;
        public string author;
        public string coursename;
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
        /// Method to get all Posts by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public List<Result> getFavourites(string email)
        {
            LD = LinkDatabase.getInstance();
            //string query = @"SELECT ID FROM " + LD.databaseName + ".FAVOURITES WHERE EMAIL = '" + email + "';";
            string query = @"SELECT ID, TITLE FROM " + LD.databaseName + @".POST AS P NATURAL JOIN " + LD.databaseName +
                @".FAVOURITES AS F WHERE F.EMAIL = '" + email + @"' AND F.ID = P.ID;";
            List<Result> Favs = new List<Result>();
            try
            {
                Result temp;
                MySqlDataReader reader = LD.executeGenericSQL(query);
                while (reader.Read())
                {
                    temp = new Result();
                    temp.ID = reader.GetInt32("ID").ToString();
                    temp.title = reader.GetString("Title");
                    Favs.Add(temp);
                }
            } catch (Exception e)
            {
                sw.Write(e.Message);
            } finally
            {
                LD.doClose();
            }

                return Favs;
        }


        public string getUserPhone(string email)
        {
            LD = LinkDatabase.getInstance();
            string query = @"SELECT PHONE_NUM FROM " + LD.databaseName + @".USER WHERE EMAIL = '" + email + @"';";
            string result = "";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                if (reader.Read())
                {
                    result = reader.GetString("Phone_num");
                }
            } catch (Exception e)
            {
                sw.Write("Failure in getPhone: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            } finally
            {
                LD.doClose();
            }
            return result;
        }
        public bool favouriteExists(string phone, string email, string id)
        {
            bool result = false;
            LD = LinkDatabase.getInstance();
            string query = @"SELECT * FROM " + LD.databaseName + @".FAVOURITES WHERE PHONE_NUM = '" + phone +
                @"' AND EMAIL = '" + email + @"' AND ID = '" + id + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                if (reader.Read())
                {
                    result = true;
                }
            } catch (Exception e)
            {
                sw.Write(e.Message);
            } finally
            {
                LD.doClose();
            }
            return result;
        }
        public void saveFavourite(string phone, string email, string id)
        {
            if (!favouriteExists(phone, email, id))
            {
                string query = @"INSERT INTO " + LD.databaseName + @".FAVOURITES(phone_num, email, id)VALUES('" + phone +
                    @"', '" + email + @"', '" + id + @"');";
                try
                {
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query);
                }
                catch (Exception e)
                {
                    sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                }
            }
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
            catch (ArgumentException e)
            {
                LD.doClose();
                return null;
            }
            catch (Exception e)
            {
                try
                {
                    sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                }
                catch { }
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
            } finally {
                LD.doClose();
            }
            
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
        /// This method removes a university by the given name
        /// </summary>
        /// <param name="Uni_Name"></param>
        public void removeUniversityByName(String Uni_Name)
        {
            LD = LinkDatabase.getInstance();
            string query = @"DELETE FROM " + LD.databaseName + @".UNIVERSITY WHERE UNI_NAME = '" + Uni_Name + @"';";
            try
            {
                LD.executeNonQueryGeneric(query);
            }
            catch (Exception e)
            {
                sw.Write("Failure in insertFavourite: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
        }

        public UserInfo getUserInfo(string username)
        {
            LD = LinkDatabase.getInstance();

            string query = @"SELECT UNI_NAME, PHONE_NUM FROM " + LD.databaseName + @".USER WHERE EMAIL = '" +
                username + @"';";


            UserInfo info = new UserInfo();
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                if (reader.Read())
                {
                    info.phone = reader.GetString("PHONE_NUM");
                    info.university = reader.GetString("UNI_NAME");
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in getPost: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            LD.doClose();
            return info;

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
                    temp.date = reader.GetString("Date");
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

        public List<Result> getUserPosts(string username)
        {
            LD = LinkDatabase.getInstance();
            string query = @"SELECT TITLE, ID FROM " + LD.databaseName + @".POST WHERE EMAIL = '" + username +
                @"';";
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
            }
            catch (MySqlException d)
            {
                sw.Write("Sql Error:" + d.Message);
            }
            catch (Exception e)
            {
                sw.Write("Failure in getUniversities: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            finally
            {
                LD.doClose();
            }
            return search;

        }

        /// <summary>
        /// 
        /// </summary>
        public bool deletePost(string id)
        {
            LD = LinkDatabase.getInstance();
            string query = @"DELETE FROM " + LD.databaseName + @".POST WHERE ID = '" + id + @"';";
            try
            {
                LD.executeNonQueryGeneric(query);
                return true;
            }
            catch (Exception e)
            {
                sw.Write("Failure in deletePost: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            return false;
        }

  
        /// <summary>
        /// Method to insert a new post into the db
        /// </summary>
        /// <param name="post"></param>
        public string insertPost(Post post)
        {
            string query1 = @"INSERT INTO " + LD.databaseName + @".POST(Phone_Num, Email, UNI_NAME, Date, Book_Condition, Price, Description, Title)" + 
                @"VALUES('" + post.Phone + @"','" + post.email + @"','" + post.Uni + @"','" + post.date + @"','" + post.condition + @"','" + post.price + @"','" + post.description + 
                @"','" + post.Title +  @"');";

            if (checkTextbook(post.Title, post.edition))
            {
                if (checkAuthor(post.author, post.Title))
                {
                    //Title and Author both in data base, just insert post (Best Case)
                    try
                    {                        
                        if (!checkCourse(post.code, post.department, post.Uni))
                        {
                            string query5 = @"INSERT INTO " + LD.databaseName + @".COURSE(course_title, coursenum, department, uni_name)VALUES('" +
                                post.coursename + @"', '" + post.code + @"', '" + post.department + @"', '" + post.Uni + @"');";
                            LD = LinkDatabase.getInstance();
                            LD.executeNonQueryGeneric(query5); //Insert course
                        }
                        if (!checkUsedFor(post.Title, post.code, post.department))
                        {
                            string query4 = @"INSERT INTO " + LD.databaseName + @".USED_FOR(title, coursenum, department)VALUES('" +
                                post.Title + @"', '" + post.code + @"', '" + post.department + @"');";
                            LD = LinkDatabase.getInstance();
                            LD.executeNonQueryGeneric(query4); //Insert used_for
                        }
                        LD = LinkDatabase.getInstance();
                        LD.executeNonQueryGeneric(query1); //Insert Post
                    }
                    catch (Exception e)
                    {

                    }
                }
                else
                {
                    //Textbook exists but not author, insert post and author (highly unlikely)
                    string query2 = @"INSERT INTO " + LD.databaseName + @".AUTHOR(name, title)VALUES('" +
                        post.author + @"', '" + post.Title + @"');";
                    try
                    {
                        LD = LinkDatabase.getInstance();
                        LD.executeNonQueryGeneric(query2); //Insert author
                        if (!checkCourse(post.code, post.department, post.Uni))
                        {
                            string query5 = @"INSERT INTO " + LD.databaseName + @".COURSE(course_title, coursenum, department, uni_name)VALUES('" +
                                post.coursename + @"', '" + post.code + @"', '" + post.department + @"', '" + post.Uni + @"');";
                            LD = LinkDatabase.getInstance();
                            LD.executeNonQueryGeneric(query5); //Insert course
                        }
                        if (!checkUsedFor(post.Title, post.code, post.department))
                        {
                            string query4 = @"INSERT INTO " + LD.databaseName + @".USED_FOR(title, coursenum, department)VALUES('" +
                                post.Title + @"', '" + post.code + @"', '" + post.department + @"');";
                            LD = LinkDatabase.getInstance();
                            LD.executeNonQueryGeneric(query4); //Insert used_for
                        }
                        LD = LinkDatabase.getInstance();
                        LD.executeNonQueryGeneric(query1); //Insert post
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            else if (checkAuthor(post.author, post.Title))
            {

                string query3 = @"INSERT INTO " + LD.databaseName + @".TEXTBOOK(title, edition)VALUES('" +
                    post.Title + @"', '" + post.edition + @"');";
                //Textbook does not exist but author does(highly unlikely)
                try
                {
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query3); //Insert textbook
                    if (!checkCourse(post.code, post.department, post.Uni))
                    {
                        string query5 = @"INSERT INTO " + LD.databaseName + @".COURSE(course_title, coursenum, department, uni_name)VALUES('" +
                            post.coursename + @"', '" + post.code + @"', '" + post.department + @"', '" + post.Uni + @"');";
                        LD = LinkDatabase.getInstance();
                        LD.executeNonQueryGeneric(query5); //Insert course
                    }
                    if (!checkUsedFor(post.Title, post.code, post.department))
                    {
                        string query4 = @"INSERT INTO " + LD.databaseName + @".USED_FOR(title, coursenum, department)VALUES('" +
                            post.Title + @"', '" + post.code + @"', '" + post.department + @"');";
                        LD = LinkDatabase.getInstance();
                        LD.executeNonQueryGeneric(query4); //Insert used_for
                    }
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query1); //Insert post

                }
                catch (Exception e)
                {
                    sw.Write("Failure in insertPost textbook and post: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                }
            }
            else
            {
                string query2 = @"INSERT INTO " + LD.databaseName + @".AUTHOR(name, title)VALUES('" +
                    post.author + @"', '" + post.Title + @"');";
                string query3 = @"INSERT INTO " + LD.databaseName + @".TEXTBOOK(title, edition)VALUES('" +
                    post.Title + @"', '" + post.edition + @"');";
                try
                {
                    //Both text book and author do not , insert all
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query3); //Insert textbook
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query2); //Insert author
                    if (!checkCourse(post.code, post.department, post.Uni))
                    {
                        LD = LinkDatabase.getInstance();
                        string query5 = @"INSERT INTO " + LD.databaseName + @".COURSE(course_title, coursenum, department, uni_name)VALUES('" +
                            post.coursename + @"', '" + post.code + @"', '" + post.department + @"', '" + post.Uni + @"');";
                        
                        LD.executeNonQueryGeneric(query5); //Insert course
                    }
                    if (!checkUsedFor(post.Title, post.code, post.department))
                    {
                        LD = LinkDatabase.getInstance();
                        string query4 = @"INSERT INTO " + LD.databaseName + @".USED_FOR(title, coursenum, department)VALUES('" +
                            post.Title + @"', '" + post.code + @"', '" + post.department + @"');";
                        
                        LD.executeNonQueryGeneric(query4); //Insert used_for
                    }
                    LD = LinkDatabase.getInstance();
                    LD.executeNonQueryGeneric(query1); //Insert post
                }
                catch (Exception e)
                {
                    sw.Write("Failure in insertPost, post: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                }
            }
            return "You have successfully created a post";

        }
        public bool checkCourse(string code, string department, string university)
        {
            bool result = false;
            LD = LinkDatabase.getInstance();
            string query = @"SELECT COURSENUM, DEPARTMENT, UNI_NAME FROM " + LD.databaseName + @".COURSE WHERE COURSENUM = '" +
                code + @"' AND DEPARTMENT = '" + department + @"' AND UNI_NAME = '" + university + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);
                if (reader.Read())
                {
                    result = true;
                }
            } catch (Exception e)
            {
                sw.Write("Failure in insertPost textbook " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            } finally
            {
                LD.doClose();
            }
            return result;
        }
        public bool checkUsedFor(string title, string course, string department)
        {
            bool result = false;
            LD = LinkDatabase.getInstance();
            string query = @"SELECT TITLE, COURSENUM, DEPARTMENT FROM " + LD.databaseName + @".USED_FOR WHERE TITLE = '" +
                title + @"' AND COURSENUM = '" + course + @"' AND DEPARTMENT = '" + department + @"';"; 
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);

                if (reader.Read())
                {
                    result = true;
                }
            } catch (Exception e)
            {
                sw.Write("Failure in insertPost textbook " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            } finally
            {
                LD.doClose();
            }
            return result;
        }

        public bool checkTextbook(string textbook, int edition)
        {
            bool result = false;
            LD = LinkDatabase.getInstance();
            string query = @"SELECT TITLE, EDITION FROM " + LD.databaseName + @".TEXTBOOK WHERE TITLE = '" +
                textbook + @"' AND EDITION = '" + edition + @"';";
            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);

                if (reader.Read())
                {
                    result = true;
                }
            } catch (Exception e)
            {
                sw.Write("Failure in insertPost textbook " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            } finally
            {
                LD.doClose();
            }
            return result;
        }

        public bool checkAuthor(string author, string title)
        {
            bool result = false;
            LD = LinkDatabase.getInstance();
            string query = @"SELECT NAME, TITLE FROM " + LD.databaseName + @".AUTHOR WHERE NAME = '" +
                author + "' AND TITLE = '" + title + @"';";

            try
            {
                MySqlDataReader reader = LD.executeGenericSQL(query);

                if (reader.Read())
                {
                    result = true;
                }
            }
            catch (Exception e)
            {
                sw.Write("Failure in insertPost textbook " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
            finally
            {
                LD.doClose();
            }
            return result;
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
                        temp.pw = reader.GetString("Password");
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
        /// This function modifies a users email 
        /// </summary>
        /// <param name="newEmail"></param>
        /// <param name="oldEmail"></param>
        public void modifyUserEmail(string newEmail, string oldEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET Email = '" + newEmail + @"' WHERE Email = '" + oldEmail + @"'; ";
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
        /// This function modifies a users password
        /// </summary>
        /// <param name="newPassword"></param>
        /// <param name="oldPassword"></param>
        public void modifyUserPassword(string newPassword, string oldPassword, string userEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET Password = '" + newPassword + @"' WHERE Password = '" + oldPassword+ @"'AND Email = '" + userEmail + @"'; ";
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
        /// This function changes a users Fname
        /// </summary>
        /// <param name="newFname"></param>
        /// <param name="oldFname"></param>
        /// <param name="userEmail"></param>
        public void modifyUserFname(string newFname, string oldFname, string userEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET Fname = '" + newFname + @"' WHERE Fname = '" + oldFname + @"'AND Email = '" + userEmail + @"'; ";
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
        /// This function changes a users Lname
        /// </summary>
        /// <param name="newLname"></param>
        /// <param name="oldLname"></param>
        /// <param name="userEmail"></param>
        public void modifyUserLname(string newLname, string oldLname, string userEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET Lname = '" + newLname + @"' WHERE Lname = '" + oldLname + @"'AND Email = '" + userEmail + @"'; ";
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
        /// This function changes a users phone number
        /// </summary>
        /// <param name="newPhone"></param>
        /// <param name="oldPhone"></param>
        /// <param name="userEmail"></param>
        public void modifyUserPhone(string newPhone, string oldPhone, string userEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET Phone_Num = '" + newPhone + @"' WHERE Phone_Num = '" + oldPhone + @"'AND Email = '" + userEmail + @"'; ";
            try
            {
                LD.executeNonQueryGeneric(query);
            }
            catch (Exception e)
            {
                sw.Write("Failure in insertFavourite: " + e.Message + " " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
            }
        }

        public void modifyUserUniversity(string newUni, string oldUni, string userEmail)
        {
            LD = LinkDatabase.getInstance();
            string query = @"UPDATE " + LD.databaseName + @".USER SET UNI_NAME = '" + newUni + @"' WHERE UNI_NAME = '" + oldUni + @"'AND Email = '" + userEmail + @"'; ";
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
        /// This function removes an Admin given its email.
        /// </summary>
        /// <param name="email"></param>
        public void removeAdminByEmail(String email)
        {
            LD = LinkDatabase.getInstance();
            string query = @"DELETE FROM " + LD.databaseName + @".ADMIN WHERE Email = '" + email + @"';";
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