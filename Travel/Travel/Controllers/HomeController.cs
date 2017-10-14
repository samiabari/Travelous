using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Travel.Models.BusinessLogic;
using Travel.Models.DB;
using Travel.Models.ViewModel;

namespace ProjectTravel.Controllers
{
    public class HomeController : Controller
    {


        TourDBEntities entity = new TourDBEntities();
        VendorLogic vl = new VendorLogic();
        PlaceLogic pl = new PlaceLogic();
        HostLogic hl = new HostLogic();
        #region home
        [AllowAnonymous]
        public ActionResult Index()
        {
            #region deactivating expired product
            OfferLogic ol = new OfferLogic();
            ol.DeactivateAllExpiredOffer();
            HostLogic hl = new HostLogic();
            hl.DeactivateAllExpiredHost();
            #endregion



            IndexViewModel ivm = new IndexViewModel();
            ivm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
            if (TempData["not"] != null)
            {
                var n = TempData["not"];
                TempData["not"] = "There is no data for your selection, Sorry!";
            }
            return View(ivm);
        }
        #endregion


        #region Search



        public ActionResult Search(int CountryId, int CityId, string Arrival, string Departure)
        {
            DateTime a, de;
            if ((Arrival != "" || Departure != "") && (Arrival != null || Departure != null))
            {
                a = Convert.ToDateTime(Arrival);
                de = Convert.ToDateTime(Departure);
            }

            else
            {
                Arrival = "";
                Departure = "";
            }


            DateTime t = DateTime.Now;

            try
            {
                SearchViewModel svm = new SearchViewModel();
                int rest = 0, hot = 0, tran = 0, trav = 0, pla = 0, res = 0, restVen = 0, hotVen = 0, tranVen = 0, travVen = 0;



                #region Search without date
                if ((CountryId != 0 && CityId != 0) && ((Arrival == "" && Departure == "") || (Arrival == null && Departure == null)))
                {
                    var p = (from c in entity.Offers
                             join e in entity.OfferPictures on c.OfferProfilePicId
                             equals e.OfferPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join r in entity.Vendors on c.VendorId equals r.VendorId
                             where c.CityId == CityId && c.CountryId == CountryId && r.VendorTypeId == svm.RestHouseVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                             orderby r.VendorFavourite descending
                             select new
                             {
                                 c.OfferId,
                                 c.OfferName,
                                 e.OfferPictureData,
                                 c.VendorId,
                                 r.VendorOfficeName,
                                 r.VendorTypeId,
                                 c.CountryId,
                                 c.CityId

                             }).FirstOrDefault();

                    OfferIndexViewModel oivm = new OfferIndexViewModel();
                    Session["SearchCountryId"] = CountryId;
                    Session["SearchCityId"] = CityId;
                    Session["SearchArrival"] = "";
                    Session["SearchDeparture"] = "";

                    if (p != null)
                    {


                        oivm.OfferPictureData = p.OfferPictureData;
                        oivm.OfferId = p.OfferId;
                        oivm.VendorId = Convert.ToInt32(p.VendorId);
                        oivm.VendorName = p.VendorOfficeName;
                        oivm.OfferName = p.OfferName;
                        oivm.VendorTypeId = Convert.ToInt32(p.VendorTypeId);

                        svm.RestOffer = oivm;
                        ViewData["RestOffer"] = oivm;
                    }
                    else
                    {
                        TempData["Rest"] = "not";
                        rest = 1;

                    }

                    OfferIndexViewModel oivm3 = new OfferIndexViewModel();
                    var b = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join r in entity.Vendors on c.VendorId equals r.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && r.VendorTypeId == svm.HotelVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby r.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             r.VendorOfficeName,
                             r.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();


                    if (b != null)
                    {

                        oivm3.OfferPictureData = b.OfferPictureData;
                        oivm3.OfferId = b.OfferId;
                        oivm3.VendorId = Convert.ToInt32(b.VendorId);
                        oivm3.VendorName = b.VendorOfficeName;
                        oivm3.OfferName = b.OfferName;
                        oivm3.VendorTypeId = Convert.ToInt32(b.VendorTypeId);

                        svm.HotelOffer = oivm3;
                        ViewData["HotelOffer"] = oivm3;

                    }
                    else
                    {
                        TempData["Hot"] = "not";
                        hot = 1;
                    }
                    OfferIndexViewModel oivm1 = new OfferIndexViewModel();
                    var d = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join r in entity.Vendors on c.VendorId equals r.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && r.VendorTypeId == svm.TransportVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby r.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             r.VendorOfficeName,
                             r.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();

                    if (d != null)
                    {
                        oivm1.OfferPictureData = d.OfferPictureData;
                        oivm1.OfferId = d.OfferId;
                        oivm1.VendorId = Convert.ToInt32(d.VendorId);
                        oivm1.VendorName = d.VendorOfficeName;
                        oivm1.OfferName = d.OfferName;
                        oivm1.VendorTypeId = Convert.ToInt32(d.VendorTypeId);

                        svm.HotelOffer = oivm1;
                        ViewData["TransOffer"] = oivm1;
                    }
                    else
                    {
                        TempData["Trans"] = "not";
                        tran = 1;
                    }
                    var y = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join r in entity.Vendors on c.VendorId equals r.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && r.VendorTypeId == svm.TravelVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby r.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             r.VendorOfficeName,
                             r.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();
                    OfferIndexViewModel oivm2 = new OfferIndexViewModel();
                    if (y != null)
                    {
                        oivm2.OfferPictureData = y.OfferPictureData;
                        oivm2.OfferId = y.OfferId;
                        oivm2.VendorId = Convert.ToInt32(y.VendorId);
                        oivm2.VendorName = y.VendorOfficeName;
                        oivm2.OfferName = y.OfferName;
                        oivm2.VendorTypeId = Convert.ToInt32(y.VendorTypeId);

                        svm.HotelOffer = oivm2;
                        ViewData["TraAgeOffer"] = oivm2;
                    }
                    else
                    {
                        TempData["Trav"] = "not";
                        trav = 1;
                    }


                    if ((rest == 1) && (hot == 1) && (tran == 1) && (trav == 1))
                    {
                        TempData["AllOff"] = "not";
                    }


                    var q = (from c in entity.Places
                             join e in entity.PlacePictures on c.PlaceProfilePicId
                             equals e.PlacePictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join r in entity.PlaceTypes on c.PlaceTypeId equals r.PlaceTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.PlaceAdminsPermit == true
                             orderby c.PlaceFavourite descending
                             select new
                             {
                                 c.PlaceId,
                                 c.PlaceName,
                                 e.PlacePictureData,
                             }).Take(3);
                    var l = from c in q
                            select new PlaceIndexViewModel()
                            {
                                PlaceId = c.PlaceId,
                                PlacePictureData = c.PlacePictureData,
                                PlaceName = c.PlaceName
                            };

                    int x = l.Count();
                    if (x > 0)
                    {
                        svm.Places = l.ToList();
                    }
                    else
                    {
                        pla = 1;
                        TempData["Pla"] = "not";
                    }

                    VendorDetailViewModel vdvm = new VendorDetailViewModel();

                    var o = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join r in entity.Countries on c.CountryId equals r.CountryId
                             join f in entity.Cities on c.CityId equals f.CityId
                             join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.RestHouseVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                                 c.VendorOfficeAddress,
                                 c.VendorOfficeWebsite,
                                 c.VendorOfficePhnNo,
                                 h.VendorTypeId,
                                 h.VendorTypeName,
                                 c.CountryId,
                                 c.CityId,
                                 c.VendorFavourite,
                                 c.VendorsVendorShipDetail,
                                 f.CityName,
                                 r.CountryName,
                               
                                 c.VendorOfferStatus,
                                 c.VendorsStatus,
                                 c.VendorAdminsPermit

                             }).FirstOrDefault();

                    if (o != null)
                    {
                        vdvm.VendorId = o.VendorId;
                        vdvm.VendorName = o.VendorOfficeName;
                        vdvm.VendorAddress = o.VendorOfficeAddress;
                        vdvm.VendorPhnNo = o.VendorOfficePhnNo;
                        vdvm.VendorWebsite = o.VendorOfficeWebsite;
                        vdvm.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                        vdvm.VendorTypeName = o.VendorTypeName;
                        vdvm.CityId = Convert.ToInt32(o.CityId);
                        vdvm.CityName = o.CityName;
                        vdvm.CountryId = Convert.ToInt32(o.CountryId);
                        vdvm.CountryName = o.CountryName;
                        vdvm.VendorPictureData = o.VendorPictureData;
                        vdvm.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                        vdvm.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                        vdvm.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm.VendorFullDetail = o.VendorsVendorShipDetail;
                        vdvm.VendorLike = Convert.ToInt32(o.VendorFavourite);
                     
                        vdvm.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);
                        svm.RestVendor = vdvm;
                        ViewData["RestVendor"] = svm.RestVendor;
                    }
                    else
                    {
                        TempData["RestVen"] = "not";
                        restVen = 1;
                    }

