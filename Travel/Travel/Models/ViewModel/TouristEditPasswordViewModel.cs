using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class TouristEditPasswordViewModel
    {
        public int TouristId { set; get; }

        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]

        public string TouristPassword { set; get; }



        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("TouristPassword", ErrorMessage = "Not matched with password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string TouristConfirmPassword { set; get; }
    }
}