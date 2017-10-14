using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class RestaurentVendorViewModel
    {
        public byte[] VendorPictureData { set; get; }
        public int VendorId { set; get; }
        public string VendorName { set; get; }
    }
}