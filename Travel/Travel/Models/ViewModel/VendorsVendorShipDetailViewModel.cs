using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class VendorsVendorShipDetailViewModel
    {
        public int VendorId { set; get; }



        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Shouldn't be above 50 characters.")]
        [Display(Name = "Company Name")]


        public string VendorOfficeName { set; get; }

      
        [Display(Name = "Company Type")]
        public Nullable<int> VendorTypeId { set; get; }


    
        [Display(Name = "Country")]
        public Nullable<int> CountryId { set; get; }



     
        [Display(Name = "City")]
        public Nullable<int> CityId { set; get; }




        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Phone No.")]
        public string VendorOfficePhnNo { set; get; }



        [Required(ErrorMessage = "Website is required.")]
        [RegularExpression(@"^(https?:\/\/)?([\da-z\.-]+)\.([a-z\.]{2,6})([\/\w \.-]*)*\/?$", ErrorMessage = "Website is not valid")]
        [Display(Name = "Company's Website")]
        
        public string VendorOfficeWebsite { set; get; }




        [Required(ErrorMessage = "You have to provide an Address.")]
        [Display(Name = "Address.")]
        public string VendorOfficeAddress { set; get; }

     

      

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression(@"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$", ErrorMessage = "Email is not valid")]
        [Display(Name = "Company's Email")]

        public string VendorOfficeMail { set; get; }



        [AllowHtml]

        [Display(Name = "Company's Detail")]
        public string VendorsVendorShipDetail { set; get; }




        public IEnumerable<SelectListItem> CountryList { set; get; }
        public IEnumerable<SelectListItem> VendorTypeList { set; get; }
    }
}