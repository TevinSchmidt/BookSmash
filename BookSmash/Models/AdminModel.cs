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
}