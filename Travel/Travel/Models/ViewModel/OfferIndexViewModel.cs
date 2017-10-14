using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class OfferIndexViewModel
    {
        public byte[] OfferPictureData { set; get; }
        public int OfferId { set; get; }
        public int VendorId { set; get; }
        public int VendorTypeId { set; get; }
        public string OfferName { set; get; }
        public string VendorName { set; get; }

      
    }
}