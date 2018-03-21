
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class UniversitiesModel
    {
        [Required]
        [Display(Name ="Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name ="Department")]
        public string Department { get; set; }

        [Required]
        [Display(Name ="Code")]
        public string Code { get; set; }

        [Required]
        [Display(Name ="University")]
        public string University { get; set; }

        public IEnumerable<SelectListItem> Universities { get; set; }
        
    }
}