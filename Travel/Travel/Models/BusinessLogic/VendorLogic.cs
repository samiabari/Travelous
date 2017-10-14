using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Models.DB;
using Travel.Models.ViewModel;

namespace Travel.Models.BusinessLogic
{
    public class VendorLogic
    {

        TourDBEntities entity = new TourDBEntities();
        Admin ad = new Admin();


        #region SignUp
        public bool CheckVendorEmailExist(int cityId, string vendorEmail)
        {
            bool result;
            Vendor vendor = new Vendor();
            vendor = (from c in entity.Vendors where c.VendorEmail == vendorEmail && c.CityId == cityId select c).SingleOrDefault();
            if (vendor != null)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;

        }
        #endregion


   

        #region SignIn
        public bool SignInLogic(string Email, string Password)
        {


            bool result;
            string userCode = "vendor";
            Vendor VendorInfo = new Vendor();
            VendorInfo = (from c in entity.Vendors where c.VendorEmail == Email && c.VendorPassword == Password select c).FirstOrDefault();
            if (VendorInfo != null)
            {
                HttpContext.Current.Session["VendorId"] = Convert.ToInt32(VendorInfo.VendorId);

                HttpContext.Current.Session["VendorOfficeName"] = VendorInfo.VendorOfficeName.ToString();
                HttpContext.Current.Session["VendorEmail"] = VendorInfo.VendorEmail.ToString();
              
                HttpContext.Current.Session["VendorLike"] = Convert.ToInt32(VendorInfo.VendorFavourite);
          

                if (VendorInfo.VendorsStatus == false)
                {
                    VendorInfo.VendorsStatus = true;
                    entity.SaveChanges();
                }

                result = true;
                HttpContext.Current.Session["userCode"] = userCode.ToString();
            }
            else
            {
                result = false;
            }


            return result;

        }
        #endregion




        #region add Profile picture

        public bool CheckVendorIdnAlbumTypeIdInVendorAlbum(int VendorId, int AlbumTypeId)
        {
            bool result;
            result = (from c in entity.VendorAlbums where c.VendorId == VendorId && c.AlbumTypeId == AlbumTypeId select c).Any();

            return result;
        }



        #endregion

        public bool CheckPhoto(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Vendors where c.CountryId == countryId && c.CityId == cityId && c.VendorTypeId == 13 select c).Any();
            return result;
        }

        public bool CheckLaun(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Vendors where c.CountryId == countryId && c.CityId == cityId && c.VendorTypeId == 9 select c).Any();
            return result;
        }
        public bool FoodDe(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Vendors where c.CountryId == countryId && c.CityId == cityId && c.VendorTypeId == 4 select c).Any();
            return result;
        }
        public bool Guid(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Vendors where c.CountryId == countryId && c.CityId == cityId && c.VendorTypeId == 7 select c).Any();
            return result;
        }

       public bool BlockVendor(int vendorId)
        {
            bool result=false;
             Vendor tour = (from c in entity.Vendors where c.VendorId == vendorId select c).FirstOrDefault();
            tour.VendorAdminsPermit = false;
            string mail = tour.VendorEmail;
            
            if (tour.VendorOfferStatus == true)
            {
                var offer = (from c in entity.Offers where c.VendorId == vendorId && c.OfferAdminsPermit == true select c).ToList();
                foreach (var i in offer)
                {
                    i.OfferAdminsPermit = false;
                }
            }

            if (entity.SaveChanges() > 0)
            {
                //string body = " due to some rules violation Your Vendorship account and offers(if there is) has been seized on our site. please contact with admin for further inquery. Thank you! ";
                //string Subject = "Vendorship account blockage by admin";
                //ad.SentMail(mail, body, Subject);
                return result=true;
            }
            else
            {
                return result = false;
            }
        }

        public bool UnBlockVendor(int vendorId)
        {
            bool result=false;
            Vendor tour = (from c in entity.Vendors where c.VendorId == vendorId select c).FirstOrDefault();
            tour.VendorAdminsPermit = true;
        //    string mail = tour.VendorEmail;

            if (tour.VendorOfferStatus == true)
            {
                var offer = (from c in entity.Offers where c.VendorId == vendorId && c.OfferAdminsPermit == false select c).ToList();
                foreach (var i in offer)
                {
                    i.OfferAdminsPermit = true;
                }
            }

            if (entity.SaveChanges() > 0)
            {
                ////string body = " due to some rules violation Your Vendorship account and offers(if there is) was been seized on our site. No by your request it has been open again. Thank you! ";
                ////string Subject = "Vendorship account blockage by admin";
                ////ad.SentMail(mail, body, Subject);
                return result = true;
            }
            else
            {
                return result = false;
            }
        }





        #region check in


        public bool CheckCheckIn(int touristId, int VendorId)
        {
            bool result = (from c in entity.CheckIns where c.TouristId == touristId && c.VendorId == VendorId && c.CheckInDate == DateTime.Today select c).Any();
            return result;
        }

        public void CheckInPlace(int VendorId, int touristId)
        {

            CheckIn ch = new CheckIn();
            Vendor vendor = (from c in entity.Vendors where c.VendorId == VendorId select c).First();
            ch.VendorId = VendorId;
            ch.CountryId = vendor.CountryId;
            ch.TouristId = touristId;
            ch.CityId = vendor.CityId;
            ch.CheckInDate = DateTime.Today;
            int like = Convert.ToInt32(vendor.VendorFavourite);
            like = like + 1;
            vendor.VendorFavourite = like;
            entity.CheckIns.Add(ch);
            entity.SaveChanges();

        }

        public bool CheckWishIn(int touristId, int VendorId)
        {
            bool result = (from c in entity.WishLists where c.TouristId == touristId && c.VendorId == VendorId select c).Any();
            return result;
        }

        public void AddWishVendor(int VendorId, int touristId)
        {

            WishList ch = new WishList();
            Vendor vendor = (from c in entity.Vendors where c.VendorId == VendorId select c).First();
            ch.VendorId = VendorId;
            ch.CountryId = vendor.CountryId;
            ch.TouristId = touristId;
            ch.CityId = vendor.CityId;
            ch.WishAddedDate = DateTime.Today;
            int like = Convert.ToInt32(vendor.VendorFavourite);
            like = like + 1;
            vendor.VendorFavourite = like;
            entity.WishLists.Add(ch);
            entity.SaveChanges();

        }

        #endregion
    }
}