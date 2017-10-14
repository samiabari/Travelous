using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class VendorOwnDetailViewModel
    {
        public int VendorId { set; get; }


        [Required(ErrorMessage = "Name is Required")]
        [StringLength(20, ErrorMessage = "Shouldn't be above 20 characters.")]
        [Display(Name = "Name")]
        
        public string VendorName { set; get; }



        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Phone No.")]
        public string VendorPhnNo { set; get; }


        [Required(ErrorMessage = "You have to provide an Addresss.")]
        [Display(Name = "Address.")]
        public string VendorAddress { set; get; }


    }
}