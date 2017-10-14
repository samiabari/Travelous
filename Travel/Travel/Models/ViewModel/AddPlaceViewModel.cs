using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class AddPlaceViewModel
    {
        [Display(Name = "Place Profile Picture")]
        public byte[] PlacePictureData { set; get; }

        [Required(ErrorMessage = "Name is Requirde")]

      
        [Display(Name = "Name")]
        public string PlaceName { set; get; }
        [Required(ErrorMessage = "Country is Requirde")]
        [Display(Name = "Country")]

        public int CountryId { set; get; }
        [Required(ErrorMessage = "City is Requirde")]
        [Display(Name = "City")]

        public int CityId { set; get; }
        [Required(ErrorMessage = "Place type is Requirde")]
        [Display(Name = "Place Type")]
        public int PlaceTypeId { set; get; }

        [Required(ErrorMessage = "Place Address is Requirde")]
        [Display(Name = "Place Address")]
        public string PlaceAddress { set; get; }

        [Display(Name = "Place Detail")]
        [AllowHtml]
        public string PlaceDetail { set; get; }


        public IEnumerable<SelectListItem> CountryList { set; get; }

        public IEnumerable<SelectListItem> PlaceTypeList { set; get; }
    }
}