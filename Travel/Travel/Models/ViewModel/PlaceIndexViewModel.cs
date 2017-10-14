using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class PlaceIndexViewModel
    {
        public byte[] PlacePictureData { set; get; }
        public int PlaceId { set; get; }
        public string PlaceName { set; get; }
       
    }
}