                    VendorDetailViewModel vdvm1 = new VendorDetailViewModel();

                    var m = (from c in entity.Vendors
                         join e in entity.VendorPictures on c.VendorProfilePicId
                         equals e.VendorPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join r in entity.Countries on c.CountryId equals d.CountryId
                         join f in entity.Cities on c.CityId equals f.CityId
                         join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                         where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.HotelVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                         orderby c.VendorFavourite descending
                         select new
                         {
                             c.VendorId,
                             c.VendorOfficeName,
                             e.VendorPictureData,
                             c.VendorOfficeAddress,
                             c.VendorOfficeWebsite,
                             c.VendorOfficePhnNo,
                             h.VendorTypeId,
                             h.VendorTypeName,
                             c.CountryId,
                             c.CityId,
                             c.VendorFavourite,
                             c.VendorsVendorShipDetail,
                             f.CityName,
                             r.CountryName,
                             c.VendorOfferStatus,
                             c.VendorsStatus,
                             c.VendorAdminsPermit

                         }).FirstOrDefault();

                    if (m != null)
                    {
                        vdvm1.VendorId = m.VendorId;
                        vdvm1.VendorName = m.VendorOfficeName;
                        vdvm1.VendorAddress = m.VendorOfficeAddress;
                        vdvm1.VendorPhnNo = m.VendorOfficePhnNo;
                        vdvm1.VendorWebsite = m.VendorOfficeWebsite;
                        vdvm1.VendorTypeId = Convert.ToInt32(m.VendorTypeId);
                        vdvm1.VendorTypeName = m.VendorTypeName;
                        vdvm1.CityId = Convert.ToInt32(m.CityId);
                        vdvm1.CityName = m.CityName;
                        vdvm1.CountryId = Convert.ToInt32(m.CountryId);
                        vdvm1.CountryName = m.CountryName;
                        vdvm1.VendorPictureData = m.VendorPictureData;
                        vdvm1.VendorStatus = Convert.ToBoolean(m.VendorsStatus);
                        vdvm1.VendorOfferStatus = Convert.ToBoolean(m.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(m.VendorsVendorShipDetail);
                        vdvm1.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm1.VendorFullDetail = m.VendorsVendorShipDetail;
                        vdvm1.VendorLike = Convert.ToInt32(m.VendorFavourite);
                     
                        vdvm1.VendorAdminPermit = Convert.ToBoolean(m.VendorAdminsPermit);
                        svm.HotelVendor = vdvm1;
                        ViewData["HotelVendor"] = vdvm1;
                    }
                    else
                    {
                        TempData["HotVen"] = "not";
                        hotVen = 1;
                    }
                    VendorDetailViewModel vdvm2 = new VendorDetailViewModel();
                    var n = (from c in entity.Vendors
                         join e in entity.VendorPictures on c.VendorProfilePicId
                         equals e.VendorPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join rd in entity.Countries on c.CountryId equals rd.CountryId
                         join f in entity.Cities on c.CityId equals f.CityId
                         join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                         where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.TransportVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                         orderby c.VendorFavourite descending
                         select new
                         {
                             c.VendorId,
                             c.VendorOfficeName,
                             e.VendorPictureData,
                             c.VendorOfficeAddress,
                             c.VendorOfficeWebsite,
                             c.VendorOfficePhnNo,
                             h.VendorTypeId,
                             h.VendorTypeName,
                             c.CountryId,
                             c.CityId,
                             c.VendorFavourite,
                             c.VendorsVendorShipDetail,
                             f.CityName,
                             rd.CountryName,
                         
                             c.VendorOfferStatus,
                             c.VendorsStatus,
                             c.VendorAdminsPermit

                         }).FirstOrDefault();

                    if (n != null)
                    {
                        vdvm2.VendorId = n.VendorId;
                        vdvm2.VendorName = n.VendorOfficeName;
                        vdvm2.VendorAddress = n.VendorOfficeAddress;
                        vdvm2.VendorPhnNo = n.VendorOfficePhnNo;
                        vdvm2.VendorWebsite = n.VendorOfficeWebsite;
                        vdvm2.VendorTypeId = Convert.ToInt32(n.VendorTypeId);
                        vdvm2.VendorTypeName = n.VendorTypeName;
                        vdvm2.CityId = Convert.ToInt32(n.CityId);
                        vdvm2.CityName = n.CityName;
                        vdvm2.CountryId = Convert.ToInt32(n.CountryId);
                        vdvm2.CountryName = n.CountryName;
                        vdvm2.VendorPictureData = n.VendorPictureData;
                        vdvm2.VendorStatus = Convert.ToBoolean(n.VendorsStatus);
                        vdvm2.VendorOfferStatus = Convert.ToBoolean(n.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(n.VendorsVendorShipDetail);
                        vdvm2.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm2.VendorFullDetail = n.VendorsVendorShipDetail;
                        vdvm2.VendorLike = Convert.ToInt32(n.VendorFavourite);
                       
                        vdvm2.VendorAdminPermit = Convert.ToBoolean(n.VendorAdminsPermit);
                        svm.TransVendor = vdvm2;
                        ViewData["TransVendor"] = vdvm2;
                    }
                    else
                    {
                        TempData["tranVen"] = "not";
                        tranVen = 1;
                    }
                    VendorDetailViewModel vdvm3 = new VendorDetailViewModel();

                var be = (from c in entity.Vendors
                         join e in entity.VendorPictures on c.VendorProfilePicId
                         equals e.VendorPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join r in entity.Countries on c.CountryId equals r.CountryId
                         join f in entity.Cities on c.CityId equals f.CityId
                         join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                         where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.TravelVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                         orderby c.VendorFavourite descending
                         select new
                         {
                             c.VendorId,
                             c.VendorOfficeName,
                             e.VendorPictureData,
                             c.VendorOfficeAddress,
                             c.VendorOfficeWebsite,
                             c.VendorOfficePhnNo,
                             h.VendorTypeId,
                             h.VendorTypeName,
                             c.CountryId,
                             c.CityId,
                             c.VendorFavourite,
                             c.VendorsVendorShipDetail,
                             f.CityName,
                             r.CountryName,
                           
                             c.VendorOfferStatus,
                             c.VendorsStatus,
                             c.VendorAdminsPermit

                         }).FirstOrDefault();

                    if (be != null)
                    {
                        vdvm3.VendorId = be.VendorId;
                        vdvm3.VendorName = be.VendorOfficeName;
                        vdvm3.VendorAddress = be.VendorOfficeAddress;
                        vdvm3.VendorPhnNo = be.VendorOfficePhnNo;
                        vdvm3.VendorWebsite = be.VendorOfficeWebsite;
                        vdvm3.VendorTypeId = Convert.ToInt32(be.VendorTypeId);
                        vdvm3.VendorTypeName = be.VendorTypeName;
                        vdvm3.CityId = Convert.ToInt32(be.CityId);
                        vdvm3.CityName = be.CityName;
                        vdvm3.CountryId = Convert.ToInt32(be.CountryId);
                        vdvm3.CountryName = be.CountryName;
                        vdvm3.VendorPictureData = be.VendorPictureData;
                        vdvm3.VendorStatus = Convert.ToBoolean(be.VendorsStatus);
                        vdvm3.VendorOfferStatus = Convert.ToBoolean(be.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(be.VendorsVendorShipDetail);
                        vdvm3.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm3.VendorFullDetail = be.VendorsVendorShipDetail;
                        vdvm3.VendorLike = Convert.ToInt32(be.VendorFavourite);
                        
                        vdvm3.VendorAdminPermit = Convert.ToBoolean(be.VendorAdminsPermit);
                        svm.TravVendor = vdvm3;
                        ViewData["TravVendor"] = vdvm3;
                    }
                    else
                    {
                        TempData["travVen"] = "not";
                        travVen = 1;
                    }














                    var g = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.RestaurentVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                             }).Take(3);

                    var k = from i in g
                            select new RestaurentVendorViewModel()
                            {
                                VendorId = i.VendorId,
                                VendorName = i.VendorOfficeName,
                                VendorPictureData = i.VendorPictureData
                            };
                    x = k.Count();
                    if (x > 0)
                    {

                        svm.Rest = k.ToList();
                    }
                    else
                    {
                        res = 1;
                        TempData["Food"] = "not";
                    }
                    bool png = vl.CheckPhoto(CountryId, CityId);
                    bool hos = pl.CheckHos(CountryId, CityId);
                    bool laun = vl.CheckLaun(CountryId, CityId);
                    bool food = vl.FoodDe(CountryId, CityId);
                    bool Guid = vl.Guid(CountryId, CityId);
                    bool host = hl.HostCheck(CountryId, CityId);
                    bool Market = pl.Market(CountryId, CityId);
                    if (png == true)
                    {
                        TempData["png"] = "yes";
                    }
                    if (hos == true)
                    {
                        TempData["hos"] = "yes";
                    }
                    if (laun == true)
                    {
                        TempData["laun"] = "yes";
                    }
                    if (food == true)
                    {
                        TempData["foodDeli"] = "yes";
                    }
                    if (Guid == true)
                    {
                        TempData["Guid"] = "yes";
                    }
                    if (Market == true)
                    {
                        TempData["Market"] = "yes";
                    }

                    if (png == false && hos == false && res == 1 && laun == false && food == false && Guid == false && Market == false && pla == 1 && rest == 1 && hot == 1 && tran == 1 && trav == 1 && host == false && travVen == 1 && tranVen == 1 && restVen == 1 && hotVen == 1)
                    {
                        TempData["not"] = "no";
                        return Redirect(Url.Action("Index", "Home") + "#successMessage");
                    }
                    else
                    {
                        svm.CountryId = CountryId;
                        svm.CityId = CityId;
                        svm.Arrival = Arrival;
                        svm.Departure = Departure;
                        svm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        if (TempData["hostAvai"] != null)
                        {
                            TempData["hostAvai"] = "There is no data for your selection.";
                        }
                        if (TempData["offerAvai"] != null)
                        {
                            TempData["offerAvai"] = "There is no data for your selection.";
                        }
                        return View(svm);
                    }
                }
                #endregion















