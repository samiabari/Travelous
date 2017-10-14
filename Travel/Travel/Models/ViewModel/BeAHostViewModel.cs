using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class BeAHostViewModel
    {
        public int TouristId { set; get; }
        public Nullable<int> CountryId { set; get; }
        public Nullable<int> CityId { set; get; }


        [Display(Name = "Offer Profile Picture")]
        public byte[] HostPictureData { set; get; }

        [Required(ErrorMessage = "This is Required")]
        [Display(Name = "Host Type")]
        public int HostTypeId { set; get; }


        [Required(ErrorMessage = "Address is Required")]
    
        [Display(Name = "Address")]



        public string HostAddress { set; get; }




        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Business Phone No.")]

        public string HostPhnNo { set; get; }



        [RegularExpression(@"^[1-9]\d{0,7}(?:\.\d{1,4})?|\.\d{1,4}$", ErrorMessage = "Given price is not valid")]
        [Required(ErrorMessage = "You have to provide a Price")]
        [Display(Name = "Price.")]
        public string HostPrice { get; set; }
        [Required(ErrorMessage = "You have to provide a Time")]
        [Display(Name = "Opening date.")]
        [DataType(DataType.Date)]
        public DateTime HostStartTime { get; set; }

        [Required(ErrorMessage = "You have to provide a Time")]
        [Display(Name = "Ending dat.")]
        [DataType(DataType.Date)]
        public DateTime HostStopTime { get; set; }


        [AllowHtml]

        [Display(Name = "Facilities/ Detail")]
        public string HostDetail { get; set; }

        public IEnumerable<SelectListItem> CountryList { set; get; }
        public IEnumerable<SelectListItem> HostTypeList { set; get; }
    }
}