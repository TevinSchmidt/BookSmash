
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class CreatePostModel
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name ="Author")]
        public string Author { get; set; }

        [Display(Name ="Condition")]
        public string Condition { get; set; }

        [Required]
        [Display(Name = "Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Display(Name ="Course Name")]
        public string CourseName { get; set; }

        [Required]
        [Display(Name = "University")]
        public string University { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name ="Price")]
        public string Price { get; set; }


        public IEnumerable<SelectListItem> Universities { get; set; }

    }
}