                #region search with date
                else if ((CountryId != 0 && CityId != 0) && (Arrival != "" || Departure != "") && (Arrival != null || Departure != null))
                {
                    a = Convert.ToDateTime(Arrival);
                    de = Convert.ToDateTime(Departure);
                    #region offer for rest house
                    var p = (from c in entity.Offers
                             join e in entity.OfferPictures on c.OfferProfilePicId
                             equals e.OfferPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.Vendors on c.VendorId equals d.VendorId
                             where c.CityId == CityId && c.CountryId == CountryId && c.OfferStartTime.Value >= a.Date && c.OfferStopTime >= de && d.VendorTypeId == svm.RestHouseVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                             orderby d.VendorFavourite descending
                             select new
                             {
                                 c.OfferId,
                                 c.OfferName,
                                 e.OfferPictureData,
                                 c.VendorId,
                                 d.VendorOfficeName,
                                 d.VendorTypeId,
                                 c.CountryId,
                                 c.CityId

                             }).FirstOrDefault();

                    OfferIndexViewModel oivm = new OfferIndexViewModel();

                    if (p != null)
                    {


                        oivm.OfferPictureData = p.OfferPictureData;
                        oivm.OfferId = p.OfferId;
                        oivm.VendorId = Convert.ToInt32(p.VendorId);
                        oivm.VendorName = p.VendorOfficeName;
                        oivm.OfferName = p.OfferName;
                        oivm.VendorTypeId = Convert.ToInt32(p.VendorTypeId);

                        svm.RestOffer = oivm;
                        ViewData["RestOffer"] = svm.RestOffer;
                    }
                    else
                    {
                        TempData["Rest"] = "not";
                        rest = 1;

                    }
                    #endregion
                    Session["SearchCountryId"] = CountryId;
                    Session["SearchCityId"] = CityId;
                    Session["SearchArrival"] = Arrival;
                    Session["SearchDeparture"] = Departure;
                    #region offer hotel
                    p = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && c.OfferStartTime.Value >= a.Date && c.OfferStopTime >= de && d.VendorTypeId == svm.HotelVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             d.VendorOfficeName,
                             d.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();


                    if (p != null)
                    {

                        oivm.OfferPictureData = p.OfferPictureData;
                        oivm.OfferId = p.OfferId;
                        oivm.VendorId = Convert.ToInt32(p.VendorId);
                        oivm.VendorName = p.VendorOfficeName;
                        oivm.OfferName = p.OfferName;
                        oivm.VendorTypeId = Convert.ToInt32(p.VendorTypeId);

                        svm.HotelOffer = oivm;
                        ViewData["HotelOffer"] = svm.HotelOffer;

                    }
                    else
                    {
                        TempData["Hot"] = "not";
                        hot = 1;
                    }
                    #endregion

                    #region offer transport

                    p = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && c.OfferStartTime.Value >= a.Date && c.OfferStopTime >= de && d.VendorTypeId == svm.TransportVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             d.VendorOfficeName,
                             d.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();

                    if (p != null)
                    {
                        oivm.OfferPictureData = p.OfferPictureData;
                        oivm.OfferId = p.OfferId;
                        oivm.VendorId = Convert.ToInt32(p.VendorId);
                        oivm.VendorName = p.VendorOfficeName;
                        oivm.OfferName = p.OfferName;
                        oivm.VendorTypeId = Convert.ToInt32(p.VendorTypeId);

                        svm.HotelOffer = oivm;
                        ViewData["TransOffer"] = svm.TransOffer;
                    }
                    else
                    {
                        TempData["Trans"] = "not";
                        tran = 1;
                    }
                    #endregion

                    #region offer travel agent
                    p = (from c in entity.Offers
                         join e in entity.OfferPictures on c.OfferProfilePicId
                         equals e.OfferPictureId into ppl
                         from e in ppl.DefaultIfEmpty()
                         join d in entity.Vendors on c.VendorId equals d.VendorId
                         where c.CityId == CityId && c.CountryId == CountryId && c.OfferStartTime.Value >= a.Date && c.OfferStopTime >= de && d.VendorTypeId == svm.TravelVendorId && c.OfferStatus == true && c.OfferAdminsPermit == true && c.OfferStopTime >= t
                         orderby d.VendorFavourite descending
                         select new
                         {
                             c.OfferId,
                             c.OfferName,
                             e.OfferPictureData,
                             c.VendorId,
                             d.VendorOfficeName,
                             d.VendorTypeId,
                             c.CountryId,
                             c.CityId

                         }).FirstOrDefault();

                    if (p != null)
                    {
                        oivm.OfferPictureData = p.OfferPictureData;
                        oivm.OfferId = p.OfferId;
                        oivm.VendorId = Convert.ToInt32(p.VendorId);
                        oivm.VendorName = p.VendorOfficeName;
                        oivm.OfferName = p.OfferName;
                        oivm.VendorTypeId = Convert.ToInt32(p.VendorTypeId);

                        svm.HotelOffer = oivm;
                        ViewData["TraAgeOffer"] = svm.TraAgeOffer;
                    }
                    else
                    {
                        TempData["Trav"] = "not";
                        trav = 1;
                    }

                    #endregion
                    if ((rest == 1) && (hot == 1) && (tran == 1) && (trav == 1))
                    {
                        TempData["AllOff"] = "not";
                    }

                    #region place

                    var q = (from c in entity.Places
                             join e in entity.PlacePictures on c.PlaceProfilePicId
                             equals e.PlacePictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.PlaceTypes on c.PlaceTypeId equals d.PlaceTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.PlaceAdminsPermit == true
                             orderby c.PlaceFavourite descending
                             select new
                             {
                                 c.PlaceId,
                                 c.PlaceName,
                                 e.PlacePictureData,
                             }).Take(3);
                    var l = from c in q
                            select new PlaceIndexViewModel()
                            {
                                PlaceId = c.PlaceId,
                                PlacePictureData = c.PlacePictureData,
                                PlaceName = c.PlaceName
                            };

                    int x = l.Count();
                    if (x > 0)
                    {
                        svm.Places = l.ToList();
                    }
                    else
                    {
                        pla = 1;
                        TempData["Pla"] = "not";
                    }
                    #endregion



                    VendorDetailViewModel vdvm = new VendorDetailViewModel();

                    var o = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.Countries on c.CountryId equals d.CountryId
                             join f in entity.Cities on c.CityId equals f.CityId
                             join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.RestHouseVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                                 c.VendorOfficeAddress,
                                 c.VendorOfficeWebsite,
                                 c.VendorOfficePhnNo,
                                 h.VendorTypeId,
                                 h.VendorTypeName,
                                 c.CountryId,
                                 c.CityId,
                                 c.VendorFavourite,
                                 c.VendorsVendorShipDetail,
                                 f.CityName,
                                 d.CountryName,

                                 c.VendorOfferStatus,
                                 c.VendorsStatus,
                                 c.VendorAdminsPermit

                             }).FirstOrDefault();

