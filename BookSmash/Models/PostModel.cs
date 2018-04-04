using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
namespace BookSmash.Models
{
    public class PostModel
    {
        [Required]
        [Display(Name = "ID")]
        public string ID{ get; set;}
    }
}