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
    public class OfferController : Controller
    {
        TourDBEntities entity = new TourDBEntities();
        OfferLogic ol = new OfferLogic();

        #region Add Offer with a new review start



        [AllowAnonymous]
        public ActionResult AddOffer()
        {

            if ((string)Session["userCode"] == "vendor")
            {
                int VendorId = (int)Session["VendorId"];
                var d = (from c in entity.Vendors where c.VendorId == VendorId select c).FirstOrDefault();
                AddOfferViewModel aovm = new AddOfferViewModel();
                aovm.CountryId = Convert.ToInt32(d.CountryId);
                aovm.CityId = Convert.ToInt32(d.CityId);
                aovm.VendorId = Convert.ToInt32(d.VendorId);
                aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
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
        public ActionResult AddOffer(AddOfferViewModel aovm, HttpPostedFileBase AddPicture)
        {

            if (ModelState.IsValid)
            {

                try
                {


                    bool result = ol.CheckVendorIdInOffer(aovm.VendorId);
                    if (result == true)
                    {
                        result = ol.CheckTypeId(aovm.VendorId, aovm.OfferTypeId);
                        if (result == true)
                        {
                            result = ol.CheckOverlap(aovm.OfferStartTime, aovm.OfferStopTime, aovm.VendorId, aovm.OfferTypeId);
                            if (result == true)
                            {
                                TempData["Success"] = "At the same time you can't two offers of same type.";
                                aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
                                return View(aovm);
                            }
                        }
                    }
                    result = ol.CheckNameExistance(aovm.OfferName, aovm.VendorId);
                    if (result == true)
                    {
                        TempData["Success"] = "You have a same Offer in your List please check that";
                        aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
                        return View(aovm);
                    }
                    Offer offer = new Offer();
                    offer.OfferName = aovm.OfferName;
                    offer.VendorId = aovm.VendorId;

                    offer.CountryId = aovm.CountryId;
                    offer.CityId = aovm.CityId;
                    offer.OfferTypeId = aovm.OfferTypeId;
                    offer.OfferPrice = aovm.OfferPrice;
                    offer.OfferStartTime = Convert.ToDateTime(aovm.OfferStartTime);
                    offer.OfferStopTime = Convert.ToDateTime(aovm.OfferStopTime);
                    if (aovm.OfferDetail != null)
                    {
                        offer.OfferDetail = aovm.OfferDetail;
                    }
                    else
                    {
                        offer.OfferDetail = "Not given yet.";
                    }
                    offer.OfferStatus = true;
                    offer.OfferAdminsPermit = true;

                    offer.OfferDateOfAccountCreation = DateTime.Now;
                
                    entity.Offers.Add(offer);
                    Vendor vendor = (from c in entity.Vendors where c.VendorId == aovm.VendorId select c).FirstOrDefault();
                    vendor.VendorOfferStatus = true;


                    if (entity.SaveChanges() > 0)
                    {

                        var p = (from c in entity.Offers where c.OfferName == aovm.OfferName && c.CountryId == aovm.CountryId && c.CityId == aovm.CityId select new { c.OfferId, c.CountryId, c.CityId }).FirstOrDefault();
                        if (AddPicture != null)
                        {

                            OfferAlbum pa = new OfferAlbum();
                            pa.OfferAlbumName = "Profile Picture";
                            pa.OfferId = p.OfferId;
                            pa.AlbumTypeId = 1;
                            pa.CityId = p.CityId;
                            pa.CountryId = p.CountryId;
                            pa.VendorId = aovm.VendorId;
                            pa.OfferAlbumAdminPermit = true;
                            pa.OfferAlbumPrivacy = true;
                            pa.OfferAlbumDateOfAdded = DateTime.Now;
                            entity.OfferAlbums.Add(pa);
                            entity.SaveChanges();
                            OfferPicture pp = new OfferPicture();
                            pp.OfferPictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(pp.OfferPictureData, 0, AddPicture.ContentLength);
                            pp.OfferId = p.OfferId;
                            var albumId = (from c in entity.OfferAlbums where c.OfferId == p.OfferId && c.AlbumTypeId == 1 select c.OfferAlbumId).Single();
                            int AlbumId = Convert.ToInt32(albumId);
                            pp.OfferAlbumId = AlbumId;
                            pp.AlbumTypeId = 1;
                            pp.OfferPictureAdminPermit = true;
                            pp.CountryId = p.CountryId;
                            pp.CityId = p.CityId;
                            pp.VendorId = aovm.VendorId;
                            pp.OfferPicturePrivacy = true;
                            pp.OfferPictureDateOfAdded = DateTime.Now;
                            entity.OfferPictures.Add(pp);
                            if (entity.SaveChanges() > 0)
                            {
                                Offer o = (from c in entity.Offers where c.OfferId == p.OfferId select c).FirstOrDefault();
                                int pictureId = (from c in entity.OfferPictures
                                                 where c.OfferId == p.OfferId && c.AlbumTypeId == 1
                                                 orderby c.OfferPictureId descending
                                                 select c.OfferPictureId).First();
                                o.OfferProfilePicId = pictureId;
                                entity.SaveChanges();


                            }




                        }
                        ModelState.Clear();
                        TempData["Success"] = "Added Successfully";
                        int VendorId = (int)Session["VendorId"];
                        var d = (from c in entity.Vendors where c.VendorId == VendorId select c).FirstOrDefault();
                  
                        aovm.CountryId = Convert.ToInt32(d.CountryId);
                        aovm.CityId = Convert.ToInt32(d.CityId);
                        aovm.VendorId = Convert.ToInt32(d.VendorId);
                        aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
                        return View(aovm);
                    }

                    else
                    {
                        aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
                        TempData["Success"] = "Error not Saved";
                        return View(aovm);
                    }




                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Offer", "AddOffer"));
                }

            }
            else
            {
            
                aovm.OfferTypeList = new SelectList(entity.OfferTypes, "OfferTypeId", "OfferTypeName");
                return View(aovm);
            }
        }
            #endregion



      
        #region searchOfferList

        public ActionResult SearchOfferList(int CountryId, int CityId,int VendorTypeId, string Arrival, string Departure)
        {

            List<OfferDetailViewModel> sp = new List<OfferDetailViewModel>();
            DateTime t = DateTime.Now;
            if ((CountryId != 0 && CityId != 0) && ((Arrival == "" && Departure == "") || (Arrival == null || Departure == null)))
            {
                Session["SearchCountryId"] = CountryId;
                Session["SearchCityId"] = CityId;
                Session["SearchArrival"] = "";
                Session["SearchDeparture"] = "";
                Session["VendorTypeId"] = VendorTypeId;
                var p = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join i in entity.OfferTypes on c.OfferTypeId equals i.OfferTypeId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.VendorTypes on d.VendorTypeId equals k.VendorTypeId
                         where c.CountryId == CountryId && c.CityId == CityId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             d.VendorId,
                             d.VendorOfficeName,
                             c.OfferTypeId,
                             d.VendorTypeId,
                             i.OfferTypeName,
                             f.CountryId,
                             g.CityId,
                             d.VendorPhnNo,
                             d.VendorOfficeAddress,
                         
                             f.CountryName,
                             g.CityName,
                             c.OfferStartTime,
                             c.OfferStopTime,
                             c.OfferDetail,
                             c.OfferPrice,
                             k.VendorTypeName,
                             c.OfferStatus,
                             c.OfferAdminsPermit
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        OfferDetailViewModel oivm = new OfferDetailViewModel();
                        oivm.OfferId = i.OfferId;
                        oivm.OfferTypeName = i.OfferTypeName;
                        oivm.OfferTypeId = Convert.ToInt32(i.OfferTypeId);
                        oivm.VendorTypeId = Convert.ToInt32(i.VendorTypeId);
                        oivm.CountryId = i.CountryId;
                        oivm.CityId = i.CityId;
                        oivm.OfferName = i.OfferName;
                        oivm.OfferPictureData = i.OfferPictureData;
                        oivm.VendorId = i.VendorId;
                        oivm.VendorName = i.VendorOfficeName;
                        oivm.OfferPhnNo = i.VendorPhnNo;
                        oivm.OfferAddress = i.VendorOfficeAddress;
                
                        oivm.CountryName = i.CountryName;
                        oivm.CityName = i.CityName;
                        oivm.OfferPrice = i.OfferPrice;
                        string st = CutDetail.StripHTML(i.OfferDetail);
                        oivm.OfferShortDetail = CutDetail.Truncate(st, 40);
                        oivm.OfferFullDetail = i.OfferDetail;
                        oivm.OfferingStartTime = Convert.ToDateTime(i.OfferStartTime);
                        oivm.OfferingEndTime = Convert.ToDateTime(i.OfferStopTime);
                        oivm.VendorTypeName =i.VendorTypeName;
                        oivm.OfferStatus = Convert.ToBoolean(i.OfferStatus);
                        oivm.OfferAdminPermit = Convert.ToBoolean(i.OfferAdminsPermit);
                        sp.Add(oivm);
                    }
                    var q = sp;
                    return View(q);

                }

                else
                {
                    Arrival = Convert.ToString(Arrival);
                    Departure = Convert.ToString(Departure);
                    TempData["offerAvai"] = "no";
                    return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = "", Departure = "" });
                }
            }
            else  if ((CountryId != 0 && CityId != 0) && ((Arrival != "" && Departure != "") || (Arrival != null || Departure != null)))
            {
                Session["SearchCountryId"] = CountryId;
                Session["SearchCityId"] = CityId;
                Session["SearchArrival"] = Arrival;
                Session["SearchDeparture"] = Departure;
                DateTime a = Convert.ToDateTime(Arrival);
                Session["VendorTypeId"] = VendorTypeId;
                DateTime de = Convert.ToDateTime(Departure);
                var p = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join i in entity.OfferTypes on c.OfferTypeId equals i.OfferTypeId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.VendorTypes on d.VendorTypeId equals k.VendorTypeId
                         where c.CountryId == CountryId && c.CityId == CityId && c.OfferStartTime>=a&& c.OfferStopTime>=de && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             d.VendorId,
                             d.VendorOfficeName,
                             c.OfferTypeId,
                             d.VendorTypeId,
                             i.OfferTypeName,
                             f.CountryId,
                             g.CityId,
                             d.VendorPhnNo,
                             d.VendorOfficeAddress,
                           
                             
                             f.CountryName,
                             g.CityName,
                             c.OfferStartTime,
                             c.OfferStopTime,
                             c.OfferDetail,
                             c.OfferPrice,
                             k.VendorTypeName,
                             c.OfferStatus,
                             c.OfferAdminsPermit
                         }).ToList();
                int x = p.Count();
                if (x > 0)
                {
                    foreach (var i in p)
                    {
                        OfferDetailViewModel oivm = new OfferDetailViewModel();
                        oivm.OfferId = i.OfferId;
                        oivm.OfferTypeName = i.OfferTypeName;
                        oivm.OfferTypeId = Convert.ToInt32(i.OfferTypeId);
                        oivm.VendorTypeId = Convert.ToInt32(i.VendorTypeId);
                        oivm.CountryId = i.CountryId;
                        oivm.CityId = i.CityId;
                        oivm.OfferName = i.OfferName;
                        oivm.OfferPictureData = i.OfferPictureData;
                        oivm.VendorId = i.VendorId;
                        oivm.VendorName = i.VendorOfficeName;
                        oivm.OfferPhnNo = i.VendorPhnNo;
                        oivm.OfferAddress = i.VendorOfficeAddress;
                      
                        oivm.CountryName = i.CountryName;
                        oivm.CityName = i.CityName;
                        oivm.OfferPrice = i.OfferPrice;
                        string st = CutDetail.StripHTML(i.OfferDetail);
                        oivm.OfferShortDetail = CutDetail.Truncate(st, 40);
                        oivm.OfferFullDetail = i.OfferDetail;
                        oivm.OfferingStartTime = Convert.ToDateTime(i.OfferStartTime);
                        oivm.OfferingEndTime = Convert.ToDateTime(i.OfferStopTime);
                        oivm.VendorTypeName = i.VendorTypeName;
                        oivm.OfferStatus = Convert.ToBoolean(i.OfferStatus);
                        oivm.OfferAdminPermit = Convert.ToBoolean(i.OfferAdminsPermit);
                        sp.Add(oivm);
                    }
                    var q = sp;
                    return View(q);

                }

                else
                {
                    Arrival = Convert.ToString(Arrival);
                    Departure = Convert.ToString(Departure);
                    TempData["offerAvai"] = "no";
                    return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = Arrival, Departure = Departure });
                }
            }
            else
            {
                return RedirectToAction("Search", "Home", new { CountryId = CountryId, CityId = CityId, Arrival = Arrival, Departure = Departure });
            }

        }

        #endregion



        


        #region Offer  detail

        public ActionResult SearchOfferOfferDetail(int OfferId)
        {

            OfferDetailViewModel sp = new OfferDetailViewModel();

            if (OfferId != 0)
            {

                         var i = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join j in entity.OfferTypes on c.OfferTypeId equals j.OfferTypeId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.VendorTypes on d.VendorTypeId equals k.VendorTypeId
                         where c.OfferId == OfferId
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             d.VendorId,
                             d.VendorOfficeName,
                             c.OfferTypeId,
                             d.VendorTypeId,
                             j.OfferTypeName,
                             f.CountryId,
                             g.CityId,
                             d.VendorPhnNo,
                             d.VendorOfficeAddress,
                   
                             f.CountryName,
                             g.CityName,
                             c.OfferStartTime,
                             c.OfferStopTime,
                             c.OfferDetail,
                             c.OfferPrice,
                             k.VendorTypeName,
                             c.OfferStatus,
                             c.OfferAdminsPermit
                         }).First();

                if (i != null)
                {

                    sp.OfferId = i.OfferId;
                    sp.OfferTypeName = i.OfferTypeName;
                    sp.OfferTypeId = Convert.ToInt32(i.OfferTypeId);
                    sp.VendorTypeId = Convert.ToInt32(i.VendorTypeId);
                    sp.CountryId = i.CountryId;
                    sp.CityId = i.CityId;
                    sp.OfferName = i.OfferName;
                    sp.OfferPictureData = i.OfferPictureData;
                    sp.VendorId = i.VendorId;
                    sp.VendorName = i.VendorOfficeName;
                    sp.OfferPhnNo = i.VendorPhnNo;
                    sp.OfferAddress = i.VendorOfficeAddress;
                 
                    sp.CountryName = i.CountryName;
                    sp.CityName = i.CityName;
                    sp.OfferPrice = i.OfferPrice;
                    string st = CutDetail.StripHTML(i.OfferDetail);
                    sp.OfferShortDetail = CutDetail.Truncate(st, 40);
                    sp.OfferFullDetail = i.OfferDetail;
                    sp.OfferingStartTime = Convert.ToDateTime(i.OfferStartTime);
                    sp.OfferingEndTime = Convert.ToDateTime(i.OfferStopTime);
                    sp.VendorTypeName = i.VendorTypeName;
                    sp.OfferStatus = Convert.ToBoolean(i.OfferStatus);
                    sp.OfferAdminPermit = Convert.ToBoolean(i.OfferAdminsPermit);


                }
                var q = sp;
            
                return View(q);

            }

            else
            {

                Session["message"] = "No data found"; 
                  
                    return RedirectToAction("Index", "Home");
               
            }
        }


        #endregion

        public ActionResult MyOfferList()
        {
            List<OfferDetailViewModel> sp = new List<OfferDetailViewModel>();
            int vendorId = (int)Session["VendorId"];
            var p = (from c in entity.Offers
                     join e in entity.OfferPictures on c.OfferProfilePicId
                     equals e.OfferPictureId into ppl
                     from e in ppl.DefaultIfEmpty()
                     join d in entity.Vendors on c.VendorId equals d.VendorId
                     join f in entity.Countries on c.CountryId equals f.CountryId
                     join i in entity.OfferTypes on c.OfferTypeId equals i.OfferTypeId
                     join g in entity.Cities on c.CityId equals g.CityId
                     join k in entity.VendorTypes on d.VendorTypeId equals k.VendorTypeId
                     where c.VendorId== vendorId
                     orderby d.VendorFavourite descending
                     select new
                     {
                         c.OfferId,
                         c.OfferName,
                         e.OfferPictureData,
                         d.VendorId,
                         d.VendorOfficeName,
                         c.OfferTypeId,
                         d.VendorTypeId,
                         i.OfferTypeName,
                         f.CountryId,
                         g.CityId,
                         d.VendorPhnNo,
                         d.VendorOfficeAddress,


                         f.CountryName,
                         g.CityName,
                         c.OfferStartTime,
                         c.OfferStopTime,
                         c.OfferDetail,
                         c.OfferPrice,
                         k.VendorTypeName,
                         c.OfferStatus,
                         c.OfferAdminsPermit
                     }).ToList();
            int x = p.Count();
            if (x > 0)
            {
                foreach (var i in p)
                {
                    OfferDetailViewModel oivm = new OfferDetailViewModel();
                    oivm.OfferId = i.OfferId;
                    oivm.OfferTypeName = i.OfferTypeName;
                    oivm.OfferTypeId = Convert.ToInt32(i.OfferTypeId);
                    oivm.VendorTypeId = Convert.ToInt32(i.VendorTypeId);
                    oivm.CountryId = i.CountryId;
                    oivm.CityId = i.CityId;
                    oivm.OfferName = i.OfferName;
                    oivm.OfferPictureData = i.OfferPictureData;
                    oivm.VendorId = i.VendorId;
                    oivm.VendorName = i.VendorOfficeName;
                    oivm.OfferPhnNo = i.VendorPhnNo;
                    oivm.OfferAddress = i.VendorOfficeAddress;

                    oivm.CountryName = i.CountryName;
                    oivm.CityName = i.CityName;
                    oivm.OfferPrice = i.OfferPrice;
                    string st = CutDetail.StripHTML(i.OfferDetail);
                    oivm.OfferShortDetail = CutDetail.Truncate(st, 40);
                    oivm.OfferFullDetail = i.OfferDetail;
                    oivm.OfferingStartTime = Convert.ToDateTime(i.OfferStartTime);
                    oivm.OfferingEndTime = Convert.ToDateTime(i.OfferStopTime);
                    oivm.VendorTypeName = i.VendorTypeName;
                    oivm.OfferStatus = Convert.ToBoolean(i.OfferStatus);
                    oivm.OfferAdminPermit = Convert.ToBoolean(i.OfferAdminsPermit);
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
        public ActionResult DeactivateOffer(int OfferId)
        {
            ol.DeactivateOffer(OfferId);
            return RedirectToAction("MyOfferList", "Offer");
        }


    }
}
