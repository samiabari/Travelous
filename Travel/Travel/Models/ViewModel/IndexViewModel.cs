using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<SelectListItem> CountryList { set; get; }

        [Required(ErrorMessage = "Country is Required")]
      
        public int CountryId { set; get; }



        [Required(ErrorMessage = "City is required")]
        public int CityId { set; get; }
      
        [DataType(DataType.Date)]
        public DateTime Arrival { set; get; }
      
        [DataType(DataType.Date)]
        public DateTime Departure { set; get; }

       
    }
}