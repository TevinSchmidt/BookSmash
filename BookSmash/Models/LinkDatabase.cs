using MySql.Data.MySqlClient;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace BookSmash.Models
{
    public partial class LinkDatabase : AbstractDatabase
    {
        private LinkDatabase() { }

        public static LinkDatabase getInstance()
        {
            if(instance == null)
            {
                instance = new LinkDatabase();
            }
            return instance;
        }

        /// <summary>
        /// Gets a long URL based on the id of the short url
        /// </summary>
        /// <param name="id">The id of the short url</param>
        /// <throws type="ArgumentException">Throws an argument exception if the short url id does not refer to anything in the database</throws>
        /// <returns>The long url the given short url refers to</returns>
        public string getLongUrl(string id)
        {
            string query = @"SELECT * FROM " + dbname + ".shortenedLinks "
                + "WHERE id=" + id + ";";

            if(openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                if(reader.Read() == true)
                {
                    return reader.GetString("original");
                }
                else
                {
                    //Throw an exception indicating no result was found
                    throw new ArgumentException("No url in the database matches that id.");
                }
            }
            else
            {
                throw new Exception("Could not connect to database.");
            }
        }

        /// <summary>
        /// Saves the longURL to the database to be accessed later via the id that is returned.
        /// </summary>
        /// <param name="longURL">The longURL to be saved</param>
        /// <returns>The id of the url</returns>
        public string saveLongURL(string longURL)
        {
            string query = @"INSERT INTO " + dbname + ".shortenedLinks(original) "
                + @"VALUES('" + longURL + @"');";

            if(openConnection() == true)
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();

                command.CommandText = "SELECT * FROM " + dbname + ".shortenedLinks WHERE id = LAST_INSERT_ID();";

                MySqlDataReader reader = command.ExecuteReader();

                if(reader.Read() == true)
                {
                    string result = reader.GetInt64("id").ToString();
                    closeConnection();
                    return result.ToString();
                }
                else
                {
                    closeConnection();
                    throw new Exception("Error: LAST_INSERT_ID() did not work as intended.");
                }
            }
            else
            {
                throw new Exception("Could not connect to database");
            }
        }
    }

    public partial class LinkDatabase : AbstractDatabase
    {
        private static LinkDatabase instance = null;

        private const String dbname = "BookSmash";
        public override String databaseName { get; } = dbname;

        protected override Table[] tables { get; } =
        {
            // This represents the database schema
            new Table
            (
                dbname,
                "TEXTBOOK", 
                new Column[]
                {
                    new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Edition", "INTEGER", new string[] {"NOT NULL"}, false, false, null, 1, 1)
                }
            ),
            new Table
            (
                dbname,
                "AUTHOR",
                new Column[]
                {
                    new Column("Name", "VARCHAR(100)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL"}, true, true, "TEXTBOOK", 1, 1) //On update cascade??
                }
            ),
            new Table
            (
                dbname,
                "COURSE",
                new Column[]
                {
                    new Column("Course_Title", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("CourseNum", "INTEGER", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Department", "CHAR(4)", new string[] {"NOT NULL"}, true, false, null, 1, 1)
                   
                }

            ),
            new Table
            (
                dbname,
                "USED_FOR",
                new Column[]
                {
                    new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL"}, true, true, "TEXTBOOK", 1, 1),
                    new Column("CourseNum", "INTEGER", new string[] {"NOT NULL"}, true, true, "COURSE", 1, 1),
                    new Column("Department", "CHAR(4)" ,new string[] {"NOT NULL"}, true, false, null, 1, 1)
                }
            ),

          new Table
            (
                dbname,
                "UNIVERSITY",
                new Column[]
                {
                    new Column("UNI_NAME", "VARCHAR(50)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("City", "VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Prov_State", "CHAR(2)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Country", "VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1)

                }
            ),

                /**,
            new Table
            (
                dbname, 
                "USER",
                new Column[]
                {
                    new Column("Phone_Num", "VARCHAR(14)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Email", "VARCHAR(50)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("UNI_NAME", "VARCHAR(50)", new string[] {"NOT NULL"}, false, true, "UNIVERSITY", 1, 1),
                    new Column("Fname","VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Lname","VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Password","VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                }
            )*/

        };
    }
}