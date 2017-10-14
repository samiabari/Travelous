using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Travel.Models.BusinessLogic;
using Travel.Models.DB;
using Travel.Models.ViewModel;

namespace Travel.Controllers
{
    public class PlaceController : Controller
    {
        TourDBEntities entity = new TourDBEntities();
        PlaceLogic pl = new PlaceLogic();




        #region Place add Plus propic addition if given
        [AllowAnonymous]
        public ActionResult AddPlace()
        {
            if (Session["userCode"] != null)
            {
                AddPlaceViewModel apvm = new AddPlaceViewModel();

                apvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");



                apvm.PlaceTypeList = new SelectList(entity.PlaceTypes, "PlaceTypeId", "PlaceTypeName");
                return View(apvm);
            }
            else
            {
                Session["message"] = "please log in to do so.";
                return RedirectToAction("Index", "Home");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AddPlace(AddPlaceViewModel apvm, HttpPostedFileBase AddPicture)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    int countryId = Convert.ToInt32(apvm.CountryId);
                    int cityId = Convert.ToInt32(apvm.CityId);
                    bool result;
                    result = pl.CheckPlaceExist(apvm.PlaceName, countryId, cityId);
                    if (result == false)
                    {

                        int placeTypeId = Convert.ToInt32(apvm.PlaceTypeId);


                        Place place = new Place();
                        place.PlaceName = apvm.PlaceName;
                        place.PlaceTypeId = Convert.ToInt32(apvm.PlaceTypeId);

                        place.CountryId = Convert.ToInt32(apvm.CountryId);
                        place.CityId = Convert.ToInt32(apvm.CityId);
                        place.PlaceAddress = apvm.PlaceAddress;
                        if (apvm.PlaceDetail == null)
                        {
                            place.PlaceDetail = "Not Given yet.";
                        }
                        else
                        {

                            place.PlaceDetail = apvm.PlaceDetail;
                        }

                        place.PlaceAdminsPermit = true;
                        place.PlaceDateOfAccountCreation = DateTime.Now;
                        place.PlaceFavourite = 0;
                        PlacePicture pp = new PlacePicture();
                        entity.Places.Add(place);

                        if (entity.SaveChanges() > 0)
                        {

                            var p = (from c in entity.Places where c.PlaceName == apvm.PlaceName && c.CountryId == apvm.CountryId && c.CityId == apvm.CityId select new { c.PlaceId, c.CountryId, c.CityId }).FirstOrDefault();
                            if (AddPicture != null)
                            {

                                PlaceAlbum pa = new PlaceAlbum();
                                pa.PlaceAlbumName = "Profile Picture";
                                pa.PlaceId = p.PlaceId;
                                pa.AlbumTypeId = 1;
                                pa.CityId = p.CityId;
                                pa.CountryId = p.CountryId;
                                pa.PlaceAlbumAdminPermit = true;
                                pa.PlaceAlbumPivacy = true;
                                pa.PlaceAlbumDateOfCreation = DateTime.Now;
                                entity.PlaceAlbums.Add(pa);
                                entity.SaveChanges();
                                pp.PlacePictureData = new byte[AddPicture.ContentLength];
                                AddPicture.InputStream.Read(pp.PlacePictureData, 0, AddPicture.ContentLength);
                                pp.PlaceId = p.PlaceId;
                                var albumId = (from c in entity.PlaceAlbums where c.PlaceId == p.PlaceId && c.AlbumTypeId == 1 select c.PlaceAlbumId).Single();
                                int AlbumId = Convert.ToInt32(albumId);
                                pp.PlaceAlbumId = AlbumId;
                                pp.AlbumTypeId = 1;
                                pp.PlacePictureAdminPermit = true;
                                pp.CountryId = p.CountryId;
                                pp.CityId = p.CityId;

                                pp.PlacePicturePrivacy = true;
                                pp.PlacePictureDateOfAdded = DateTime.Now;
                                entity.PlacePictures.Add(pp);
                                if (entity.SaveChanges() > 0)
                                {
                                    Place d= (from c in entity.Places where c.PlaceId == p.PlaceId select c).FirstOrDefault();
                                    int pictureId = (from c in entity.PlacePictures
                                                     where c.PlaceId == p.PlaceId && c.AlbumTypeId == 1
                                                     orderby c.PlacePictureId descending
                                                     select c.PlacePictureId).First();
                                    d.PlaceProfilePicId = pictureId;
                                    entity.SaveChanges();


                                }



                            }
                            ModelState.Clear();
                            TempData["Success"] = "Added Successfully";

                        }
                        else
                        {

                            TempData["Success"] = "Problem occured";
                        }

                    }
                    else
                    {

                        TempData["Success"] = "Duplicacy found";
                    }
                    AddPlaceViewModel apvm1 = new AddPlaceViewModel();
                    apvm1.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");



                    apvm1.PlaceTypeList = new SelectList(entity.PlaceTypes, "PlaceTypeId", "PlaceTypeName");



                    return View(apvm1);
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "AddPlace"));
                }

            }
            else
            {
                AddPlaceViewModel apvm1 = new AddPlaceViewModel();
                apvm1.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");



                apvm1.PlaceTypeList = new SelectList(entity.PlaceTypes, "PlaceTypeId", "PlaceTypeName");



                return View(apvm1);
            }

        }


        #endregion


        #region Add ProfilePicture Of a Place

        [HttpPost]
        public ActionResult AddProfilePic(int PlaceId, int albumTypeId, HttpPostedFileBase AddPicture)
        {
            PlaceLogic pl = new PlaceLogic();
            PlacePicture pp = new PlacePicture();
            if ((string)Session["userCode"] == "admin")
            {
                try
                {


                    var p = (from c in entity.Places where c.PlaceId == PlaceId select new { c.CountryId, c.CityId }).FirstOrDefault();

                    bool result = pl.CheckPlaceIdAlbumTypeIdInPictureAlbum(PlaceId, albumTypeId);
                    Place place = (from c in entity.Places where c.PlaceId == PlaceId select c).FirstOrDefault();
                    if (result == true)
                    {
                        var albumId = (from c in entity.PlaceAlbums where c.PlaceId == PlaceId && c.AlbumTypeId == albumTypeId select c.PlaceAlbumId).Single();
                        int AlbumId = Convert.ToInt32(albumId);
                        if (AddPicture != null)
                        {
                            pp.PlacePictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(pp.PlacePictureData, 0, AddPicture.ContentLength);


                        }
                        else
                        {
                            TempData["message"] = "no picture has been selected";
                            return RedirectToAction("PlaceDetail", new { PlaceId = PlaceId });
                        }
                        pp.PlaceId = PlaceId;
                        pp.PlaceAlbumId = AlbumId;
                        pp.AlbumTypeId = albumTypeId;
                        pp.PlacePictureAdminPermit = true;
                        pp.CountryId = p.CountryId;
                        pp.CityId = p.CityId;
                        pp.PlacePictureAdminPermit = true;
                        pp.PlacePicturePrivacy = true;
                        pp.PlacePictureDateOfAdded = DateTime.Now;
                        entity.PlacePictures.Add(pp);
                        if (entity.SaveChanges() > 0)
                        {

                            int pictureId = (from c in entity.PlacePictures
                                             where c.PlaceId == PlaceId && c.AlbumTypeId == albumTypeId
                                             orderby c.PlacePictureId descending
                                             select c.PlacePictureId).First();
                            place.PlaceProfilePicId = pictureId;
                            entity.SaveChanges();
                            TempData["PlaceProPic"]= "Profile Picture added";

                            
                                return RedirectToAction("SearchPlaceDetail", "Place",new { PlaceId=PlaceId});
                           

                        }
                        else
                        {
                            TempData["PlaceProPic"] = "not added";

                            return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });

                        }
                    }
                    else
                    {
                        if (AddPicture != null)
                        {
                            pp.PlacePictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(pp.PlacePictureData, 0, AddPicture.ContentLength);



                        }
                        else
                        {
                            TempData["PlaceProPic"] = "no picture has been selected";

                            return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });



                        }

                        PlaceAlbum pa = new PlaceAlbum();
                        pa.PlaceAlbumName = "Profile Picture";
                        pa.PlaceId = PlaceId;
                        pa.AlbumTypeId = albumTypeId;
                        pa.CityId = p.CityId;
                        pa.CountryId = p.CountryId;
                        pa.PlaceAlbumAdminPermit = true;
                        pa.PlaceAlbumPivacy = true;
                        pa.PlaceAlbumDateOfCreation = DateTime.Now;
                        entity.PlaceAlbums.Add(pa);
                        entity.SaveChanges();
                        pp.PlaceId = PlaceId;
                        var albumId = (from c in entity.PlaceAlbums where c.PlaceId == PlaceId && c.AlbumTypeId == albumTypeId select c.PlaceAlbumId).Single();
                        int AlbumId = Convert.ToInt32(albumId);
                        pp.PlaceAlbumId = AlbumId;
                        pp.AlbumTypeId = albumTypeId;
                        pp.PlacePictureAdminPermit = true;
                        pp.CountryId = p.CountryId;
                        pp.CityId = p.CityId;

                        pp.PlacePicturePrivacy = true;
                        pp.PlacePictureDateOfAdded = DateTime.Now;
                        entity.PlacePictures.Add(pp);
                        if (entity.SaveChanges() > 0)
                        {

                            int pictureId = (from c in entity.PlacePictures
                                             where c.PlaceId == PlaceId && c.AlbumTypeId == albumTypeId
                                             orderby c.PlacePictureId descending
                                             select c.PlacePictureId).First();
                            place.PlaceProfilePicId = pictureId;
                            entity.SaveChanges();

                            TempData["PlaceProPic"] = "Picture Added";

                            return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });




                        }
                        else
                        {
                            TempData["PlaceProPic"] = "No added";

                            return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });



                        }

                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "AddProfilePic"));
                }

            }
            else
            {
                Session["message"] = "please log in First";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion


        #region Placelist from index

        public ActionResult SearchPlaceList(int CountryId, int CityId)
        {

            List<PlaceDetailViewModel> sp = new List<PlaceDetailViewModel>();
            Session["SearchPlaceList"] = "yes";
            if (CountryId != 0 && CityId != 0)
            {
                var p = (from c in entity.Places
                         join e in entity.PlacePictures on c.PlaceProfilePicId
                         equals e.PlacePictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                       
                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                         where c.CountryId == CountryId && c.CityId == CityId &&  c.PlaceAdminsPermit == true
                         orderby c.PlaceFavourite descending
                         select new
                         {
                             c.PlaceId,
                             c.PlaceName,
                             e.PlacePictureData,
                          
                             k.PlaceTypeId,
                             c.PlaceAddress,
                             c.PlaceFavourite,
                            
                             f.CountryName,
                             g.CityName,
                             g.CityId,
                             f.CountryId,
                             c.PlaceDetail,
                         
                             k.PlaceTypeName,
                            
                             c.PlaceAdminsPermit,
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        PlaceDetailViewModel oivm = new PlaceDetailViewModel();
                        oivm.PlacePictureData = i.PlacePictureData;
                        oivm.PlaceId = i.PlaceId;
                      
                        oivm.PlaceName = i.PlaceName;
                        oivm.PlaceAddress = i.PlaceAddress;
                        oivm.PlaceLike = Convert.ToInt32(i.PlaceFavourite);
                    
                        oivm.PlaceTypeName = i.PlaceTypeName;
                        oivm.CityName = i.CityName;
                        oivm.CountryName = i.CountryName;
                        string st = CutDetail.StripHTML(i.PlaceDetail);
                        oivm.PlaceShortDetail = CutDetail.Truncate(st, 40);
                        oivm.PlaceFullDetail = i.PlaceDetail;
                        oivm.PlaceCityId = i.CityId;
                        oivm.PlaceCountryId = i.CountryId;
                        oivm.PlaceTypeId = i.PlaceTypeId;
                        oivm.PlaceAdminPermit = Convert.ToBoolean(i.PlaceAdminsPermit);
                    
                        sp.Add(oivm);
                    }
                    var q = sp;
                    TempData["CountryId"] = CountryId;
                    TempData["CityId"] = CityId;

                    return View(q);

                }

                else
                { 
                    
                   
                    return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = "", Departure = "" });
                }
            }
            else
            {
                return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = "", Departure = "" });
            }
        }
        #endregion




        #region Placelist from index

        public ActionResult SearchPlaceListWithType(int CountryId, int CityId,int PlaceTypeId)
        {
            Session["SearchPlaceTypeId"] = PlaceTypeId;
            List<PlaceDetailViewModel> sp = new List<PlaceDetailViewModel>();
            Session["SearchPlaceListType"] = "yes";
            if (CountryId != 0 && CityId != 0)
            {
                var p = (from c in entity.Places
                         join e in entity.PlacePictures on c.PlaceProfilePicId
                         equals e.PlacePictureId into ppl
                         from e in ppl.DefaultIfEmpty()

                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                         where c.CountryId == CountryId && c.CityId == CityId && c.PlaceTypeId==PlaceTypeId && c.PlaceAdminsPermit == true
                         orderby c.PlaceFavourite descending
                         select new
                         {
                             c.PlaceId,
                             c.PlaceName,
                             e.PlacePictureData,

                             k.PlaceTypeId,
                             c.PlaceAddress,
                             c.PlaceFavourite,

                             f.CountryName,
                             g.CityName,
                             g.CityId,
                             f.CountryId,
                             c.PlaceDetail,

                             k.PlaceTypeName,

                             c.PlaceAdminsPermit,
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        PlaceDetailViewModel oivm = new PlaceDetailViewModel();
                        oivm.PlacePictureData = i.PlacePictureData;
                        oivm.PlaceId = i.PlaceId;

                        oivm.PlaceName = i.PlaceName;
                        oivm.PlaceAddress = i.PlaceAddress;
                        oivm.PlaceLike = Convert.ToInt32(i.PlaceFavourite);

                        oivm.PlaceTypeName = i.PlaceTypeName;
                        oivm.CityName = i.CityName;
                        oivm.CountryName = i.CountryName;
                        string st = CutDetail.StripHTML(i.PlaceDetail);
                        oivm.PlaceShortDetail = CutDetail.Truncate(st, 40);
                        oivm.PlaceFullDetail = i.PlaceDetail;
                        oivm.PlaceCityId = i.CityId;
                        oivm.PlaceCountryId = i.CountryId;
                        oivm.PlaceTypeId = i.PlaceTypeId;
                        oivm.PlaceAdminPermit = Convert.ToBoolean(i.PlaceAdminsPermit);

                        sp.Add(oivm);
                    }
                    var q = sp;
                    TempData["CountryId"] = CountryId;
                    TempData["CityId"] = CityId;

                    return View(q);

                }

                else
                {


                    return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = "", Departure = "" });
                }
            }
            else
            {
                return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = "", Departure = "" });
            }
        }
        #endregion

        #region placedetails

        public ActionResult SearchPlaceDetail(int PlaceId)
        {
            if (TempData["CheckInPlaceSearch"] != null)
            {
                TempData["CheckInPlaceSearch"] =TempData["CheckInPlaceSearch"];
                     }
            if (TempData["AddToWishPlaceSearch"] != null)
            {
                TempData["AddToWishPlaceSearch"] = TempData["AddToWishPlaceSearch"];
            }
            if (TempData["PlaceBlock"] != null)
            {
                TempData["PlaceBlock"] = TempData["PlaceBlock"];
            }
            if (TempData["PlaceProPic"] != null)
            { TempData["PlaceProPic"] = TempData["PlaceProPic"];
            }
            if (TempData["EditPlace"] != null)
            {
                TempData["EditPlace"] = TempData["EditPlace"];
            }
            PlaceDetailViewModel sp = new PlaceDetailViewModel();

            if (PlaceId != 0)
            {
                var p = (from c in entity.Places
                         join e in entity.PlacePictures on c.PlaceProfilePicId
                         equals e.PlacePictureId into ppl
                         from e in ppl.DefaultIfEmpty()

                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                         where c.PlaceId== PlaceId
                         orderby c.PlaceFavourite descending
                         select new
                         {
                             c.PlaceId,
                             c.PlaceName,
                             e.PlacePictureData,

                             k.PlaceTypeId,
                             c.PlaceAddress,
                             c.PlaceFavourite,
                        
                             f.CountryName,
                             g.CityName,

                             c.PlaceDetail,
                             g.CityId,
                             f.CountryId,
                             k.PlaceTypeName,

                             c.PlaceAdminsPermit,
                         }).First();
              
                if (p!=null)
                {
                  
                        sp.PlacePictureData = p.PlacePictureData;
                        sp.PlaceId = p.PlaceId;
                     
                        sp.PlaceName = p.PlaceName;
                        sp.PlaceAddress = p.PlaceAddress;
                        sp.PlaceLike = Convert.ToInt32(p.PlaceFavourite);
                     
                        sp.PlaceTypeName = p.PlaceTypeName;
                        sp.CityName = p.CityName;
                        sp.CountryName = p.CountryName;
                        string st = CutDetail.StripHTML(p.PlaceDetail);
                        sp.PlaceShortDetail = CutDetail.Truncate(st, 40);
                        sp.PlaceFullDetail = p.PlaceDetail;
                  sp.PlaceCityId = p.CityId;
                    sp.PlaceCountryId = p.CountryId;
                    sp.PlaceTypeId = p.PlaceTypeId;
                        sp.PlaceAdminPermit = Convert.ToBoolean(p.PlaceAdminsPermit);

                        
                    }
                    var q = sp;
               
                return View(q);

                }

                else
                {
                
               
              
                    return RedirectToAction("UserPanel", "Home");
                }
         
            }


        #endregion


        #region New PlaceList Admin
        public ActionResult NewPlaceListAdmin()
        {

            List<PlaceDetailViewModel> sp = new List<PlaceDetailViewModel>();
            DateTime t = DateTime.Now.AddDays(-2);
             var p = (from c in entity.Places
                         join e in entity.PlacePictures on c.PlaceProfilePicId
                         equals e.PlacePictureId into ppl
                         from e in ppl.DefaultIfEmpty()

                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                         where  c.PlaceDateOfAccountCreation>=t
                         orderby c.PlaceFavourite descending
                         select new
                         {
                             c.PlaceId,
                             c.PlaceName,
                             e.PlacePictureData,

                             k.PlaceTypeId,
                          
                             c.PlaceAddress,
                             c.PlaceFavourite,
                      
                             f.CountryName,
                             g.CityName,
                             g.CityId,
                             f.CountryId,
                             c.PlaceDetail,

                             k.PlaceTypeName,

                             c.PlaceAdminsPermit,
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        PlaceDetailViewModel oivm = new PlaceDetailViewModel();
                        oivm.PlacePictureData = i.PlacePictureData;
                        oivm.PlaceId = i.PlaceId;

                        oivm.PlaceName = i.PlaceName;
                        oivm.PlaceAddress = i.PlaceAddress;
                        oivm.PlaceLike = Convert.ToInt32(i.PlaceFavourite);
                       
                        oivm.PlaceTypeName = i.PlaceTypeName;
                        oivm.CityName = i.CityName;
                        oivm.CountryName = i.CountryName;
                        string st = CutDetail.StripHTML(i.PlaceDetail);
                        oivm.PlaceShortDetail = CutDetail.Truncate(st, 40);
                        oivm.PlaceFullDetail = i.PlaceDetail;
                    oivm.PlaceCityId = i.CityId;
                    oivm.PlaceCountryId = i.CountryId;
                    oivm.PlaceTypeId = i.PlaceTypeId;
                        oivm.PlaceAdminPermit = Convert.ToBoolean(i.PlaceAdminsPermit);

                        sp.Add(oivm);
                    }
                    var q = sp;
                   

                    return View(q);

                }

                else
                {
                TempData["Place"] = "No data found";
                    return RedirectToAction("UserPanel", "Home");
                }
          
        }

        #endregion







        #region Edit A Place

        [AllowAnonymous]
        public ActionResult EditPlace(int PlaceId)
        {
            if ((string)Session["userCode"] == "admin")
            {

                try
                {
                    PlaceDetailViewModel sp = new PlaceDetailViewModel();
                    var p = (from c in entity.Places
                             join e in entity.PlacePictures on c.PlaceProfilePicId
                             equals e.PlacePictureId into ppl
                             from e in ppl.DefaultIfEmpty()

                             join f in entity.Countries on c.CountryId equals f.CountryId
                             join g in entity.Cities on c.CityId equals g.CityId
                             join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                             where c.PlaceId == PlaceId
                             orderby c.PlaceFavourite descending
                             select new
                             {
                                 c.PlaceId,
                                 c.PlaceName,
                                 e.PlacePictureData,

                                 k.PlaceTypeId,
                                 c.PlaceAddress,
                                 c.PlaceFavourite,
                                
                                 f.CountryName,
                                 g.CityName,

                                 c.PlaceDetail,
                                 g.CityId,
                                 f.CountryId,
                                 k.PlaceTypeName,

                                 c.PlaceAdminsPermit,
                             }).First();

                    if (p != null)
                    {

                        sp.PlacePictureData = p.PlacePictureData;
                        sp.PlaceId = p.PlaceId;

                        sp.PlaceName = p.PlaceName;
                        sp.PlaceAddress = p.PlaceAddress;
                        sp.PlaceLike = Convert.ToInt32(p.PlaceFavourite);
                       
                        sp.PlaceTypeName = p.PlaceTypeName;
                        sp.CityName = p.CityName;
                        sp.CountryName = p.CountryName;
                        string st = CutDetail.StripHTML(p.PlaceDetail);
                        sp.PlaceShortDetail = CutDetail.Truncate(st, 40);
                        sp.PlaceFullDetail = p.PlaceDetail;
                        sp.PlaceCityId = p.CityId;
                        sp.PlaceCountryId = p.CountryId;
                        sp.PlaceTypeId = p.PlaceTypeId;
                        sp.PlaceAdminPermit = Convert.ToBoolean(p.PlaceAdminsPermit);


                    }
                    var q = sp;
                    ViewData["pl"] = q;
                        EditPlaceViewModel epvm = new EditPlaceViewModel();
                    epvm.CountryId = q.CountryId;
                    epvm.CityId = q.CityId;
                        epvm.PlaceName = q.PlaceName;
                        epvm.PlaceAddress = q.PlaceAddress;

                        epvm.PlaceTypeList = new SelectList(entity.PlaceTypes, "PlaceTypeId", "PlaceTypeName");
               
                        return View(epvm);

                 
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "EditPlace"));
                }

            }
            else
            {
                Session["message"] = "please log In first.";
                return RedirectToAction("Welcome", "Home");
            }
        }


        [HttpPost]


        public ActionResult EditPlace(EditPlaceViewModel epvm1)
        {
            if (ModelState.IsValid)
            {
                if ((string)Session["userCode"] == "admin")
                {


                    try
                    {
                      
                        if (epvm1.PlaceId != 0)
                        {
                            Place place = (from c in entity.Places where c.PlaceId == epvm1.PlaceId select c).FirstOrDefault();
                            if (epvm1.PlaceTypeId == null)
                            {
                                epvm1.PlaceTypeId = Convert.ToInt32(place.PlaceTypeId);
                            }


                            if (epvm1.PlaceDetail == null)
                            {
                                epvm1.PlaceDetail = place.PlaceDetail;
                            }



                            Place p = (from c in entity.Places where c.PlaceId == epvm1.PlaceId select c).SingleOrDefault();
                            p.PlaceDetail = epvm1.PlaceDetail;
                            p.PlaceTypeId = epvm1.PlaceTypeId;

                            p.PlaceAddress = epvm1.PlaceAddress;
                            p.PlaceName = epvm1.PlaceName;
                            if (entity.SaveChanges() > 0)
                            {
                                ModelState.Clear();

                              TempData["EditPlace"] = "done";
                                return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = epvm1.PlaceId });

                            }
                            else
                            {
                                TempData["EditPlace"] = "Problem occured";
                                return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = epvm1.PlaceId });
                            }

                        }
                        else
                        {
                            TempData["EditPlace"] = "Not chosen any data";
                            return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = epvm1.PlaceId });
                        }

                       
                    }
                    catch (Exception ex)
                    {
                        return View("Error", new HandleErrorInfo(ex, "Admin", "EditPlace"));
                    }

                }
                else
                {
                    Session["message"] = "please log in First";
                    return RedirectToAction("Welcome", "Home");
                }
            }
            else
            {
                PlaceDetailViewModel sp = new PlaceDetailViewModel();
                var p = (from c in entity.Places
                         join e in entity.PlacePictures on c.PlaceProfilePicId
                         equals e.PlacePictureId into ppl
                         from e in ppl.DefaultIfEmpty()

                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.PlaceTypes on c.PlaceTypeId equals k.PlaceTypeId
                         where c.PlaceId == epvm1.PlaceId
                         orderby c.PlaceFavourite descending
                         select new
                         {
                             c.PlaceId,
                             c.PlaceName,
                             e.PlacePictureData,

                             k.PlaceTypeId,
                             c.PlaceAddress,
                             c.PlaceFavourite,
                             
                             f.CountryName,
                             g.CityName,

                             c.PlaceDetail,
                             g.CityId,
                             f.CountryId,
                             k.PlaceTypeName,

                             c.PlaceAdminsPermit,
                         }).First();

                if (p != null)
                {

                    sp.PlacePictureData = p.PlacePictureData;
                    sp.PlaceId = p.PlaceId;

                    sp.PlaceName = p.PlaceName;
                    sp.PlaceAddress = p.PlaceAddress;
                    sp.PlaceLike = Convert.ToInt32(p.PlaceFavourite);
                  
                    sp.PlaceTypeName = p.PlaceTypeName;
                    sp.CityName = p.CityName;
                    sp.CountryName = p.CountryName;
                    string st = CutDetail.StripHTML(p.PlaceDetail);
                    sp.PlaceShortDetail = CutDetail.Truncate(st, 40);
                    sp.PlaceFullDetail = p.PlaceDetail;
                    sp.PlaceCityId = p.CityId;
                    sp.PlaceCountryId = p.CountryId;
                    sp.PlaceTypeId = p.PlaceTypeId;
                    sp.PlaceAdminPermit = Convert.ToBoolean(p.PlaceAdminsPermit);


                }
                var q = sp;
                ViewData["pl"] = q;
                EditPlaceViewModel epvm = new EditPlaceViewModel();
                epvm.CountryId = q.CountryId;
                epvm.CityId = q.CityId;
                epvm.PlaceName = q.PlaceName;
                epvm.PlaceAddress = q.PlaceAddress;

                epvm.PlaceTypeList = new SelectList(entity.PlaceTypes, "PlaceTypeId", "PlaceTypeName");
               
                return View(epvm);
            }
        }





        #endregion




        #region block place

        public ActionResult BlockPlace(int PlaceId)
        {
            if ((string)Session["userCode"] == "admin")
            {

                try
                {
                    
                    if (PlaceId != 0)
                    {

                        bool result = pl.BlockPlace(PlaceId);
                        if (result == true)
                        {
                          TempData["PlaceBlock"] = "Done";
                        }
                        else
                        {
                            TempData["PlaceBlock"] = "Not done yet";
                        }

                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId});
                    }
                    else
                    {
                        TempData["PlaceBlock"] = "Not chosen any data";

                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId});
                    }
                 
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "BlockPlace"));
                }

            }
            else
            {
                Session["message"] = "please log in First";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion


        #region Unblock place

        public ActionResult UnBlockPlace(int PlaceId)
        {
            if ((string)Session["userCode"] == "admin")
            {
              
                try
                {
                    if (PlaceId != 0)
                    {

                        bool result = pl.UnBlockPlace(PlaceId);
                        if (result == true)
                        {
                            TempData["PlaceBlock"] = "Done";
                        }
                        else
                        {
                            TempData["PlaceBlock"] = "Not done yet";
                        }
                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });
                    }
                    else
                    {
                        TempData["PlaceBlock"] = "Not chosen any data";
                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });
                    }
                  
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "BlockPlace"));
                }

            }
            else
            {
                Session["message"] = "please log in First";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion





        #region CheckIn place

        public ActionResult searchCheckIn(int PlaceId)
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                try
                {
                    bool result = pl.CheckCheckIn(PlaceId,TouristId);
                    if(result==false)
                    {
                        TempData["CheckInPlaceSearch"] = "Done";
                        pl.CheckInPlace(PlaceId,TouristId);
                        return RedirectToAction("SearchPlaceDetail","Place",new { PlaceId=PlaceId});
                    }
                    else
                    {
                        TempData["CheckInPlaceSearch"] = "You already have that for today";
                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "searchCheckIn"));
                }

            }
            else
            {
                Session["message"] = "please log in First";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion



        #region CheckIn place

        public ActionResult searchAddToWishPlace(int PlaceId)
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                try
                {
                    bool result = pl.CheckWishIn(PlaceId, TouristId);
                    if (result == false)
                    {
                        TempData["AddToWishPlaceSearch"] = "Done";
                        pl.AddWishPlace(PlaceId, TouristId);
                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });
                    }
                    else
                    {
                        TempData["AddToWishPlaceSearch"] = "You already have this place";
                        return RedirectToAction("SearchPlaceDetail", "Place", new { PlaceId = PlaceId });
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Place", "searchAddToWishPlace"));
                }

            }
            else
            {
                Session["message"] = "please log in First";
                return RedirectToAction("Index", "Home");
            }

        }
        #endregion

    }
}