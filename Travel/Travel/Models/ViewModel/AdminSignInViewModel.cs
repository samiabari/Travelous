using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class AdminSignInViewModel
    {
        [Required(ErrorMessage = "You have fill this field.")]
        [StringLength(5, ErrorMessage = "Not Pssible")]
        [Display(Name = "Admin")]
        public string AdminName { set; get; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(5, ErrorMessage = "Not Possible")]
        [Display(Name = "Password")]
        public string AdminPassword { set; get; }
    }
}