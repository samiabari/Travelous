using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Models.DB;

namespace Travel.Models.BusinessLogic
{
    public class OfferLogic
    {
        TourDBEntities entity = new TourDBEntities();

        #region Add Offer
      
        public bool CheckVendorIdInOffer(int VendorId)
        {
            bool result;

            var vendorId = (from c in entity.Offers where c.VendorId == VendorId select c.VendorId).FirstOrDefault();
            int t = Convert.ToInt32(vendorId);
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
        public bool CheckTypeId(int VendorId, int OfferTypeId)
        {
            bool result;

            result = (from c in entity.Offers where c.VendorId == VendorId && c.OfferTypeId == OfferTypeId select c.OfferTypeId).Any();


            return result;
        }
        public bool CheckOverlap(DateTime StartTime, DateTime StopTime, int VendorId, int OfferTypeId)

        {
            bool result =
                entity.Offers.Where(s => s.VendorId == VendorId && s.OfferTypeId == OfferTypeId)
                    .Any(s => (s.OfferStartTime >= StartTime && s.OfferStartTime <= StopTime) ||
                              (s.OfferStopTime <= StopTime && s.OfferStopTime >= StartTime));

            return result;
        }



        public bool CheckNameExistance(string OfferName, int VendorId)
        {
            bool result = (from c in entity.Offers where c.VendorId == VendorId && c.OfferName == OfferName  select c).Any();
            return result;
        }

        #endregion


        #region PageLoad Deactivation Of expired Offer

        public void DeactivateAllExpiredOffer()
        {
            var offer = (from c in entity.Offers where c.OfferStopTime < DateTime.Now && c.OfferStatus == true select c).ToList();
            foreach (var item in offer)
            {
                item.OfferStatus = false;
                bool result = (from c in entity.Offers where c.VendorId == item.VendorId && c.OfferStopTime >= DateTime.Now select c).Any();
                if (result == false)
                {
                    Vendor vendor =
                        (from c in entity.Vendors where c.VendorId == item.VendorId select c).FirstOrDefault();
                    vendor.VendorOfferStatus = false;
                }

            }
            entity.SaveChanges();
        }

        #endregion


        #region  Deactivation Of  Offer

        public void DeactivateOffer(int OfferId)
        {

            Offer host = (from c in entity.Offers where c.OfferId == OfferId select c).FirstOrDefault();
            int touristId = Convert.ToInt32(host.VendorId);
            host.OfferStatus = false;
            entity.SaveChanges();
            bool Result = (from c in entity.Offers where c.VendorId == touristId && c.OfferStatus == true select c).Any();
            if (Result == false)
            {
                Vendor tourist =
                        (from c in entity.Vendors where c.VendorId == touristId select c).FirstOrDefault();
                tourist.VendorOfferStatus = false;
                entity.SaveChanges();

            }


        }
        #endregion

    }
}