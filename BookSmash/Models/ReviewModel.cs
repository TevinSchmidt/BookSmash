using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class ReviewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Phone_Num")]
        public string Phone_Num { get; set; }

        [Required]
        [Display(Name = "Reviewer_Email")]
        public string Reviewer_Email { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Rating")]
        public string Rating { get; set; }

        public IEnumerable<SelectListItem> ratings { get; set; }
    }

    enum ratings { One, Two, Three, Four, Five}; 
}