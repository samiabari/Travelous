using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class SignInUserViewModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
      + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
      + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
      + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$", ErrorMessage = "Email is not valid")]
        [Display(Name = "Your Email")]
        public string Email { set; get; }


        [Required(ErrorMessage = "Password should be added for security purpose")]
        [Display(Name = "Password")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Should be above 3 characters")]
        public string Password { set; get; }

        [Required(ErrorMessage = "You have to select the type of your sign in.")]
        [Display(Name = "User Type")]
        public int UserType { set; get; }

    }
}