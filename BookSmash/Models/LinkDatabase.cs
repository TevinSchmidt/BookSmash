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
        /// Generic function to execute a command on the DB
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public MySqlDataReader executeGenericSQL(string query)
        {
            if (!openConnection())
            {
                throw new Exception("Could not connect to database.");
            }  
            MySqlCommand command = new MySqlCommand(query, connection);
            MySqlDataReader reader = command.ExecuteReader();
            return reader;
        }
        /// <summary>
        /// Method to close connection from outside
        /// </summary>
        public void doClose()
        {
            closeConnection();
        }

        /// <summary>
        /// Method for executing deletes and inserts or updates
        /// </summary>
        /// <param name="query"></param>
        public void executeNonQueryGeneric(string query)
        {

            if (!openConnection())
            {
                throw new Exception("Could not connect to database.");
            }
                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
                closeConnection();                
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
                    new Column("Edition", "INTEGER", new string[] {"NOT NULL"}, true, false, null, 1, 1)
                }
            ),
            new Table
            (
                dbname,
                "AUTHOR",
                new Column[]
                {
                    new Column("Name", "VARCHAR(100)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL"}, true, true, "TEXTBOOK", 1, 1) 
                }
            ),
            new Table
            (
                dbname,
                "UNIVERSITY",
                new Column[]
                {
                    new Column("UNI_NAME", "VARCHAR(100)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("City", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Prov_State", "CHAR(2)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Country", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1)

                }
            ),
            new Table
            (
                dbname,
                "COURSE",
                new Column[]
                {
                    new Column("Course_Title", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("CourseNum", "VARCHAR(15)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("Department", "CHAR(4)", new string[] {"NOT NULL"}, true, false, null, 1, 1),
                    new Column("UNI_NAME", "VARCHAR(100)", new string[] {"NOT NULL"}, false, true, "UNIVERSITY", 1, 1)
                   
                }

            ),
            new Table
            (
                dbname,
                "USED_FOR",
                new Column[]
                {
                    new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL"}, true, true, "TEXTBOOK", 1, 1),
                    new Column("CourseNum", "VARCHAR(15)", new string[] {"NOT NULL"}, true, true, "COURSE", 1, 1),
                    new Column("Department", "CHAR(4)" ,new string[] {"NOT NULL"}, true, false, null, 1, 1)
                }
            ),
            new Table
            (
                dbname, 
                "USER",
                new Column[]
                {
                    new Column("Phone_Num", "VARCHAR(14)", new string[] {"NOT NULL", "UNIQUE"}, false, false, null, 1, 1),
                    new Column("Email", "VARCHAR(100)", new string[] {"NOT NULL", "UNIQUE"}, true, false, null, 1, 1),
                    new Column("UNI_NAME", "VARCHAR(100)", new string[] {"NOT NULL"}, false, true, "UNIVERSITY", 1, 1),
                    new Column("Fname","VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Lname","VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Password","VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1)
                }
            ),
            new Table
            (
                dbname,
                "ADMIN",
                new Column[]
                {
                    new Column ("Email", "VARCHAR(100)", new string[] {"NOT NULL", "UNIQUE"}, true, true, "USER", 1, 1),
                    new Column ("Role", "VARCHAR(200)", new string[] {}, false, false, null, 1, 1)
                }       
            ),
            new Table
            (
                dbname,
                "REVIEW",
                new Column[]
                {
                    new Column("Phone_Num", "VARCHAR(14)", new string[] {"NOT NULL"}, false, true, "USER", 1, 1),
                    new Column("Email", "VARCHAR(100)", new string[] {"NOT NULL"}, false, true, "USER", 1, 1),
                    new Column("Reviewer_Email", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Description", "VARCHAR(400)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                    new Column("Rating", "INTEGER", new string[] {"NOT NULL"}, false, false, null, 1, 1)
                }
            ),
            new Table
            (
                dbname,
                "POST",
                new Column[]
                {
                   new Column("ID", "INTEGER", new string[] {"NOT NULL", "UNIQUE", "AUTO_INCREMENT"}, true, false, null, 1, 1),
                   new Column("Phone_Num", "VARCHAR(14)", new string[] {"NOT NULL"}, false, true, "USER", 1, 1),
                   new Column("Email", "VARCHAR(100)", new string[] {"NOT NULL"}, false, true, "USER", 1, 1),
                   new Column("UNI_NAME", "VARCHAR(100)", new string[] {"NOT NULL"}, false, true, "UNIVERSITY", 1, 1),
                   new Column("Date", "VARCHAR(50)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                   //new Column("BookType", "VARCHAR(100)", new string[] {}, false, false, null, 1, 1),
                   new Column("Book_Condition", "VARCHAR(100)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                   new Column("Price", "DOUBLE", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                   new Column("Description", "VARCHAR(400)", new string[] {"NOT NULL"}, false, false, null, 1, 1),
                   new Column("Title", "VARCHAR(100)", new string[] {"NOT NULL" }, false, true, "TEXTBOOK", 1, 1)

                }
            ),
            new Table
            (
                dbname,
                "FAVOURITES",
                new Column[]
                {
                    new Column("Phone_Num", "VARCHAR(14)", new string[] {"NOT NULL"}, true, true, "USER", 1, 1),
                    new Column("Email", "VARCHAR(100)", new string[] {"NOT NULL"}, true, true, "USER", 1, 1),
                    new Column("ID", "INTEGER", new string[] {"NOT NULL"}, true, true, "POST", 1, 1)
                }

            )
        };
    }
}