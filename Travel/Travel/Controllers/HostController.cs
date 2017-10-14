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
    public class HostController : Controller
    {

        TourDBEntities entity = new TourDBEntities();
        HostLogic ol = new HostLogic();

        #region Add Offer with a new review start



        [AllowAnonymous]
        public ActionResult BeAHost()
        {

            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                var d = (from c in entity.Tourists where c.TouristId == TouristId select c).FirstOrDefault();
                BeAHostViewModel aovm = new BeAHostViewModel();
                if(d.TouristAddress!=null)
                {
                    aovm.HostAddress = d.TouristAddress;
                }
                aovm.HostPhnNo = d.TouristPhnNo;
                aovm.TouristId = Convert.ToInt32(d.TouristId);
                aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                return View(aovm);

            }
            else
            {
                Session["message"] = "please log in to add your offer.";
                return RedirectToAction("Index", "Home");
            }

        }



        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BeAHost(BeAHostViewModel aovm, HttpPostedFileBase AddPicture)
        {

            if (ModelState.IsValid)
            {

                try
                {


                    bool result = ol.CheckTouristIdInHost(aovm.TouristId);
                    if (result == true)
                    {
                        result = ol.CheckAddressNTypeId(aovm.TouristId,aovm.HostAddress,aovm.HostTypeId);
                        if (result == true)
                        {
                            result = ol.CheckOverlap(aovm.HostStartTime, aovm.HostStopTime, aovm.TouristId, aovm.HostAddress, aovm.HostTypeId);
                            if (result == true)
                            {
                                TempData["Success"]   = "there is a time overlap";
                                aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                                aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                                return View(aovm);
                            }
                        }
                    }
                   
                    Host offer = new Host();
                
                    offer.TouristId = aovm.TouristId;
                    var d = (from c in entity.Tourists where c.TouristId == aovm.TouristId select c).FirstOrDefault();
                    if (aovm.CountryId==null)
                    {
                        aovm.CountryId= Convert.ToInt32(d.CountryId);
                    }
                    if(aovm.CityId == null)
                    {
                        aovm.CityId = Convert.ToInt32(d.CityId);
                    }
                    if(aovm.HostAddress=="Not given yet")
                    {
                        TempData["Success"]= "Address must be given.";
                        aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                        return View(aovm);
                    }
                    offer.HostingCountryId = aovm.CountryId;
                    offer.HostingCityId = aovm.CityId;
                    offer.HostTypeId = aovm.HostTypeId;
                    offer.Rent = aovm.HostPrice;
                    offer.HostingPlaceAddress = aovm.HostAddress;
                    offer.HostingPhnNo = aovm.HostPhnNo;
                    offer.HostingStartTime = Convert.ToDateTime(aovm.HostStartTime);
                    offer.HostingStopTime = Convert.ToDateTime(aovm.HostStopTime);
                    if (aovm.HostDetail != null)
                    {
                        offer.HostingDetail = aovm.HostDetail;
                    }
                    else
                    {
                        offer.HostingDetail = "Not given yet.";
                    }
                    offer.HostingStatus = true;
                    offer.HostAdminsPermit = true;

                    offer.HostDateOfAccountCreation = DateTime.Now;
                  
                    entity.Hosts.Add(offer);
                    Tourist tour = (from c in entity.Tourists where c.TouristId == aovm.TouristId select c).FirstOrDefault();
                    tour.TouristsHostStatus = true;


                    if (entity.SaveChanges() > 0)
                    {

                        var p = (from c in entity.Hosts where c.TouristId == aovm.TouristId && c.HostingCountryId == aovm.CountryId && c.HostingCityId == aovm.CityId && c.HostingPlaceAddress==aovm.HostAddress && c.HostTypeId==aovm.HostTypeId && c.HostingStartTime==aovm.HostStartTime select new { c.HostId, c.HostingCountryId, c.HostingCityId }).FirstOrDefault();
                        if (AddPicture != null)
                        {

                            HostAlbum pa = new HostAlbum();
                            pa.HostAlbumName = "Profile Picture";
                            pa.HostId = p.HostId;
                            pa.AlbumTypeId = 1;
                            pa.CityId = p.HostingCityId;
                            pa.CountryId = p.HostingCountryId;
                            pa.HostAlbumAdminPermit = true;
                            pa.HostAlbumPrivacy = true;
                            pa.TouristId = aovm.TouristId;
                            pa.HostAlbumDateOfAdded = DateTime.Now;
                            entity.HostAlbums.Add(pa);
                            entity.SaveChanges();
                            HostPicture pp = new HostPicture();
                            pp.HostPictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(pp.HostPictureData, 0, AddPicture.ContentLength);
                            pp.HostId = p.HostId;
                            var albumId = (from c in entity.HostAlbums where c.HostId == p.HostId && c.AlbumTypeId == 1 select c.HostAlbumId).Single();
                            int AlbumId = Convert.ToInt32(albumId);
                            pp.HostAlbumId = AlbumId;
                            pp.AlbumTypeId = 1;
                            pp.HostPictureAdminPermit = true;
                            pp.CountryId = p.HostingCountryId;
                            pp.CityId = Convert.ToInt32(p.HostingCityId);
                            pp.TouristId = aovm.TouristId;
                            pp.HostPicturePrivacy = true;
                            pp.HostPictureDateOfAdded = DateTime.Now;
                            entity.HostPictures.Add(pp);
                            if (entity.SaveChanges() > 0)
                            {
                                Host o = (from c in entity.Hosts where c.HostId == p.HostId select c).FirstOrDefault();

                                int pictureId = (from c in entity.HostPictures
                                                 where c.HostId == p.HostId && c.AlbumTypeId == 1
                                                 orderby c.HostPictureId descending
                                                 select c.HostPictureId).First();
                                o.HostProfilePicId = pictureId;
                                entity.SaveChanges();


                            }




                        }
                        ModelState.Clear();
                        TempData["Success"] = "Saved";
                        int TouristId = (int)Session["TouristId"];
                        d = (from c in entity.Tourists where c.TouristId == TouristId select c).FirstOrDefault();
                
                        if (d.TouristAddress != null)
                        {
                            aovm.HostAddress = d.TouristAddress;
                        }
                        aovm.HostPhnNo = d.TouristPhnNo;
                        aovm.TouristId = Convert.ToInt32(d.TouristId);
                        aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                        return View(aovm);
                    }

                    else
                    {
                        TempData["Success"]  = "Error not Saved";
                        aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                        return View(aovm);
                    }




                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Host", "BeAHost"));
                }

            }
            else
            {

                aovm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                aovm.HostTypeList = new SelectList(entity.HostTypes, "HostTypeId", "HostTypeName");
                return View(aovm);
            }
        }
        #endregion




        #region searchHostList

        public ActionResult SearchHostList(int CountryId, int CityId, string Arrival, string Departure)
        {

            List<HostDetailSearchViewModel> sp = new List<HostDetailSearchViewModel>();
            DateTime t = DateTime.Now;
            if ((CountryId != 0 && CityId != 0) && ((Arrival == "" && Departure == "") || (Arrival == null || Departure == null)))
            {
                Session["HostList"] = "yes";
                var p = (from c in entity.Hosts
                         join e in entity.HostPictures on c.HostProfilePicId
                         equals e.HostPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Tourists on c.TouristId equals d.TouristId
                         join f in entity.Countries on c.HostingCountryId equals f.CountryId
                         join g in entity.Cities on c.HostingCityId equals g.CityId
                         join k in entity.HostTypes on c.HostTypeId equals k.HostTypeId
                         where c.HostingCountryId == CountryId && c.HostingCityId == CityId && c.HostingStatus == true && c.HostAdminsPermit == true && c.HostingStopTime >= t
                         orderby c.HostDateOfAccountCreation descending
                         select new
                         {
                             c.HostId,
                             d.TouristName,
                             e.HostPictureData,
                             d.TouristId,
                             f.CountryId,
                             g.CityId,
                             k.HostTypeId,
                             c.HostingPhnNo,
                             c.HostingPlaceAddress,
                           
                             f.CountryName,
                             g.CityName,
                             c.HostingStartTime,
                             c.HostingStopTime,
                             c.HostingDetail,
                             c.Rent,
                             k.HostTypeName,
                             c.HostingStatus,
                             c.HostAdminsPermit
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        HostDetailSearchViewModel oivm = new HostDetailSearchViewModel();
                        oivm.HostPictureData = i.HostPictureData;
                        oivm.HostId = i.HostId;
                        oivm.TouristId = Convert.ToInt32(i.TouristId);
                        oivm.HostName = i.TouristName;
                        oivm.HostAddress = i.HostingPlaceAddress;
                    
                        oivm.HostTypeName = i.HostTypeName;
                        oivm.HostPhnNo = i.HostingPhnNo;
                        oivm.CityId = i.CityId;
                        oivm.CountryId = i.CountryId;
                        oivm.HostTypeId = i.HostTypeId;
                        oivm.HostPrice = i.Rent;
                        oivm.HostingStartTime = Convert.ToDateTime(i.HostingStartTime);
                        oivm.HostingEndTime = Convert.ToDateTime(i.HostingStopTime);
                        string st = CutDetail.StripHTML(i.HostingDetail);
                        oivm.HostShortDetail = CutDetail.Truncate(st, 40);
                        oivm.HostFullDetail = i.HostingDetail;
                        oivm.HostTypeName = i.HostTypeName;
                        oivm.hostAdminPermit = Convert.ToBoolean(i.HostAdminsPermit);
                        oivm.HostStatus = Convert.ToBoolean(i.HostingStatus);
                        sp.Add(oivm);
                    }
                    var q = sp;


                    return View(q);

                }
                else
                {
                    TempData["hostAvai"] = "no";
                    return Redirect(Url.Action("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = Arrival, Departure = Departure }) + "#successMessage");
                }
            }
            else if ((CountryId != 0 && CityId != 0) && ((Arrival != "" && Departure != "") && (Arrival != null || Departure != null)))
            {
                DateTime a = Convert.ToDateTime(Arrival);
                DateTime de = Convert.ToDateTime(Departure);
                Session["HostList"] = "yes";
                var l = (from c in entity.Hosts
                         join e in entity.HostPictures on c.HostProfilePicId
                         equals e.HostPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Tourists on c.TouristId equals d.TouristId
                         join f in entity.Countries on c.HostingCountryId equals f.CountryId
                         join g in entity.Cities on c.HostingCityId equals g.CityId
                         join k in entity.HostTypes on c.HostTypeId equals k.HostTypeId
                         where c.HostingCountryId == CountryId && c.HostingCityId == CityId && c.HostingStartTime >= a && c.HostingStopTime >= de && c.HostingStatus == true && c.HostAdminsPermit == true && c.HostingStopTime >= t
                         orderby c.HostDateOfAccountCreation descending
                         select new
                         {
                             c.HostId,
                             d.TouristName,
                             e.HostPictureData,
                             d.TouristId,
                             f.CountryId,
                             g.CityId,
                             k.HostTypeId,
                             c.HostingPhnNo,
                             c.HostingPlaceAddress,
                        
                            
                             f.CountryName,
                             g.CityName,
                             c.HostingStartTime,
                             c.HostingStopTime,
                             c.HostingDetail,
                             c.Rent,
                             k.HostTypeName,
                             c.HostingStatus,
                             c.HostAdminsPermit
                         }).ToList();
                int y = l.Count();
                if (y > 0)
                {
                    foreach (var i in l)
                    {
                        HostDetailSearchViewModel oivm = new HostDetailSearchViewModel();
                        oivm.HostPictureData = i.HostPictureData;
                        oivm.HostId = i.HostId;
                        oivm.TouristId = Convert.ToInt32(i.TouristId);
                        oivm.HostName = i.TouristName;
                        oivm.HostAddress = i.HostingPlaceAddress;
                 
                        oivm.HostTypeName = i.HostTypeName;
                        oivm.HostPhnNo = i.HostingPhnNo;
                        oivm.CityId = i.CityId;
                        oivm.CountryId = i.CountryId;
                        oivm.HostTypeId = i.HostTypeId;
                        oivm.HostPrice = i.Rent;
                        oivm.HostingStartTime = Convert.ToDateTime(i.HostingStartTime);
                        oivm.HostingEndTime = Convert.ToDateTime(i.HostingStopTime);
                        string st = CutDetail.StripHTML(i.HostingDetail);
                        oivm.HostShortDetail = CutDetail.Truncate(st, 40);
                        oivm.HostFullDetail = i.HostingDetail;
                        oivm.HostTypeName = i.HostTypeName;
                        oivm.hostAdminPermit = Convert.ToBoolean(i.HostAdminsPermit);
                        oivm.HostStatus = Convert.ToBoolean(i.HostingStatus);
                        sp.Add(oivm);
                    }
                    var q = sp;

                    return View(q);

                }
                else
                {
                    TempData["hostAvai"] = "no";
                    return Redirect(Url.Action("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = Arrival, Departure = Departure }) + "#successMessage");
                }


            }
            else
            {
                TempData["hostAvai"] = "no";
                return Redirect(Url.Action("Search", "Home",new { CountryId=CountryId,CityId=CityId,Arrival=Arrival,Departure=Departure}) + "#successMessage");
            }
        }
        
        #endregion

        #region searchHostList

        public ActionResult ShowHost(int HostId,int CountryId, int CityId, int Page)
        {

         HostDetailSearchViewModel sp = new HostDetailSearchViewModel();
            if (HostId != 0)
            {
               
                var i = (from c in entity.Hosts
                         join e in entity.HostPictures on c.HostProfilePicId
                         equals e.HostPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Tourists on c.TouristId equals d.TouristId
                         join f in entity.Countries on c.HostingCountryId equals f.CountryId
                         join g in entity.Cities on c.HostingCityId equals g.CityId
                         join k in entity.HostTypes on c.HostTypeId equals k.HostTypeId
                         where c.HostId==HostId 
                         orderby c.HostDateOfAccountCreation descending
                         select new
                         {
                             c.HostId,
                             d.TouristName,
                             e.HostPictureData,
                             d.TouristId,
                             f.CountryId,
                             g.CityId,
                             k.HostTypeId,
                             c.HostingPhnNo,
                             c.HostingPlaceAddress,
                           
                             f.CountryName,
                             g.CityName,
                             c.HostingStartTime,
                             c.HostingStopTime,
                             c.HostingDetail,
                             c.Rent,
                             k.HostTypeName,
                             c.HostingStatus,
                             c.HostAdminsPermit
                         }).First();
              
                if (i!=null)
                {

                    sp.HostPictureData = i.HostPictureData;
                    sp.HostId = i.HostId;
                    sp.TouristId = Convert.ToInt32(i.TouristId);
                    sp.HostName = i.TouristName;
                    sp.HostAddress = i.HostingPlaceAddress;
                 
                
                    sp.HostTypeName = i.HostTypeName;
                    sp.HostPhnNo = i.HostingPhnNo;
                    sp.CityId = i.CityId;
                    sp.CountryId = i.CountryId;
                    sp.HostTypeId = i.HostTypeId;
                    sp.HostPrice = i.Rent;
                    sp.HostingStartTime = Convert.ToDateTime(i.HostingStartTime);
                    sp.HostingEndTime = Convert.ToDateTime(i.HostingStopTime);
                    string st = CutDetail.StripHTML(i.HostingDetail);
                    sp.HostShortDetail = CutDetail.Truncate(st, 40);
                    sp.HostFullDetail = i.HostingDetail;
                    sp.HostTypeName = i.HostTypeName;
                    sp.hostAdminPermit = Convert.ToBoolean(i.HostAdminsPermit);
                    sp.HostStatus = Convert.ToBoolean(i.HostingStatus);

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

        #endregion

        public ActionResult MyHostList()
        {
            List<HostDetailSearchViewModel> sp = new List<HostDetailSearchViewModel>();
            int touristId = (int)Session["TouristId"];
            var p = (from c in entity.Hosts
                     join e in entity.HostPictures on c.HostProfilePicId
                     equals e.HostPictureId into ppl
                     from e in ppl.DefaultIfEmpty()
                     join d in entity.Tourists on c.TouristId equals d.TouristId
                     join f in entity.Countries on c.HostingCountryId equals f.CountryId
                     join g in entity.Cities on c.HostingCityId equals g.CityId
                     join k in entity.HostTypes on c.HostTypeId equals k.HostTypeId
                     where c.TouristId== touristId
                     orderby c.HostDateOfAccountCreation descending
                     select new
                     {
                         c.HostId,
                         d.TouristName,
                         e.HostPictureData,
                         d.TouristId,
                         f.CountryId,
                         g.CityId,
                         k.HostTypeId,
                         c.HostingPhnNo,
                         c.HostingPlaceAddress,

                         f.CountryName,
                         g.CityName,
                         c.HostingStartTime,
                         c.HostingStopTime,
                         c.HostingDetail,
                         c.Rent,
                         k.HostTypeName,
                         c.HostingStatus,
                         c.HostAdminsPermit
                     }).ToList();
            int x = p.Count();
            if (x > 0)
            {
                foreach (var i in p)
                {
                    HostDetailSearchViewModel oivm = new HostDetailSearchViewModel();
                    oivm.HostPictureData = i.HostPictureData;
                    oivm.HostId = i.HostId;
                    oivm.TouristId = Convert.ToInt32(i.TouristId);
                    oivm.HostName = i.TouristName;
                    oivm.HostAddress = i.HostingPlaceAddress;

                    oivm.HostTypeName = i.HostTypeName;
                    oivm.HostPhnNo = i.HostingPhnNo;
                    oivm.CityId = i.CityId;
                    oivm.CountryId = i.CountryId;
                    oivm.HostTypeId = i.HostTypeId;
                    oivm.HostPrice = i.Rent;
                    oivm.HostingStartTime = Convert.ToDateTime(i.HostingStartTime);
                    oivm.HostingEndTime = Convert.ToDateTime(i.HostingStopTime);
                    string st = CutDetail.StripHTML(i.HostingDetail);
                    oivm.HostShortDetail = CutDetail.Truncate(st, 40);
                    oivm.HostFullDetail = i.HostingDetail;
                    oivm.HostTypeName = i.HostTypeName;
                    oivm.hostAdminPermit = Convert.ToBoolean(i.HostAdminsPermit);
                    oivm.HostStatus = Convert.ToBoolean(i.HostingStatus);
                    sp.Add(oivm);
                }
                var q = sp;


                return View(q);

            }
            else
            {
                TempData["hostAvai"] = "no";
                return RedirectToAction("UserPanel", "Home");
            }
        }
        public ActionResult DeactivateHost(int HostId)
        {
            ol.DeactivateHost(HostId);
            return RedirectToAction("MyHostList","Host");
        }
        }
    }