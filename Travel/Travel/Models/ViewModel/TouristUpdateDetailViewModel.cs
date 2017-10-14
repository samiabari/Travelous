using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class TouristUpdateDetailViewModel
    {

        public int TouristId { set; get; }
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(20, ErrorMessage = "Shouldn't be above 20 characters.")]
        [Display(Name = "Name")]


        public string TouristName { set; get; }


        [Display(Name = "Country")]
        public Nullable<int> CountryId { set; get; }




        [Display(Name = "City")]
        public Nullable<int> CityId { set; get; }




        [RegularExpression(@"^[0-9,+,(), ,]{1,}(,[0-9]+){0,}$", ErrorMessage = "Phone is not valid")]
        [Required(ErrorMessage = "You have to provide a phone no.")]
        [Display(Name = "Phone No.")]
        public string TouristPhnNo { set; get; }




        public IEnumerable<SelectListItem> CountryList { set; get; }
    }
}