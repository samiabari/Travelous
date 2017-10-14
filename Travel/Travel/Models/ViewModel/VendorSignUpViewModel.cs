using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class VendorSignUpViewModel
    {

        [Required(ErrorMessage = "This is Required")]
        [Display(Name = "Company Type")]
        public int VendorTypeId { set; get; }


        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$", ErrorMessage = "Email is not valid")]
        [Display(Name = "Your Email")]
        public string VendorEmail { set; get; }



        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Phone No.")]
        public string VendorPhnNo { set; get; }


        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50,ErrorMessage = "Shouldn't be above 20 characters.")]
        [Display(Name = "Company Name")]

      
        public string VendorOfficeName { set; get; }

        [Required(ErrorMessage = "Country is Required")]
        [Display(Name = "Country")]
        public int CountryId { set; get; }



        [Required(ErrorMessage = "City is Required")]
        [Display(Name = "City")]
        public int CityId { set; get; }



        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$", ErrorMessage = "Email is not valid")]
        [Display(Name = "Company's Email")]

        public string VendorOfficeMail { set; get; }


        
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", ErrorMessage = "Website is not valid")]
        [Display(Name = "Company's Website")]

        
        public string VendorOfficeWebsite { set; get; }

     
        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string VendorPassword { set; get; }



        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("VendorPassword", ErrorMessage = "Not matched with password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string VendorConfirmPassword { set; get; }

        public IEnumerable<SelectListItem> CountryList { set; get; }
        public IEnumerable<SelectListItem> VendorTypeList { set; get; }
    }
}