                    if (o != null)
                    {
                        vdvm.VendorId = o.VendorId;
                        vdvm.VendorName = o.VendorOfficeName;
                        vdvm.VendorAddress = o.VendorOfficeAddress;
                        vdvm.VendorPhnNo = o.VendorOfficePhnNo;
                        vdvm.VendorWebsite = o.VendorOfficeWebsite;
                        vdvm.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                        vdvm.VendorTypeName = o.VendorTypeName;
                        vdvm.CityId = Convert.ToInt32(o.CityId);
                        vdvm.CityName = o.CityName;
                        vdvm.CountryId = Convert.ToInt32(o.CountryId);
                        vdvm.CountryName = o.CountryName;
                        vdvm.VendorPictureData = o.VendorPictureData;
                        vdvm.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                        vdvm.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                        vdvm.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm.VendorFullDetail = o.VendorsVendorShipDetail;
                        vdvm.VendorLike = Convert.ToInt32(o.VendorFavourite);

                        vdvm.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);
                        svm.RestVendor = vdvm;
                        ViewData["RestVendor"] = svm.RestVendor;
                    }
                    else
                    {
                        TempData["RestVen"] = "not";
                        restVen = 1;
                    }

                    VendorDetailViewModel vdvm1 = new VendorDetailViewModel();

                    var m = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.Countries on c.CountryId equals d.CountryId
                             join f in entity.Cities on c.CityId equals f.CityId
                             join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.HotelVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                                 c.VendorOfficeAddress,
                                 c.VendorOfficeWebsite,
                                 c.VendorOfficePhnNo,
                                 h.VendorTypeId,
                                 h.VendorTypeName,
                                 c.CountryId,
                                 c.CityId,
                                 c.VendorFavourite,
                                 c.VendorsVendorShipDetail,
                                 f.CityName,
                                 d.CountryName,
                                 c.VendorOfferStatus,
                                 c.VendorsStatus,
                                 c.VendorAdminsPermit

                             }).FirstOrDefault();

                    if (m != null)
                    {
                        vdvm1.VendorId = m.VendorId;
                        vdvm1.VendorName = m.VendorOfficeName;
                        vdvm1.VendorAddress = m.VendorOfficeAddress;
                        vdvm1.VendorPhnNo = m.VendorOfficePhnNo;
                        vdvm1.VendorWebsite = m.VendorOfficeWebsite;
                        vdvm1.VendorTypeId = Convert.ToInt32(m.VendorTypeId);
                        vdvm1.VendorTypeName = m.VendorTypeName;
                        vdvm1.CityId = Convert.ToInt32(m.CityId);
                        vdvm1.CityName = m.CityName;
                        vdvm1.CountryId = Convert.ToInt32(m.CountryId);
                        vdvm1.CountryName = m.CountryName;
                        vdvm1.VendorPictureData = m.VendorPictureData;
                        vdvm1.VendorStatus = Convert.ToBoolean(m.VendorsStatus);
                        vdvm1.VendorOfferStatus = Convert.ToBoolean(m.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(m.VendorsVendorShipDetail);
                        vdvm1.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm1.VendorFullDetail = m.VendorsVendorShipDetail;
                        vdvm1.VendorLike = Convert.ToInt32(m.VendorFavourite);

                        vdvm1.VendorAdminPermit = Convert.ToBoolean(m.VendorAdminsPermit);
                        svm.HotelVendor = vdvm1;
                        ViewData["HotelVendor"] = vdvm1;
                    }
                    else
                    {
                        TempData["HotVen"] = "not";
                        hotVen = 1;
                    }
                    VendorDetailViewModel vdvm2 = new VendorDetailViewModel();
                    var n = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.Countries on c.CountryId equals d.CountryId
                             join f in entity.Cities on c.CityId equals f.CityId
                             join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.TransportVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                                 c.VendorOfficeAddress,
                                 c.VendorOfficeWebsite,
                                 c.VendorOfficePhnNo,
                                 h.VendorTypeId,
                                 h.VendorTypeName,
                                 c.CountryId,
                                 c.CityId,
                                 c.VendorFavourite,
                                 c.VendorsVendorShipDetail,
                                 f.CityName,
                                 d.CountryName,

                                 c.VendorOfferStatus,
                                 c.VendorsStatus,
                                 c.VendorAdminsPermit

                             }).FirstOrDefault();

                    if (n != null)
                    {
                        vdvm2.VendorId = n.VendorId;
                        vdvm2.VendorName = n.VendorOfficeName;
                        vdvm2.VendorAddress = n.VendorOfficeAddress;
                        vdvm2.VendorPhnNo = n.VendorOfficePhnNo;
                        vdvm2.VendorWebsite = n.VendorOfficeWebsite;
                        vdvm2.VendorTypeId = Convert.ToInt32(n.VendorTypeId);
                        vdvm2.VendorTypeName = n.VendorTypeName;
                        vdvm2.CityId = Convert.ToInt32(n.CityId);
                        vdvm2.CityName = n.CityName;
                        vdvm2.CountryId = Convert.ToInt32(n.CountryId);
                        vdvm2.CountryName = n.CountryName;
                        vdvm2.VendorPictureData = n.VendorPictureData;
                        vdvm2.VendorStatus = Convert.ToBoolean(n.VendorsStatus);
                        vdvm2.VendorOfferStatus = Convert.ToBoolean(n.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(n.VendorsVendorShipDetail);
                        vdvm2.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm2.VendorFullDetail = n.VendorsVendorShipDetail;
                        vdvm2.VendorLike = Convert.ToInt32(n.VendorFavourite);

                        vdvm2.VendorAdminPermit = Convert.ToBoolean(n.VendorAdminsPermit);
                        svm.TransVendor = vdvm2;
                        ViewData["TransVendor"] = vdvm2;
                    }
                    else
                    {
                        TempData["tranVen"] = "not";
                        tranVen = 1;
                    }
                    VendorDetailViewModel vdvm3 = new VendorDetailViewModel();

                    var b = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             join d in entity.Countries on c.CountryId equals d.CountryId
                             join f in entity.Cities on c.CityId equals f.CityId
                             join h in entity.VendorTypes on c.VendorTypeId equals h.VendorTypeId
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.TravelVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                                 c.VendorOfficeAddress,
                                 c.VendorOfficeWebsite,
                                 c.VendorOfficePhnNo,
                                 h.VendorTypeId,
                                 h.VendorTypeName,
                                 c.CountryId,
                                 c.CityId,
                                 c.VendorFavourite,
                                 c.VendorsVendorShipDetail,
                                 f.CityName,
                                 d.CountryName,

                                 c.VendorOfferStatus,
                                 c.VendorsStatus,
                                 c.VendorAdminsPermit

                             }).FirstOrDefault();

                    if (b != null)
                    {
                        vdvm3.VendorId = b.VendorId;
                        vdvm3.VendorName = b.VendorOfficeName;
                        vdvm3.VendorAddress = b.VendorOfficeAddress;
                        vdvm3.VendorPhnNo = b.VendorOfficePhnNo;
                        vdvm3.VendorWebsite = b.VendorOfficeWebsite;
                        vdvm3.VendorTypeId = Convert.ToInt32(b.VendorTypeId);
                        vdvm3.VendorTypeName = b.VendorTypeName;
                        vdvm3.CityId = Convert.ToInt32(b.CityId);
                        vdvm3.CityName = b.CityName;
                        vdvm3.CountryId = Convert.ToInt32(b.CountryId);
                        vdvm3.CountryName = b.CountryName;
                        vdvm3.VendorPictureData = b.VendorPictureData;
                        vdvm3.VendorStatus = Convert.ToBoolean(b.VendorsStatus);
                        vdvm3.VendorOfferStatus = Convert.ToBoolean(b.VendorOfferStatus);
                        string VendorShortDetail = CutDetail.StripHTML(b.VendorsVendorShipDetail);
                        vdvm3.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                        vdvm3.VendorFullDetail = b.VendorsVendorShipDetail;
                        vdvm3.VendorLike = Convert.ToInt32(b.VendorFavourite);

                        vdvm3.VendorAdminPermit = Convert.ToBoolean(b.VendorAdminsPermit);
                        svm.TravVendor = vdvm3;
                        ViewData["TravVendor"] = vdvm3;
                    }
                    else
                    {
                        TempData["travVen"] = "not";
                        travVen = 1;
                    }












                    #region restaurent
                    var g = (from c in entity.Vendors
                             join e in entity.VendorPictures on c.VendorProfilePicId
                             equals e.VendorPictureId into ppl
                             from e in ppl.DefaultIfEmpty()
                             where c.CityId == CityId && c.CountryId == CountryId && c.VendorTypeId == svm.RestaurentVendorId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                             orderby c.VendorFavourite descending
                             select new
                             {
                                 c.VendorId,
                                 c.VendorOfficeName,
                                 e.VendorPictureData,
                             }).Take(3);

                    var k = from i in g
                            select new RestaurentVendorViewModel()
                            {
                                VendorId = i.VendorId,
                                VendorName = i.VendorOfficeName,
                                VendorPictureData = i.VendorPictureData
                            };
                    x = k.Count();
                    if (x > 0)
                    {

                        svm.Rest = k.ToList();
                    }
                    else
                    {
                        res = 1;
                        TempData["Food"] = "not";
                    }
                    #endregion
                    bool png = vl.CheckPhoto(CountryId, CityId);
                    bool hos = pl.CheckHos(CountryId, CityId);
                    bool laun = vl.CheckLaun(CountryId, CityId);
                    bool food = vl.FoodDe(CountryId, CityId);
                    bool Guid = vl.Guid(CountryId, CityId);
                    bool Market = pl.Market(CountryId, CityId);
                    bool host = hl.HostCheckwDate(CountryId, CityId, a, de);
                    if (png == true)
                    {
                        TempData["png"] = "yes";
                    }
                    if (hos == true)
                    {
                        TempData["hos"] = "yes";
                    }
                    if (laun == true)
                    {
                        TempData["laun"] = "yes";
                    }
                    if (food == true)
                    {
                        TempData["foodDeli"] = "yes";
                    }
                    if (Guid == true)
                    {
                        TempData["Guid"] = "yes";
                    }
                    if (Market == true)
                    {
                        TempData["Market"] = "yes";
                    }
                    if (png == false && hos == false && res == 1 && laun == false && food == false && Guid == false && Market == false && pla == 1 && rest == 1 && hot == 1 && tran == 1 && trav == 1 && host == false && travVen == 1 && tranVen == 1 && restVen == 1 && hotVen == 1)
                    {
                        TempData["not"] = "no";
                        return Redirect(Url.Action("Index", "Home") + "#successMessage");
                    }
                    else
                    {
                        svm.CountryId = CountryId;
                        svm.CityId = CityId;
                        svm.Arrival = Arrival;
                        svm.Departure = Departure;
                        svm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        if (TempData["hostAvai"] != null)
                        {
                            TempData["hostAvai"] = "There is no data for your selection.";
                        }
                        if (TempData["offerAvai"] != null)
                        {
                            TempData["offerAvai"] = "There is no data for your selection.";
                        }
                        return View(svm);
                    }


                }
                #endregion


                else
                {
                    TempData["not"] = "no";
                    return Redirect(Url.Action("Index", "Home") + "#successMessage");

                }

            }
            catch (Exception ex)
            {

                return View("Error", new HandleErrorInfo(ex, "Home", "Index"));
            }

        }

        #endregion





        #region  Cascading dropdown for city and country

        [HttpPost]

        public JsonResult GetCityIdByCountryId(string countryId)
        {

            int countId;
            List<SelectListItem> cityList = new List<SelectListItem>();
            if (!string.IsNullOrEmpty(countryId))
            {
                countId = Convert.ToInt32(countryId);
                List<City> cities = entity.Cities.Where(x => x.CountryId == countId).ToList();
                cities.ForEach(x => { cityList.Add(new SelectListItem { Text = x.CityName, Value = x.CityId.ToString() }); });
            }


            return Json(cityList, JsonRequestBehavior.AllowGet);
        }


        #endregion


         #region website about
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        #endregion
     
       

        #region User SignIn

        [AllowAnonymous]
        public ActionResult SignInUser()
        {
            if (Session["message"] !=null)
            {
                Session["message"] = null;
                TempData["Problem"] = "The email or password is not matching.";
            }
            else
            {
                ViewBag.Message = "";
            }
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult SignInUser(SignInUserViewModel siuvm)
        {
           if(ModelState.IsValid)
            {
                try
                {
                    if(siuvm.UserType==1)
                    {
                        return RedirectToAction("SignIn","Vendor",new {Email=siuvm.Email, Password=siuvm.Password});
                    }
                    else if(siuvm.UserType == 2)
                    {
                        return RedirectToAction("SignIn", "Tourist", new { Email = siuvm.Email, Password = siuvm.Password });
                    }
                    else
                    {
                        TempData["Problem"] = "You have to choose a user type option.";
                        return View();
                    }
                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Home", "SignInUser"));
                }

            }
            else
            {
                return View(siuvm);
            }
           
        }


        #endregion

        #region user home
        public ActionResult UserPanel()
        {
            ViewBag.Message = "Your User Panel page.";
            if(TempData["Place"]!=null)
            {
                TempData["Place"] = "You dont have any new place to see";
            }
            return View();
        }
        #endregion


        #region LogOut

        public ActionResult LogOut()
        {

            Session.Abandon();
            Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));

            return RedirectToAction("Index");
        }
        #endregion



        #region subscribe

        [HttpPost]

        public ActionResult Subscriber(string mail)
        {
            try
            {
                Admin ad = new Admin();
                bool result = ad.Check(mail);
                if (result == false)
                {
                    string Subject = "Subscribed to ShepHerd";
                    string body = "Hello, You're now a valueable subscriber to our site. Now you will get to know every new places and facilites via our email. Thank you. ";

                    ad.SentMail(mail, body, Subject);
                    Subscriber sb = new Subscriber();
                    sb.SubscriberEmail = mail;
                    entity.Subscribers.Add(sb);
                    entity.SaveChanges();

                    ViewData["mail"] = "You're a subscribed member now.";
                }
                else
                {
                    ViewData["mail"] = "You're aleady a subscriber.";
                }
                return Redirect(Url.Action("Index", "Home") + "#subscribe");
        }
            catch (Exception ex)
            {

                return View("Error", new HandleErrorInfo(ex, "Home", "Subscriber"));
            }
        }

       
        #endregion


    }
}