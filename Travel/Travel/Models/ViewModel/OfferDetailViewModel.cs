using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class OfferDetailViewModel
    {
        public int OfferId { set; get; }
        public int VendorId { set; get; }
        public string VendorName { set; get; }
        public string VendorTypeName { set; get; }
        public string OfferName { set; get; }
        public string OfferAddress { set; get; }
        public string OfferPrice { set; get; }
        public string OfferPhnNo { set; get; }
      public DateTime OfferingStartTime { set; get; }
                     
      public DateTime OfferingEndTime { set; get; }
        public string OfferShortDetail { set; get; }
        public string OfferFullDetail { set; get; }
        public string CountryName { set; get; }
        public string CityName { set; get; }
        public byte[] OfferPictureData { set; get; }
       
        
        public int VendorTypeId { set; get; }
        public int CountryId { set; get; }
        public int CityId { set; get; }
        public int OfferTypeId { set; get; }
        public string OfferTypeName { set; get; }
          public bool OfferStatus { set; get; }
          public bool OfferAdminPermit { set; get; }
    }
}