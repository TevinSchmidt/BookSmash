using System.Web;

namespace BookSmash.Models
{

    public static class Globals
    {
        public static bool isLoggedIn()
        {
            if ("Logged In".Equals(getCurrentUser())) {
                return true;
            }

            return false;

        }

        public static bool isAdmin()
        {
            if ("True".Equals(getAdminStatus()))
            {
                return true;
            }

            return false;
        }

        public static void setCurrentPostEmail(string email)
        {
            HttpContext.Current.Session.Remove("currentPostEmail");

            HttpContext.Current.Session.Add("currentPostEmail", email);
        }

        public static string getCurrentPostEmail()
        {
            return (string)HttpContext.Current.Session["currentPostEmail"];
        }

        public static void removeCurrentPostEmail()
        {
            HttpContext.Current.Session.Remove("currentPostEmail");
        }

        public static void setCurrentPostId(int id)
        {
            HttpContext.Current.Session.Remove("currentPostId");
            HttpContext.Current.Session.Add("currentPostId", id);
        }

        public static int getCurrentPostId()
        {
            return (int)HttpContext.Current.Session["currentPostId"];
        }

        public static void removeCurrentPostId()
        {
            HttpContext.Current.Session.Remove("currentPostId");
        }

        public static void setCurrentUser(string email)
        {
            HttpContext.Current.Session.Remove("userStatus");
            HttpContext.Current.Session.Remove("userEmail");
            HttpContext.Current.Session.Remove("userAdmin");

            grabFromDB DB = new grabFromDB();
            if(DB.getAdminByEmail(email).Count == 1)
            {
                HttpContext.Current.Session.Add("userAdmin", "True");
            }
            else
            {
                HttpContext.Current.Session.Add("userAdmin", "False");
            }

            HttpContext.Current.Session.Add("userStatus", "Logged In");
            HttpContext.Current.Session.Add("userEmail", email);
        }

        public static void logOutCurrentUser()
        {
            HttpContext.Current.Session.Remove("userStatus");
            HttpContext.Current.Session.Remove("userEmail");
            HttpContext.Current.Session.Remove("userAdmin");
            HttpContext.Current.Session.Add("userStatus", "Logged Out");
        }

        public static string getCurrentUser()
        {
            return (string)HttpContext.Current.Session["userStatus"];
        }

        public static string getCurrentUserEmail()
        {
            return (string)HttpContext.Current.Session["userEmail"];
        }


        public static string getAdminStatus()
        {
            return (string)HttpContext.Current.Session["userAdmin"];
        }

    }


}