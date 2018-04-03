using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class UniModel
    {
        //Tthis is for adding a new uni
        [Required]
        [Display(Name = "UNI_NAME")]
        public string UNI_NAME { get; set; }

        
        [Display(Name = "City")]
        public string City { get; set; }

        
        [Display(Name = "Prov_State")]
        public string Prov_State { get; set; }

        
        [Display(Name = "Country")]
        public string Country { get; set; }

        

    }

    public class AdminModel
    {
        //This is for admin addition
        [Required]
        [Display(Name = "UserEmail")]
        public string UserEmail { get; set; }

        
        [Display(Name = "Role")]
        public string Role { get; set; }
    }

    public class AdminSearchUserModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class AdminUserResultModel
    {
        
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "confirmPassword")]
        public string confirmPassword { get; set; }
       
        [Display(Name = "Fname")]
        public string Fname { get; set; }
        
        [Display(Name = "Lname")]
        public string Lname { get; set; }

        [Display(Name = "Phone_Num")]
        public string Phone_Num { get; set; }

        [Display(Name = "University")]
        public string University { get; set; }

        public IEnumerable<SelectListItem> Universities { get; set; }
    }
}