using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class EditPlaceViewModel
    {
        public int PlaceId { set; get; }

        [RegularExpression(@"^[a-zA-Z-\s]+$", ErrorMessage = "Name is not valid")]
        [Display(Name = "Name")]
        public string PlaceName { set; get; }


        [Display(Name = "Place Type")]
        public Nullable<int> PlaceTypeId { set; get; }


        [Display(Name = "Place Address")]
        public string PlaceAddress { set; get; }

        [Display(Name = "Place Detail")]
        [AllowHtml]
        public string PlaceDetail { set; get; }

        
        public int CountryId { set; get; }
        public int CityId { set; get; }

        public IEnumerable<SelectListItem> PlaceTypeList { set; get; }
    }
}