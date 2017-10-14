using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class AddOfferViewModel
    {
        public int VendorId { set; get; }
        public int CountryId { set; get; }
        public int CityId { set; get; }


        [Display(Name = "Offer Profile Picture")]
        public byte[] OfferPictureData { set; get; }

        [Required(ErrorMessage = "This is Required")]
        [Display(Name = "Offer Type")]
        public int OfferTypeId { set; get; }

       
        [Required(ErrorMessage = "Name is Required")]
        [StringLength(50, ErrorMessage = "Shouldn't be above 50 characters.")]
        [Display(Name = "Offer Name")]


      
        public string OfferName { set; get; }

        [RegularExpression(@"^[1-9]\d{0,7}(?:\.\d{1,4})?|\.\d{1,4}$", ErrorMessage = "Given price is not valid")]
        [Required(ErrorMessage = "You have to provide a Price")]
        [Display(Name = "Price.")]
        public string OfferPrice { get; set; }
        [Required(ErrorMessage = "You have to provide a Time")]
        [Display(Name = "Opening date.")]
        [DataType(DataType.Date)]
        public DateTime OfferStartTime { get; set; }

        [Required(ErrorMessage = "You have to provide a Time")]
        [Display(Name = "Ending dat.")]
        [DataType(DataType.Date)]
        public DateTime OfferStopTime { get; set; }


        [AllowHtml]

        [Display(Name = "Offer's Detail")]
        public string OfferDetail { get; set; }

      
        public IEnumerable<SelectListItem> OfferTypeList { set; get; }


    }
}