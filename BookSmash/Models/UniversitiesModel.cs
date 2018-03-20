
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace BookSmash.Models
{
    public class UniversitiesModel
    {
        [Required]
        [Display(Name ="University")]
        public string University { get; set; }

        public IEnumerable<SelectListItem> Universities { get; set; }
        
    }
}