using System.Web;

namespace BookSmash.Models
{

    public static class Globals
    {
        public static bool isLoggedIn()
        {
            if ("Logged In".Equals(getCurrentUser())){
                return true;
            }

            return false;
            
        }

        public static void setCurrentUser(string user)
        {
            HttpContext.Current.Session.Remove("user");
            HttpContext.Current.Session.Add("user", "Logged In");
        }

        public static void logOutCurrentUser()
        {
            HttpContext.Current.Session.Remove("user");
            HttpContext.Current.Session.Add("user", "Logged Out");
        }

        public static string getCurrentUser()
        {
            return (string)HttpContext.Current.Session["user"];
        }
    }

}