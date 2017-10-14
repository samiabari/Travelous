using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Models.DB;

namespace Travel.Models.BusinessLogic
{
    public class HostLogic
    {
        TourDBEntities entity = new TourDBEntities();


        #region BeHost

        public bool CheckTouristIdInHost(int TouristId)
        {
            bool result;

            var touristId = (from c in entity.Hosts where c.TouristId == TouristId select c.TouristId).FirstOrDefault();
            int t = Convert.ToInt32(touristId);
            if (t != 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool CheckAddressNTypeId(int TouristId, string HostingPlaceAddress,int HostTypeId)
        {
            bool result;

            var touristadd = (from c in entity.Hosts where c.TouristId == TouristId && c.HostingPlaceAddress == HostingPlaceAddress && c.HostTypeId==HostTypeId select c.HostingPlaceAddress).FirstOrDefault();
            string t = Convert.ToString(touristadd);
            if (t != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public bool CheckOverlap(DateTime StartTime, DateTime StopTime, int TouristId, string HostingPlaceAddress,int HostTypeId)
        {
            bool result =
                entity.Hosts.Where(s => s.TouristId == TouristId && s.HostingPlaceAddress == HostingPlaceAddress && s.HostTypeId== HostTypeId)
                    .Any(s => (s.HostingStartTime >= StartTime && s.HostingStartTime <= StopTime) ||
                              (s.HostingStopTime <= StopTime && s.HostingStopTime >= StartTime));

            return result;
        }

        #endregion




        #region  Deactivation Of  Host

        public void DeactivateHost(int HostId)
        {

            Host host = (from c in entity.Hosts where c.HostId== HostId  select c).FirstOrDefault();
            int touristId = Convert.ToInt32(host.TouristId);
            host.HostingStatus = false;
            entity.SaveChanges();
            bool Result = (from c in entity.Hosts where c.TouristId == touristId && c.HostingStatus == true select c).Any();
            if (Result == false)
            {
                Tourist tourist =
                        (from c in entity.Tourists where c.TouristId == touristId select c).FirstOrDefault();
                tourist.TouristsHostStatus = false;
                entity.SaveChanges();

            }


        }

        #endregion



        #region PageLoad Deactivation Of expired Host

        public void DeactivateAllExpiredHost()
        {
            var host = (from c in entity.Hosts where c.HostingStopTime < DateTime.Now && c.HostingStatus == true select c).ToList();
            foreach (var item in host)
            {
                item.HostingStatus = false;
                bool result = (from c in entity.Hosts where c.TouristId == item.TouristId && c.HostingStopTime >= DateTime.Now select c).Any();
                if (result == false)
                {
                    Tourist tourist =
                        (from c in entity.Tourists where c.TouristId == item.TouristId select c).FirstOrDefault();
                    tourist.TouristsHostStatus = false;
                }

            }
            entity.SaveChanges();
        }



        #endregion

       public bool HostCheck(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Hosts where c.HostingCountryId == countryId && c.HostingCityId == cityId && c.HostingStatus == true select c).Any();
            return result;
        }

        internal bool HostCheckwDate(int countryId, int cityId, DateTime a, DateTime de)
        {
            bool result;
            result = (from c in entity.Hosts where c.HostingCountryId == countryId && c.HostingCityId == cityId&&c.HostingStartTime>=a &&c.HostingStopTime>=de && c.HostingStatus == true select c).Any();
            return result;
        }
    }
}