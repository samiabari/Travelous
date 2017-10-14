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
    public class VendorController : Controller
    {
        TourDBEntities entity = new TourDBEntities();
        VendorLogic vl = new VendorLogic();

        #region Sign up

        [AllowAnonymous]
        public ActionResult SignUp()
        {
            VendorSignUpViewModel vsuvm = new VendorSignUpViewModel();
            vsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
            vsuvm.VendorTypeList=new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");
            return View(vsuvm);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult SignUp(VendorSignUpViewModel vsuvm)
        {
            
            if (ModelState.IsValid)
            {
                int VendorTypeId = Convert.ToInt32(vsuvm.VendorTypeId);
                int CityId = Convert.ToInt32(vsuvm.CityId);
                int CountryId = Convert.ToInt32(vsuvm.CountryId);
                try
                {

                    bool result;
                    result = vl.CheckVendorEmailExist(CityId, vsuvm.VendorEmail);
                    if (result == true)
                    {

                        vsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                        vsuvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");

                        TempData["Success"] = "Vendor's Email Duplicacy found for similar city.";
                        return View(vsuvm);

                    }
                    else
                    {

                        Vendor vendor = new Vendor();
                     
                        vendor.VendorTypeId = VendorTypeId;

                        vendor.VendorEmail = vsuvm.VendorEmail;
                        vendor.VendorPhnNo = vsuvm.VendorPhnNo;
                      
                        vendor.VendorOfficeName = vsuvm.VendorOfficeName;
                        vendor.CountryId = CountryId;
                        vendor.CityId = CityId;

                      
                        if (vsuvm.VendorOfficeWebsite==null)
                        {
                            vendor.VendorOfficeWebsite = "Not given yet";
                        }
                        else
                        {
                            vendor.VendorOfficeWebsite = vsuvm.VendorOfficeWebsite;
                        }
                        vendor.VendorName = "Not given yet";
                        vendor.VendorAddress = "Not given yet";
                        vendor.VendorOfficeMail = vsuvm.VendorOfficeMail;
                        vendor.VendorOfficePhnNo = "Not given yet"; 
                        vendor.VendorsVendorShipDetail= "Not given yet";
                        vendor.VendorOfficeAddress= "Not given yet";
                        vendor.VendorsStatus = true;
                        vendor.VendorOfferStatus = false;
                        vendor.VendorAdminsPermit = true;
                        vendor.VendorDateOfAccountCreation = DateTime.Now;
                        vendor.VendorPassword = vsuvm.VendorPassword;
                        vendor.VendorFavourite = 0;
                        


                        entity.Vendors.Add(vendor);
                        if (entity.SaveChanges() > 0)
                        {
                            string userCode = "vendor";
                            Session["userCode"] = userCode.ToString();
                            Session["VendorEmail"] = vsuvm.VendorEmail;
                            Session["VendorOfficeName"] = vsuvm.VendorOfficeName;
                            var d = (from c in entity.Vendors where c.VendorEmail == vsuvm.VendorEmail select c).FirstOrDefault();
                            int vendorId = Convert.ToInt32(d.VendorId);
                            Session["VendorId"] = vendorId;


                            Session["CountryId"] = vsuvm.CountryId;
                            Session["CityId"] = vsuvm.CityId;

                            Nullable<int> ProPicId = d.VendorProfilePicId;
                            if (ProPicId != null)
                            {
                                byte[] pic = (from c in entity.VendorPictures where c.VendorId == vendorId && c.AlbumTypeId == 1 orderby c.VendorPictureId descending select c.VendorPictureData).First();
                                if (pic != null)
                                {
                                    Session["pp"] = pic;
                                }
                            }

                            return RedirectToAction("UserPanel","Home");
                        }
                        else
                        {
                            vsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                            vsuvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");

                            TempData["Success"] = "Problem occured while creating account, Please try again later.";
                            return View(vsuvm);
                      
                        }
                    }
            }
                catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Vendor", "SignUp"));
            }

        }
            else
            {
                vsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                vsuvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");

                TempData["Success"] = "";
                return View(vsuvm);
            }

        }





        #endregion


        #region SignIn
      
        public ActionResult SignIn(string Email,string Password)
        {
            try
            {
                bool result;
                result = vl.SignInLogic(Email, Password);


                if (result == true)
                {
                    int VendorId = (int)Session["VendorId"];
                    var d = (from c in entity.Vendors where c.VendorId == VendorId select c).FirstOrDefault();
                    Nullable<int> ProPicId = d.VendorProfilePicId;
                    if (ProPicId != null)
                    {
                        byte[] pic = (from c in entity.VendorPictures where c.VendorId == VendorId && c.AlbumTypeId == 1 orderby c.VendorPictureId descending select c.VendorPictureData).First();
                        if (pic != null)
                        {
                            Session["pp"] = pic;
                        }
                    }
                    return RedirectToAction("UserPanel", "Home");
                }

                else
                {

                    Session["message"] = "no";
                    return RedirectToAction("SignInUser", "Home");
                }


            }
            catch (Exception ex)
            {

                return View("Error", new HandleErrorInfo(ex, "Vendor", "SignIn"));
            }

        }
        #endregion



        #region Change Password
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            if ((string)Session["userCode"] == "vendor")
            {
                int VendorId = (int)Session["VendorId"];
                VendorEditPasswordViewModel vepvm = new VendorEditPasswordViewModel();
                vepvm.VendorId = VendorId;
                return View(vepvm);
            }
            else
            {
                Session["user"] = "no";
                return RedirectToAction("Index","Home");
            }

          
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult ChangePassword(VendorEditPasswordViewModel vepvm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Vendor vendor = (from c in entity.Vendors where c.VendorId==vepvm.VendorId select c).FirstOrDefault();
                    if (vendor.VendorPassword == vepvm.VendorPassword)
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        vendor.VendorPassword = vepvm.VendorPassword;
                        if (entity.SaveChanges() > 0)
                        {
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            ViewData["message"] = "There's a problem going on. please try again later.";
                            return View(vepvm);
                        }
                            
                  }

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Vendor", "ChangePassword"));
                }
            }
            else
            {
                return View(vepvm);
            }

        }
        #endregion

        #region Change AccountEmail
        [AllowAnonymous]
        public ActionResult ChangeAccountEmail()
        {
            if ((string)Session["userCode"] == "vendor")
            {
                int VendorId = (int)Session["VendorId"];
                VendorChangeEmailViewModel vcevm = new VendorChangeEmailViewModel();
                vcevm.VendorId = VendorId;
                vcevm.VendorEmail = (from c in entity.Vendors where c.VendorId == vcevm.VendorId select c.VendorEmail).FirstOrDefault();
                return View(vcevm);
            }
            else
            {
                Session["user"] = "no";
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult ChangeAccountEmail(VendorChangeEmailViewModel vcevm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Vendor vendor = (from c in entity.Vendors where c.VendorId == vcevm.VendorId select c).FirstOrDefault();
                    if (vendor.VendorEmail == vcevm.VendorEmail)
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        bool result = vl.CheckVendorEmailExist(Convert.ToInt32(vendor.CityId),vcevm.VendorEmail);
                        if (result == true)
                        {
                            ViewData["message"] = "Email duplicacy found.";
                            return View(vcevm);
                        }
                        else
                        {
                            vendor.VendorEmail = vcevm.VendorEmail;
                            if (entity.SaveChanges() > 0)
                            {
                                return RedirectToAction("UserPanel", "Home");
                            }
                            else
                            {
                                ViewData["message"] = "There's a problem going on. please try again later.";
                                return View(vcevm);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Vendor", "ChangeAccountEmail"));
                }
            }
            else
            {
                return View(vcevm);
            }

        }
        #endregion




        #region Change own Detail
        [AllowAnonymous]
        public ActionResult ChangeOwnDetail()
        {
            if ((string)Session["userCode"] == "vendor")
            {
                int VendorId = (int)Session["VendorId"];
                VendorOwnDetailViewModel vodvm = new VendorOwnDetailViewModel();
                vodvm.VendorId = VendorId;
                Vendor vendor = (from c in entity.Vendors where c.VendorId == vodvm.VendorId select c).FirstOrDefault();
                vodvm.VendorName = vendor.VendorName;
                vodvm.VendorPhnNo = vendor.VendorPhnNo;
                vodvm.VendorAddress = vendor.VendorAddress;
                return View(vodvm);
            }
            else
            {
                Session["user"] = "no";
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult ChangeOwnDetail(VendorOwnDetailViewModel vodvm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Vendor vendor = (from c in entity.Vendors where c.VendorId == vodvm.VendorId select c).FirstOrDefault();
                    if((vodvm.VendorName=="Not given yet")||(vodvm.VendorPhnNo=="Not given yet")||(vodvm.VendorAddress== "Not given yet"))
                    {

                        ViewData["message"] = "For security purpose you should add your detail here.";
                        return View(vodvm);
                    }
                    else if ((vendor.VendorName == vodvm.VendorName)&& (vodvm.VendorPhnNo==vendor.VendorPhnNo)&& (vodvm.VendorAddress==vendor.VendorAddress))
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        vendor.VendorPhnNo = vodvm.VendorPhnNo;
                        vendor.VendorName = vodvm.VendorName;
                        vendor.VendorAddress = vodvm.VendorAddress;
                            if (entity.SaveChanges() > 0)
                            {
                                return RedirectToAction("UserPanel", "Home");
                            }
                            else
                            {
                                ViewData["message"] = "There's a problem going on. please try again later.";
                                return View(vodvm);
                            }
                     }
                    

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Vendor", "ChangeOwnDetail"));
                }
            }
            else
            {
                return View(vodvm);
            }

        }
        #endregion


        #region Change VendorShipDetail
        [AllowAnonymous]
        public ActionResult VendorShipDetail()
        {
            if ((string)Session["userCode"] == "vendor")
            {
                int VendorId = (int)Session["VendorId"];
                VendorsVendorShipDetailViewModel vvddvm = new VendorsVendorShipDetailViewModel();
                vvddvm.VendorId = VendorId;
                Vendor vendor = (from c in entity.Vendors where c.VendorId == vvddvm.VendorId select c).FirstOrDefault();
                vvddvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                vvddvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");
                vvddvm.VendorOfficeName = vendor.VendorOfficeName;
                vvddvm.VendorOfficePhnNo = vendor.VendorOfficePhnNo;
              
                vvddvm.VendorOfficeWebsite = vendor.VendorOfficeWebsite;
                vvddvm.VendorOfficeAddress = vendor.VendorOfficeAddress;
                vvddvm.VendorOfficeMail = vendor.VendorOfficeMail;

                   
                return View(vvddvm);
            }
            else
            {
                Session["user"] = "no";
                return RedirectToAction("Index", "Home");
            }


        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult VendorShipDetail(VendorsVendorShipDetailViewModel vvddvm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    Vendor vendor = (from c in entity.Vendors where c.VendorId == vvddvm.VendorId select c).FirstOrDefault();
                 
                    if (vvddvm.VendorsVendorShipDetail==null)
                    {
                        vvddvm.VendorsVendorShipDetail = vendor.VendorsVendorShipDetail;
                    }
                    if(vvddvm.CountryId==null)
                    {
                        vvddvm.CountryId = vendor.CountryId;
                    }
                    if(vvddvm.CityId == null)
                    {
                        vvddvm.CityId = vendor.CityId;
                    }
                    if (vvddvm.VendorTypeId == null)
                    {
                        vvddvm.VendorTypeId = vendor.VendorTypeId;
                    }
                    



                    if (vvddvm.VendorsVendorShipDetail == "Not given yet")
                    {
                        ViewData["message"] = "For security purpose you should add your VendorShip detail here.";
                        return View(vvddvm);
                    }
                    if ((vvddvm.VendorOfficeName == "Not given yet") || (vvddvm.VendorOfficePhnNo == "Not given yet") || (vvddvm.VendorOfficeWebsite == "Not given yet") || (vvddvm.VendorOfficeAddress == "Not given yet") || (vvddvm.VendorOfficeMail == "Not given yet"))
                    {

                        ViewData["message"] = "For security purpose you should add your not given values here.";
                        return View(vvddvm);
                    }
                    if ((vendor.VendorOfficeName == vvddvm.VendorOfficeName) && (vendor.VendorOfficePhnNo == vvddvm.VendorOfficePhnNo) && (vendor.CountryId == vvddvm.CountryId) && (vendor.CityId == vvddvm.CityId) && (vendor.VendorTypeId == vvddvm.VendorTypeId) && (vendor.VendorOfficeWebsite == vvddvm.VendorOfficeWebsite) && (vendor.VendorOfficeAddress == vvddvm.VendorOfficeAddress) && (vendor.VendorOfficeMail == vvddvm.VendorOfficeMail) && (vendor.VendorsVendorShipDetail == vvddvm.VendorsVendorShipDetail))
                    {

                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        vendor.VendorOfficeName = vvddvm.VendorOfficeName;
                        vendor.VendorOfficePhnNo = vvddvm.VendorOfficePhnNo;
                        vendor.CountryId = vvddvm.CountryId;
                        vendor.CityId = vvddvm.CityId;
                        vendor.VendorTypeId = vvddvm.VendorTypeId;
                        vendor.VendorOfficeWebsite = vvddvm.VendorOfficeWebsite;
                        vendor.VendorOfficeAddress = vvddvm.VendorOfficeAddress;
                        vendor.VendorOfficeMail = vvddvm.VendorOfficeMail;
                        vendor.VendorsVendorShipDetail = vvddvm.VendorsVendorShipDetail;


                        if (entity.SaveChanges() > 0)
                        {
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            vvddvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                            vvddvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");
                            ViewData["message"] = "There's a problem occured, please try again later.";
                            return View(vvddvm);
                        }
                    }
            }
                catch (Exception ex)
            {

                return View("Error", new HandleErrorInfo(ex, "Vendor", "VendorShipDitail"));
            }
        }
            else
            {
                vvddvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                vvddvm.VendorTypeList = new SelectList(entity.VendorTypes, "VendorTypeId", "VendorTypeName");
                return View(vvddvm);
            }

        }
        #endregion


        #region add Profile picture
        [HttpPost]
        public ActionResult AddProfilePic(int vendorId, int albumTypeId, HttpPostedFileBase AddPicture)
        {
            try
            {
                if (Session["userCode"] != null)
                {


                bool result = vl.CheckVendorIdnAlbumTypeIdInVendorAlbum(vendorId, albumTypeId);

                    Vendor vendor = (from c in entity.Vendors where c.VendorId == vendorId select c).FirstOrDefault();
                    VendorPicture tp = new VendorPicture();
                    if (result == true)
                    {
                        var albumId = (from c in entity.VendorAlbums where c.VendorId == vendorId && c.AlbumTypeId == albumTypeId select c.VendorAlbumId).Single();
                        int AlbumId = Convert.ToInt32(albumId);
                        if (AddPicture != null)
                        {

                            tp.VendorPictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(tp.VendorPictureData, 0, AddPicture.ContentLength);



                        }
                        else
                        {
                            Session["message"] = "no picture has been selected";
                            return RedirectToAction("UserPanel", "Home");
                        }
                        tp.VendorId = vendorId;
                        tp.VendorAlbumId = AlbumId;
                        tp.AlbumTypeId = albumTypeId;
                        tp.VendorPictureAdminPermit = true;
                        tp.CountryId = vendor.CountryId;
                        tp.CityId = vendor.CityId;

                        tp.VendorPicturePrivacy = true;
                        tp.VendorPictureDateOfAdded = DateTime.Now;
                        entity.VendorPictures.Add(tp);

                        if (entity.SaveChanges() > 0)
                        {
                            var pictureId = (from c in entity.VendorPictures
                                             where c.VendorId == vendorId && c.AlbumTypeId == albumTypeId
                                             orderby c.VendorPictureId descending
                                             select c.VendorPictureId).First();
                            vendor.VendorProfilePicId = Convert.ToInt32(pictureId);
                            entity.SaveChanges();

                            var d = (from c in entity.Vendors where c.VendorId == vendorId select c).FirstOrDefault();
                            Nullable<int> ProPicId = d.VendorProfilePicId;
                            if (ProPicId != null)
                            {
                                byte[] pic = (from c in entity.VendorPictures where c.VendorId == vendorId && c.AlbumTypeId == 1 orderby c.VendorPictureId descending select c.VendorPictureData).First();
                                if (pic != null)
                                {
                                    Session["pp"] = pic;
                                }
                            }
                            Session["message"] = "Profile Picture added";
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            Session["message"] = "not added";
                            return RedirectToAction("UserPanel", "Home");
                        }
                    }
                    else
                    {
                        if (AddPicture != null)
                        {

                            tp.VendorPictureData = new byte[AddPicture.ContentLength];
                            AddPicture.InputStream.Read(tp.VendorPictureData, 0, AddPicture.ContentLength);

                        }
                        else
                        {
                            Session["message"] = "no picture has been selected";
                            return RedirectToAction("UserPanel", "Home");
                        }

                        VendorAlbum ta = new VendorAlbum();
                        ta.VendorAlbumName = "Profile Picture";
                        ta.VendorId = vendorId;
                        ta.AlbumTypeId = albumTypeId;
                        ta.CityId = vendor.CityId;
                        ta.CountryId = vendor.CountryId;

                        ta.VendorAlbumAdminPermit = true;
                        ta.VendorAlbumPrivacy = true;
                        ta.VendorDateOfAlbumCreated = DateTime.Now;
                        entity.VendorAlbums.Add(ta);
                        entity.SaveChanges();
                        tp.VendorId = vendorId;
                        var albumId = (from c in entity.VendorAlbums where c.VendorId == vendorId && c.AlbumTypeId == albumTypeId select c.VendorAlbumId).Single();
                        int AlbumId = Convert.ToInt32(albumId);
                        tp.VendorAlbumId = AlbumId;
                        tp.AlbumTypeId = albumTypeId;
                        tp.VendorPictureAdminPermit = true;
                        tp.CountryId = vendor.CountryId;
                        tp.CityId = vendor.CityId;

                        tp.VendorPicturePrivacy = true;
                        tp.VendorPictureDateOfAdded = DateTime.Now;
                        entity.VendorPictures.Add(tp);
                        if (entity.SaveChanges() > 0)
                        {
                            int pictureId = (from c in entity.VendorPictures
                                             where c.VendorId == vendorId && c.AlbumTypeId == albumTypeId
                                             orderby c.VendorPictureId descending
                                             select c.VendorPictureId).First();
                            vendor.VendorProfilePicId = pictureId;
                            entity.SaveChanges();

                            var d = (from c in entity.Vendors where c.VendorId == vendorId select c).FirstOrDefault();
                            Nullable<int> ProPicId = d.VendorProfilePicId;
                            if (ProPicId != null)
                            {
                                byte[] pic = (from c in entity.VendorPictures where c.VendorId == vendorId && c.AlbumTypeId == 1 orderby c.VendorPictureId descending select c.VendorPictureData).First();
                                if (pic != null)
                                {
                                    Session["pp"] = pic;
                                }
                            }

                            Session["message"] = "Profile Picture added";
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            Session["message"] = "not added";
                            return RedirectToAction("UserPanel", "Home");
                        }

                    }

                }
                else
                {
                    Session["message"] = "please log in to See the List";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Vendor", "AddProfilePic"));
            }
        }

        #endregion


        #region DeactivateAccountByVendor

        public ActionResult DeactivateAccountByVendor()
        {
            try
            {
                if (Session["userCode"] != null)
                {
                    int VendorId = (int)Session["VendorId"];
                    Vendor vendor = (from c in entity.Vendors where c.VendorId == VendorId select c).FirstOrDefault();
                    vendor.VendorsStatus = false;
                    if (vendor.VendorOfferStatus == true)
                    {
                        var offer = (from c in entity.Offers where c.VendorId == VendorId && c.OfferStatus == true select c).ToList();
                        foreach (var i in offer)
                        {
                            i.OfferStatus = false;
                        }
                   }
                    entity.SaveChanges();
                    Session["message"] = "Please Come back again...";
                    return RedirectToAction("LogOut", "Home");
                }
                else
                {
                    Session["message"] = "please log in to See the List";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Vendor", "DeactivateAccountByVendor"));
            }
        }

        #endregion




        #region New VendorList Admin
        public ActionResult NewVendorListAdmin()
        {

            List<VendorDetailViewModel> sp = new List<VendorDetailViewModel>();
            DateTime t = DateTime.Now.AddDays(-2);
            var p= (from c in entity.Vendors
                     join e in entity.VendorPictures on c.VendorProfilePicId
                     equals e.VendorPictureId into ppl
                     from e in ppl.DefaultIfEmpty()

                     join f in entity.Countries on c.CountryId equals f.CountryId
                     join g in entity.Cities on c.CityId equals g.CityId
                     join k in entity.VendorTypes on c.VendorTypeId equals k.VendorTypeId
                     where c.VendorDateOfAccountCreation >= t
                     orderby c.VendorFavourite descending
                     select new
                     {
                         c.VendorId,
                         c.VendorOfficeName,
                         e.VendorPictureData,
                         c.VendorOfficeAddress,
                         c.VendorOfficeWebsite,
                         c.VendorOfficePhnNo,
                         k.VendorTypeId,
                         k.VendorTypeName,
                         c.CountryId,
                         c.CityId,
                         c.VendorFavourite,
                         c.VendorsVendorShipDetail,
                         g.CityName,
                         f.CountryName,
                      
                         c.VendorOfferStatus,
                         c.VendorsStatus,
                         c.VendorAdminsPermit

                     }).ToList();

            int x = p.Count();
            if (x > 0)
            {
                foreach (var o in p)
                {
                    VendorDetailViewModel oivm = new VendorDetailViewModel();
                    oivm.VendorId = o.VendorId;
                    oivm.VendorName = o.VendorOfficeName;
                    oivm.VendorAddress = o.VendorOfficeAddress;
                    oivm.VendorPhnNo = o.VendorOfficePhnNo;
                    oivm.VendorWebsite = o.VendorOfficeWebsite;
                    oivm.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                    oivm.VendorTypeName = o.VendorTypeName;
                    oivm.CityId = Convert.ToInt32(o.CityId);
                    oivm.CityName = o.CityName;
                    oivm.CountryId = Convert.ToInt32(o.CountryId);
                    oivm.CountryName = o.CountryName;
                    oivm.VendorPictureData = o.VendorPictureData;
                    oivm.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                    oivm.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                    string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                    oivm.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                    oivm.VendorFullDetail = o.VendorsVendorShipDetail;
                    oivm.VendorLike = Convert.ToInt32(o.VendorFavourite);
                 
                    oivm.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);
                    sp.Add(oivm);
                }
                var q = sp;


                return View(q);
            }
            else
            {
                TempData["Vendor"] = "No data found";
                return RedirectToAction("UserPanel", "Home");
            }

        }

        #endregion

        #region New VendorList Admin
        public ActionResult AdminVendorList(int VendorTypeId,int CountryId,int CityId)
        {

            List<VendorDetailViewModel> sp = new List<VendorDetailViewModel>();
            DateTime t = DateTime.Now.AddDays(-2);
            var p = (from c in entity.Vendors
                     join e in entity.VendorPictures on c.VendorProfilePicId
                     equals e.VendorPictureId into ppl
                     from e in ppl.DefaultIfEmpty()

                     join f in entity.Countries on c.CountryId equals f.CountryId
                     join g in entity.Cities on c.CityId equals g.CityId
                     join k in entity.VendorTypes on c.VendorTypeId equals k.VendorTypeId
                     where c.VendorTypeId == VendorTypeId && c.VendorsStatus == true && c.VendorAdminsPermit == true
                     orderby c.VendorFavourite descending
                     select new
                     {
                         c.VendorId,
                         c.VendorOfficeName,
                         e.VendorPictureData,
                         c.VendorOfficeAddress,
                         c.VendorOfficeWebsite,
                         c.VendorOfficePhnNo,
                         k.VendorTypeId,
                         k.VendorTypeName,
                         c.CountryId,
                         c.CityId,
                         c.VendorFavourite,
                         c.VendorsVendorShipDetail,
                         g.CityName,
                         f.CountryName,
                        
                         c.VendorOfferStatus,
                         c.VendorsStatus,
                         c.VendorAdminsPermit

                     }).ToList();

            int x = p.Count();
            if (x > 0)
            {
                foreach (var o in p)
                {
                    VendorDetailViewModel oivm = new VendorDetailViewModel();
                    oivm.VendorId = o.VendorId;
                    oivm.VendorName = o.VendorOfficeName;
                    oivm.VendorAddress = o.VendorOfficeAddress;
                    oivm.VendorPhnNo = o.VendorOfficePhnNo;
                    oivm.VendorWebsite = o.VendorOfficeWebsite;
                    oivm.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                    oivm.VendorTypeName = o.VendorTypeName;
                    oivm.CityId = Convert.ToInt32(o.CityId);
                    oivm.CityName = o.CityName;
                    oivm.CountryId = Convert.ToInt32(o.CountryId);
                    oivm.CountryName = o.CountryName;
                    oivm.VendorPictureData = o.VendorPictureData;
                    oivm.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                    oivm.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                    string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                    oivm.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                    oivm.VendorFullDetail = o.VendorsVendorShipDetail;
                    oivm.VendorLike = Convert.ToInt32(o.VendorFavourite);
                   
                    oivm.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);
                    sp.Add(oivm);
                }
                var q = sp;


                return View(q);
            }
            else
            {
                TempData["Vendor"] = "No data found";
                return RedirectToAction("UserPanel", "Home");
            }

        }

        #endregion

        #region New VendorList Admin
        public ActionResult SearchVendorList(int VendorTypeId, int CountryId, int CityId)
        {

            List<VendorDetailViewModel> sp = new List<VendorDetailViewModel>();
            Session["SearchVendorList"] = "yes";
            Session["VendorTypeId"] = VendorTypeId;
            var p = (from c in entity.Vendors
                     join e in entity.VendorPictures on c.VendorProfilePicId
                     equals e.VendorPictureId into ppl
                     from e in ppl.DefaultIfEmpty()

                     join f in entity.Countries on c.CountryId equals f.CountryId
                     join g in entity.Cities on c.CityId equals g.CityId
                     join k in entity.VendorTypes on c.VendorTypeId equals k.VendorTypeId
                     where c.VendorTypeId == VendorTypeId && c.VendorsStatus == true && c.VendorAdminsPermit == true && c.CountryId==CountryId && c.CityId==CityId
                     orderby c.VendorFavourite descending
                     select new
                     {
                         c.VendorId,
                         c.VendorOfficeName,
                         e.VendorPictureData,
                         c.VendorOfficeAddress,
                         c.VendorOfficeWebsite,
                         c.VendorOfficePhnNo,
                         k.VendorTypeId,
                         k.VendorTypeName,
                         c.CountryId,
                         c.CityId,
                         c.VendorFavourite,
                         c.VendorsVendorShipDetail,
                         g.CityName,
                         f.CountryName,

                         c.VendorOfferStatus,
                         c.VendorsStatus,
                         c.VendorAdminsPermit

                     }).ToList();

            int x = p.Count();
            if (x > 0)
            {
                foreach (var o in p)
                {
                    VendorDetailViewModel oivm = new VendorDetailViewModel();
                    oivm.VendorId = o.VendorId;
                    oivm.VendorName = o.VendorOfficeName;
                    oivm.VendorAddress = o.VendorOfficeAddress;
                    oivm.VendorPhnNo = o.VendorOfficePhnNo;
                    oivm.VendorWebsite = o.VendorOfficeWebsite;
                    oivm.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                    oivm.VendorTypeName = o.VendorTypeName;
                    oivm.CityId = Convert.ToInt32(o.CityId);
                    oivm.CityName = o.CityName;
                    oivm.CountryId = Convert.ToInt32(o.CountryId);
                    oivm.CountryName = o.CountryName;
                    oivm.VendorPictureData = o.VendorPictureData;
                    oivm.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                    oivm.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                    string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                    oivm.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                    oivm.VendorFullDetail = o.VendorsVendorShipDetail;
                    oivm.VendorLike = Convert.ToInt32(o.VendorFavourite);

                    oivm.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);
                    sp.Add(oivm);
                }
                var q = sp;


                return View(q);
            }
            else
            {
                TempData["Vendor"] = "No data found";
                return RedirectToAction("Index", "Home");
            }

        }

