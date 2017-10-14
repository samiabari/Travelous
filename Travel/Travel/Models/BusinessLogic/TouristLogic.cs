using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Travel.Models.DB;

namespace Travel.Models.BusinessLogic
{
    public class TouristLogic
    {

        TourDBEntities entity = new TourDBEntities();


        #region SignUp
        public bool CheckEmailExist(string TouristEmail)
        {
            bool result;
            Tourist tour = new Tourist();
            tour = (from c in entity.Tourists where c.TouristEmail == TouristEmail select c).SingleOrDefault();
            if (tour != null)
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



        #region sign in

        public bool SignInLogic(string TouristEmail, string TouristPassword)
        {

            bool result;
            string userCode = "tourist";
            Tourist TouristInfo = new Tourist();
            TouristInfo = (from c in entity.Tourists where c.TouristEmail == TouristEmail && c.TouristPassword == TouristPassword select c).SingleOrDefault();
            if (TouristInfo != null)
            {
                HttpContext.Current.Session["TouristId"] = Convert.ToInt32(TouristInfo.TouristId);
                HttpContext.Current.Session["TouristName"] = TouristInfo.TouristName.ToString();

                HttpContext.Current.Session["TouristEmail"] = TouristInfo.TouristEmail.ToString();

                if(TouristInfo.TouristsStatus==false)
                {
                    TouristInfo.TouristsStatus = true;
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

        public bool CheckTouristIdnAlbumTypeIdInPictureAlbum(int TouristId, int AlbumTypeId)
        {
            bool result;
            result = (from c in entity.TouristAlbums where c.TouristId == TouristId && c.AlbumTypeId == AlbumTypeId select c).Any();

            return result;
        }

        #endregion


       
    }
}