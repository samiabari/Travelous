using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Travel.Models.ViewModel
{
    public class HostDetailSearchViewModel
    {
        public int HostId {set;get;}
        public int TouristId { set; get; }
        public string HostName { set; get; }
        public string HostAddress { set; get; }
        public string HostPrice { set; get; }
        public string HostPhnNo { set; get; }
        public DateTime HostingStartTime { set; get; }

        public DateTime HostingEndTime { set; get; }
        public string HostShortDetail { set; get; }
        public string HostFullDetail { set; get; }
        public string CountryName { set; get; }
        public string CityName { set; get; }
        public byte[] HostPictureData { set; get; }
     
     
        public int CountryId { set; get; }
        public int CityId { set; get; }
        public int HostTypeId { set; get; }
        public string HostTypeName { set; get; }
        public bool HostStatus { set; get; }
        public bool hostAdminPermit { set; get; } 


    }
}