#endregion




        #region Vendordetails

        public ActionResult SearchVendorDetail(int VendorId)
        {

            VendorDetailViewModel sp = new VendorDetailViewModel();
            if (TempData["CheckInVendorSearch"] != null)
            {
                TempData["CheckInVendorSearch"] = TempData["CheckInVendorSearch"];
            }
            if (TempData["AddToWishVendorSearch"] != null)
            {
                TempData["AddToWishVendorSearch"] = TempData["AddToWishVendorSearch"];
            }
            if (TempData["BlockMessageSearch"] != null)
            {
                TempData["BlockMessageSearch"] = TempData["BlockMessageSearch"];
            }
            if (VendorId != 0)
            {
                var o = (from c in entity.Vendors
                         join e in entity.VendorPictures on c.VendorProfilePicId
                         equals e.VendorPictureId into ppl
                         from e in ppl.DefaultIfEmpty()

                         join f in entity.Countries on c.CountryId equals f.CountryId
                         join g in entity.Cities on c.CityId equals g.CityId
                         join k in entity.VendorTypes on c.VendorTypeId equals k.VendorTypeId
                         where c.VendorId ==VendorId
                         orderby c.VendorFavourite descending
                         select new
                         {
                             c.VendorId,
                             c.VendorOfficeName,
                             e.VendorPictureData,
                             c.VendorOfficeAddress,
                             c.VendorOfficeWebsite,
                             c.VendorOfficePhnNo,
                             k.VendorTypeId,
                             k.VendorTypeName,
                             c.CountryId,
                             c.CityId,
                             c.VendorFavourite,
                             c.VendorsVendorShipDetail,
                             g.CityName,
                             f.CountryName,
                            
                             c.VendorOfferStatus,
                             c.VendorsStatus,
                             c.VendorAdminsPermit

                         }).First();

                if (o != null)
                {

                    sp.VendorId = o.VendorId;
                    sp.VendorName = o.VendorOfficeName;
                    sp.VendorAddress = o.VendorOfficeAddress;
                    sp.VendorPhnNo = o.VendorOfficePhnNo;
                    sp.VendorWebsite = o.VendorOfficeWebsite;
                    sp.VendorTypeId = Convert.ToInt32(o.VendorTypeId);
                    sp.VendorTypeName = o.VendorTypeName;
                    sp.CityId = Convert.ToInt32(o.CityId);
                    sp.CityName = o.CityName;
                    sp.CountryId = Convert.ToInt32(o.CountryId);
                    sp.CountryName = o.CountryName;
                    sp.VendorPictureData = o.VendorPictureData;
                    sp.VendorStatus = Convert.ToBoolean(o.VendorsStatus);
                    sp.VendorOfferStatus = Convert.ToBoolean(o.VendorOfferStatus);
                    string VendorShortDetail = CutDetail.StripHTML(o.VendorsVendorShipDetail);
                    sp.VendorShortDetail = CutDetail.Truncate(VendorShortDetail, 40);
                    sp.VendorFullDetail = o.VendorsVendorShipDetail;
                    sp.VendorLike = Convert.ToInt32(o.VendorFavourite);
                   
                    sp.VendorAdminPermit = Convert.ToBoolean(o.VendorAdminsPermit);


                }
                var q = sp;
               
                return View(q);

            }

            else
            {
                

                
                    return RedirectToAction("Index", "Home");
                
            }
        }


        #endregion




        #region block Vendor Search View

        public ActionResult BlockVendorSearch(int VendorId)
        {
            if ((string)Session["userCode"] == "admin")
            {

                try
                {

                    if (VendorId != 0)
                    {

                        bool result = vl.BlockVendor(VendorId);
                        if (result == true)
                        {
                            TempData["BlockMessageSearch"] = "Done";
                        }
                        else
                        {
                            TempData["BlockMessageSearch"] = "Not done yet";
                        }
                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId});
                    }
                    
                  
                    else
                    {

                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId});
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Vendor", "BlockVendor"));
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

        public ActionResult UnBlockVendorSearch(int VendorId)
        {
            if ((string)Session["userCode"] == "admin")
            {

                try
                {
                    if (VendorId != 0)
                    {

                        bool result = vl.UnBlockVendor(VendorId);
                        if (result == true)
                        {
                            TempData["BlockMessageSearch"] = "Done";
                        }
                        else
                        {
                            TempData["BlockMessageSearch"] = "Not done yet";
                        }
                       

                            return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId });
                      
                    }
                   
                    else
                    {

                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId});
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Vendor", "BlockPlace"));
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

        public ActionResult searchCheckIn(int VendorId)
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                try
                {
                    bool result = vl.CheckCheckIn(VendorId, TouristId);
                    if (result == false)
                    {
                        TempData["CheckInVendorSearch"] = "Done";
                        vl.CheckInPlace(VendorId, TouristId);
                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId });
                    }
                    else
                    {
                        TempData["AddToWishVendorSearch"] = "You already have that for today";
                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId });
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Vendor", "searchCheckIn"));
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

        public ActionResult searchAddToWishVendor(int VendorId)
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                try
                {
                    bool result = vl.CheckWishIn(VendorId, TouristId);
                    if (result == false)
                    {
                        TempData["AddToWishPlaceSearch"] = "Done";
                        vl.AddWishVendor(VendorId, TouristId);
                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId });
                    }
                    else
                    {
                        TempData["AddToWishPlaceSearch"] = "You already have this place";
                        return RedirectToAction("SearchVendorDetail", "Vendor", new { VendorId = VendorId });
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Vendor", "searchAddToWishPlace"));
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