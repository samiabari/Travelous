using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Travel.Models.ViewModel
{
    public class SearchViewModel
    {
        public OfferIndexViewModel RestOffer { set; get; }
        public OfferIndexViewModel HotelOffer { set; get; }
        public OfferIndexViewModel TransOffer { set; get; }
        public OfferIndexViewModel TraAgeOffer { set; get; }
        public VendorDetailViewModel RestVendor { set; get; }

        public VendorDetailViewModel HotelVendor { set; get; }
        public VendorDetailViewModel TransVendor { set; get; }
        public VendorDetailViewModel TravVendor { set; get; }
        public List<PlaceIndexViewModel> Places { set; get; }
        public List<RestaurentVendorViewModel> Rest { set; get; }
        public int CountryId{ set; get; }
        public int CityId { set; get; }
        public string Arrival { set; get; }
        public string Departure { set; get; }

        public IEnumerable<SelectListItem> CountryList { set; get; }
        public int RestaurentVendorId = 3;
        public int RestHouseVendorId = 12;
        public int HotelVendorId = 11;
        public int TransportVendorId = 5;
        public int TravelVendorId = 10;

        public int PhotoVendorId = 13;

        public int LaundVendorId = 9;

        public int FoodVendorId = 4;

        public int GuideVendorId = 7;
        public int HospitalPlaceId = 2;
        public int MarketPlaceId = 4;
    }
}