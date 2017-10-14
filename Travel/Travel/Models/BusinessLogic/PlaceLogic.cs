using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Travel.Models.DB;

namespace Travel.Models.BusinessLogic
{
    public class PlaceLogic
    {

        TourDBEntities entity = new TourDBEntities();



        #region Add Place
        public bool CheckPlaceExist(string PlaceName, int CountryId, int CityId)
        {
            bool result;
            Place place = new Place();
            place = (from c in entity.Places where c.PlaceName == PlaceName && c.CountryId == CountryId && c.CityId == CityId select c).SingleOrDefault();
            if (place != null)
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



        #region add Profile picture

        public bool CheckPlaceIdAlbumTypeIdInPictureAlbum(int PlaceId, int AlbumTypeId)
        {
            bool result;
            result = (from c in entity.PlaceAlbums where c.PlaceId == PlaceId && c.AlbumTypeId == AlbumTypeId select c).Any();

            return result;
        }



        #endregion
        public bool CheckHos(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Places where c.CountryId == countryId && c.CityId == cityId && c.PlaceTypeId == 2 select c).Any();
            return result;
        }

        public bool Market(int countryId, int cityId)
        {
            bool result;
            result = (from c in entity.Places where c.CountryId == countryId && c.CityId == cityId && c.PlaceTypeId == 4 select c).Any();
            return result;
        }





        #region Block Place
        public bool BlockPlace(int PlaceId)
        {
            bool result = false;
            Place place = entity.Places.Where(x => x.PlaceId == PlaceId).First();
            place.PlaceAdminsPermit = false;

            if (entity.SaveChanges() > 0)
            {
                result = true;
            }
            return result;
        }
        #endregion



        #region UnBlock Place
        public bool UnBlockPlace(int PlaceId)
        {
            bool result = false;
            Place place = entity.Places.Where(x => x.PlaceId == PlaceId).First();
            place.PlaceAdminsPermit = true;

            if (entity.SaveChanges() > 0)
            {
                result = true;
            }
            return result;
        }


        #endregion


        #region check in
      

        public bool CheckCheckIn(int touristId, int placeId)
        {
            bool result = (from c in entity.CheckIns where c.TouristId==touristId && c.PlaceId==placeId && c.CheckInDate==DateTime.Today select c).Any();
                return result;
        }

        public void CheckInPlace(int placeId, int touristId)
        {
            
            CheckIn ch = new CheckIn();
            Place place = (from c in entity.Places where c.PlaceId==placeId select c).First();
            ch.PlaceId = placeId;
            ch.CountryId = place.CountryId;
            ch.TouristId = touristId;
            ch.CityId = place.CityId;
            ch.CheckInDate = DateTime.Today;
            int like = Convert.ToInt32(place.PlaceFavourite);
            like = like + 1;
            place.PlaceFavourite = like;
            entity.CheckIns.Add(ch);
            entity.SaveChanges();

        }

        public bool CheckWishIn(int touristId, int placeId)
        {
            bool result = (from c in entity.WishLists where c.TouristId == touristId && c.PlaceId == placeId select c).Any();
            return result;
        }

        public void AddWishPlace(int placeId, int touristId)
        {

            WishList ch = new WishList();
            Place place = (from c in entity.Places where c.PlaceId == placeId select c).First();
            ch.PlaceId = placeId;
            ch.CountryId = place.CountryId;
            ch.TouristId = touristId;
            ch.CityId = place.CityId;
            ch.WishAddedDate = DateTime.Today;
            int like = Convert.ToInt32(place.PlaceFavourite);
            like = like + 1;
            place.PlaceFavourite = like;
            entity.WishLists.Add(ch);
            entity.SaveChanges();

        }

        #endregion
    }
}