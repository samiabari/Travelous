using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class VendorEditPasswordViewModel
    {
        public int VendorId { set; get; }
        
        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
 
        public string VendorPassword { set; get; }



        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Confirm Password")]
        [System.ComponentModel.DataAnnotations.Compare("VendorPassword", ErrorMessage = "Not matched with password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string VendorConfirmPassword { set; get; }
    }
}