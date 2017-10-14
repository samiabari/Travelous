using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class VendorDetailViewModel
    {
     
        public int VendorId { set; get; }
        public string VendorName { set; get; }
        public string VendorTypeName { set; get; }
        public int VendorTypeId { set; get; }
        public string VendorAddress { set; get; }
        public string VendorWebsite { set; get; }
        public string VendorPhnNo { set; get; }
       
        public string VendorShortDetail { set; get; }
        public string VendorFullDetail { set; get; }
        public string CountryName { set; get; }
        public string CityName { set; get; }
        public byte[] VendorPictureData { set; get; }
        public int VendorLike { set; get; }
      
     
        public bool VendorStatus { set; get; }
        public int CountryId { set; get; }
        public int CityId { set; get; }
        public bool VendorOfferStatus { set; get; }
        public bool VendorAdminPermit { set; get; }
    }
}