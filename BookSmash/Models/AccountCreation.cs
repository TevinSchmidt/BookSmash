using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class AccountCreation
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "confirmPassword")]
        public string confirmPassword { get; set; }

        [Required]
        [Display(Name = "Fname")]
        public string Fname { get; set; }

        [Required]
        [Display(Name = "Lname")]
        public string Lname { get; set; }

        [Display(Name = "Phone_Num")]
        public string Phone_Num { get; set; }


        [Display(Name = "University")]
        public string University { get; set; }

        public IEnumerable<SelectListItem> Universities { get; set; }
    }
}