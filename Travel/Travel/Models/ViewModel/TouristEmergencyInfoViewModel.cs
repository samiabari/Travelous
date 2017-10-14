using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class TouristEmergencyInfoViewModel
    {
        public int TouristId { set; get; }
        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Emergency Phone No.")]
        public string TouristEmergencyPhnNo { set; get; }


        [Required(ErrorMessage = "You have to provide an Addresss.")]
        [Display(Name = "Address.")]
        public string TouristAddress { set; get; }
    }
}