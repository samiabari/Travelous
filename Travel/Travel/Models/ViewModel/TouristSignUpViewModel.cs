using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class TouristSignUpViewModel
    {
        [Required(ErrorMessage = "Name is Required")]
        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Name is not valid")]
        [Display(Name = "Name")]
        public string TouristName { set; get; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
         + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
         + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
         + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$", ErrorMessage = "Email is not valid")]
        [Display(Name = "Email")]
        public string TouristEmail { set; get; }

        [Required(ErrorMessage = "Country is Required")]
        [Display(Name = "Country")]
        public int CountryId { set; get; }



        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]
        public int CityId { set; get; }



        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Phone No.")]

        public string TouristPhnNo { set; get; }



        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string TouristPassword { set; get; }


        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("TouristPassword", ErrorMessage = "Not matched with password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string TouristConfirmPassword { set; get; }


        public IEnumerable<SelectListItem> CountryList { set; get; }
       
    }
}