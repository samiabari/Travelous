using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class PlaceDetailViewModel
    {
        public int PlaceId { set; get; }
    
        public string PlaceName { set; get; }
        public string PlaceAddress { set; get; }
        public int PlaceTypeId { set; get; }
        public string PlaceTypeName { set; get; }
      
        public string PlaceShortDetail { set; get; }
        public string PlaceFullDetail { set; get; }
        public string CountryName { set; get; }
        public string CityName { set; get; }
        public byte[] PlacePictureData { set; get; }
        public int PlaceLike { set; get; }
   
        public int PlaceCountryId { set; get; }
        public int PlaceCityId { set; get; }
        public bool PlaceAdminPermit { set; get; }
        public int CountryId { set; get; }
        public int CityId { set; get; }
    }
}