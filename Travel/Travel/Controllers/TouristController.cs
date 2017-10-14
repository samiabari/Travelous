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
    public class TouristController : Controller
    {
        TourDBEntities entity = new TourDBEntities();

        TouristLogic tl = new TouristLogic();

        #region SignUp
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            TouristSignUpViewModel tsuvm = new TouristSignUpViewModel();
            tsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");

            return View(tsuvm);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]

        public ActionResult SignUp(TouristSignUpViewModel tsuvm)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    bool result;
                    result = tl.CheckEmailExist(tsuvm.TouristEmail);
                    if (result == true)
                    {

                        tsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");

                        ViewData["message"] = "Email Duplicate found.";
                        return View(tsuvm);

                    }
                    else
                    {
                        Tourist tourist = new Tourist();
                        tourist.TouristName = tsuvm.TouristName;
                        tourist.TouristEmail = tsuvm.TouristEmail;
                        tourist.CountryId = Convert.ToInt32(tsuvm.CountryId);
                        tourist.CityId = Convert.ToInt32(tsuvm.CityId);

                        tourist.TouristPhnNo = tsuvm.TouristPhnNo;
                        tourist.TouristAddress = "Not given yet";
                        tourist.TouristEmergencyPhnNo= "Not given yet";
                        tourist.TouristsStatus = true;
                        tourist.TouristsHostStatus = false;
                        tourist.TouristAdminsPermit = true;
                        tourist.TouristDateOfAccountCreation = DateTime.Now;
                        tourist.TouristPassword = tsuvm.TouristPassword;
                      
                        entity.Tourists.Add(tourist);
                    if (entity.SaveChanges() > 0)
                    {
                        ModelState.Clear();
                        string userCode = "tourist";
                        Session["userCode"] = userCode.ToString();
                        Session["TouristEmail"] = tsuvm.TouristEmail;
                        Session["TouristName"] = tsuvm.TouristName;
                         var d = (from c in entity.Tourists where c.TouristEmail == tsuvm.TouristEmail select c).FirstOrDefault();
                           Session["TouristId"] = d.TouristId;
                        Nullable<int> ProPicId =d.TouristProfilePicId;    
                            int touristId = (int)Session["TouristId"];
                            Session["CountryId"] = tsuvm.CountryId;
                            Session["CityId"] = tsuvm.CityId;
                        if (ProPicId != null)
                        {
                            byte[] pic = (from c in entity.TouristPictures where c.TouristId == touristId && c.AlbumTypeId == 1 orderby c.TouristPictureId descending select c.TouristPictureData).First();
                            if (pic != null)
                            {
                                Session["pp"] = pic;
                            }
                        }
                          

                            string subject = "Registered to Shepherd";
                            string message = "Congratulations, You're now a registered tourist of our site. For any new offers and places you'll get notified via an Email. Thank you.";
                            Admin ad = new Admin();
                            ad.SentMail(tsuvm.TouristEmail, message, subject);
                            Subscriber sb = new Subscriber();
                            sb.SubscriberEmail = tsuvm.TouristEmail;
                            entity.Subscribers.Add(sb);
                            entity.SaveChanges();
                            

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            tsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");


                            ViewData["message"] = "not saved";
                            return View(tsuvm);
                        }
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Tourist", "SignUp"));
                }

            }
            else
            {
                tsuvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
                return View(tsuvm);
            }

        }
        #endregion

      



        #region Tourist signIn

        public ActionResult SignIn(string Email, string Password)
        {

             try
                {
                    bool result = tl.SignInLogic(Email, Password);
                    if (result == true)
                    {
                    int touristId = (int)Session["TouristId"];
                    var d = (from c in entity.Tourists where c.TouristId == touristId select c).FirstOrDefault();
                    Nullable<int> ProPicId = d.TouristProfilePicId;
                    if (ProPicId != null)
                    {
                        byte[] pic = (from c in entity.TouristPictures where c.TouristId == touristId && c.AlbumTypeId == 1 orderby c.TouristPictureId descending select c.TouristPictureData).First();
                        if (pic != null)
                        {
                            Session["pp"] = pic;
                        }
                    }

                    return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Session["message"] = "no";
                        return RedirectToAction("SignInUser", "Home");
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Tourist", "SignIn"));
                }
          

        }
        #endregion



        #region Change Password
        [AllowAnonymous]
        public ActionResult ChangePassword()
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                TouristEditPasswordViewModel tepvm = new TouristEditPasswordViewModel();
                tepvm.TouristId = TouristId;
                return View(tepvm);
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

        public ActionResult ChangePassword(TouristEditPasswordViewModel tepvm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Tourist tourist = (from c in entity.Tourists where c.TouristId == tepvm.TouristId select c).FirstOrDefault();
                    if (tourist.TouristPassword == tepvm.TouristPassword)
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        tourist.TouristPassword = tepvm.TouristPassword;
                        if (entity.SaveChanges() > 0)
                        {
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            ViewData["message"] = "There's a problem going on. please try again later.";
                            return View(tepvm);
                        }

                    }

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Tourist", "ChangePassword"));
                }
            }
            else
            {
                return View(tepvm);
            }

        }
        #endregion



        #region Change AccountEmail
        [AllowAnonymous]
        public ActionResult ChangeAccountEmail()
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                TouristChangeEmailViewModel tcevm = new TouristChangeEmailViewModel();
                tcevm.TouristId = TouristId;
                tcevm.TouristEmail = (from c in entity.Tourists where c.TouristId == tcevm.TouristId select c.TouristEmail).FirstOrDefault();
                return View(tcevm);
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

        public ActionResult ChangeAccountEmail(TouristChangeEmailViewModel tcevm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Tourist tourist = (from c in entity.Tourists where c.TouristId == tcevm.TouristId select c).FirstOrDefault();
                    if (tourist.TouristEmail == tcevm.TouristEmail)
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        bool result = tl.CheckEmailExist(tcevm.TouristEmail);
                        if (result == true)
                        {
                            ViewData["message"] = "Email duplicacy found.";
                            return View(tcevm);
                        }
                        else
                        {
                            tourist.TouristEmail = tcevm.TouristEmail;
                            if (entity.SaveChanges() > 0)
                            {
                                return RedirectToAction("UserPanel", "Home");
                            }
                            else
                            {
                                ViewData["message"] = "There's a problem going on. please try again later.";
                                return View(tcevm);
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Tourist", "ChangeAccountEmail"));
                }
            }
            else
            {
                return View(tcevm);
            }

        }
        #endregion



        #region Change AccountEmail
        [AllowAnonymous]
        public ActionResult ChangeEmergencyInfo()
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                Tourist tourist = (from c in entity.Tourists where c.TouristId == TouristId select c).FirstOrDefault();
                TouristEmergencyInfoViewModel tcevm = new TouristEmergencyInfoViewModel();
                tcevm.TouristId = TouristId;
                tcevm.TouristEmergencyPhnNo = tourist.TouristEmergencyPhnNo;
                tcevm.TouristAddress = tourist.TouristAddress;
                return View(tcevm);
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

        public ActionResult ChangeEmergencyInfo(TouristEmergencyInfoViewModel tcevm)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    Tourist tourist = (from c in entity.Tourists where c.TouristId == tcevm.TouristId select c).FirstOrDefault();
                    if ((tcevm.TouristEmergencyPhnNo == "Not given yet") || (tcevm.TouristAddress == "Not given yet"))
                    {
                        ViewData["message"] = "You should give your emergency phn no. and address for security purpose.";
                        return View(tcevm);
                    }


                    if ((tcevm.TouristEmergencyPhnNo == tourist.TouristEmergencyPhnNo)&& (tcevm.TouristAddress == tourist.TouristAddress))
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                       
                       
                            tourist.TouristEmergencyPhnNo = tcevm.TouristEmergencyPhnNo;
                            tourist.TouristAddress = tcevm.TouristAddress;
                            if (entity.SaveChanges() > 0)
                            {
                                return RedirectToAction("UserPanel", "Home");
                            }
                            else
                            {
                                ViewData["message"] = "There's a problem going on. please try again later.";
                                return View(tcevm);
                            }
                       
                    }

                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Tourist", "TouristEmergencyInfoViewModel"));
                }
            }
            else
            {
                return View(tcevm);
            }

        }
        #endregion



        #region Change Own Detail
        [AllowAnonymous]
        public ActionResult TouristDetail()
        {
            if ((string)Session["userCode"] == "tourist")
            {
                int TouristId = (int)Session["TouristId"];
                TouristUpdateDetailViewModel vvddvm = new TouristUpdateDetailViewModel();
                vvddvm.TouristId = TouristId;
                Tourist tourist = (from c in entity.Tourists where c.TouristId == vvddvm.TouristId select c).FirstOrDefault();
                vvddvm.CountryList = new SelectList(entity.Countries, "CountryId", "CountryName");
            
                vvddvm.TouristName = tourist.TouristName;
                vvddvm.TouristPhnNo = tourist.TouristPhnNo;

             
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

        public ActionResult TouristDetail(TouristUpdateDetailViewModel vvddvm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Tourist tourist = (from c in entity.Tourists where c.TouristId == vvddvm.TouristId select c).FirstOrDefault();

                 
                    if (vvddvm.CountryId == null)
                    {
                        vvddvm.CountryId = tourist.CountryId;
                    }
                    if (vvddvm.CityId == null)
                    {
                        vvddvm.CityId = tourist.CityId;
                    }
                
                    if ((vvddvm.TouristName == "Not given yet") || (vvddvm.TouristPhnNo == "Not given yet"))
                    {

                        ViewData["message"] = "For security purpose you should add your not given values here.";
                        return View(vvddvm);
                    }
                    if ((tourist.TouristName == vvddvm.TouristName) && (tourist.TouristPhnNo == vvddvm.TouristPhnNo) && (tourist.CountryId == vvddvm.CountryId) && (tourist.CityId == vvddvm.CityId))
                    {

                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        tourist.TouristName = vvddvm.TouristName;
                        tourist.TouristPhnNo = vvddvm.TouristPhnNo;
                        tourist.CountryId = vvddvm.CountryId;
                        tourist.CityId =Convert.ToInt32(vvddvm.CityId);
                     

                        if (entity.SaveChanges() > 0)
                        {
                            return RedirectToAction("UserPanel", "Home");
                        }
                        else
                        {
                            ViewData["message"] = "There's a problem occured, please try again later.";
                            return View(vvddvm);
                        }
                    }
                }
                catch (Exception ex)
                {

                    return View("Error", new HandleErrorInfo(ex, "Tourist", "TouristDitail"));
                }
            }
            else
            {
                return View(vvddvm);
            }

        }
        #endregion



        #region add Profile picture
        [HttpPost]
        public ActionResult AddProfilePic(int touristId, int albumTypeId, HttpPostedFileBase AddPicture)
        {
            try
            {
                if (Session["userCode"] != null)
                {


                bool result = tl.CheckTouristIdnAlbumTypeIdInPictureAlbum(touristId, albumTypeId);

                Tourist tour = (from c in entity.Tourists where c.TouristId == touristId select c).FirstOrDefault();
                TouristPicture tp = new TouristPicture();
                if (result == true)
                {
                    var albumId = (from c in entity.TouristAlbums where c.TouristId == touristId && c.AlbumTypeId == albumTypeId select c.TouristAlbumId).Single();
                    int AlbumId = Convert.ToInt32(albumId);
                    if (AddPicture != null)
                    {

                        tp.TouristPictureData = new byte[AddPicture.ContentLength];
                        AddPicture.InputStream.Read(tp.TouristPictureData, 0, AddPicture.ContentLength);



                    }
                    else
                    {
                        Session["message"] = "no picture has been selected";
                        return RedirectToAction("UserPanel", "Home");
                    }
                    tp.TouristId = touristId;
                    tp.TouristAlbumId = AlbumId;
                    tp.AlbumTypeId = albumTypeId;
                    tp.TouristPictureAdminPermit = true;
                    tp.CountryId = tour.CountryId;
                    tp.CityId = tour.CityId;
     
                    tp.TouristPicturePrivarcy = true;
                    tp.TouristPictureDateOfAdded = DateTime.Now;
                    entity.TouristPictures.Add(tp);

                    if (entity.SaveChanges() > 0)
                    {
                        var pictureId = (from c in entity.TouristPictures
                                         where c.TouristId == touristId && c.AlbumTypeId == albumTypeId
                                         orderby c.TouristPictureId descending
                                         select c).First();
                        tour.TouristProfilePicId = Convert.ToInt32(pictureId);
                        entity.SaveChanges();

                            var d = (from c in entity.Tourists where c.TouristId == touristId select c).FirstOrDefault();
                            Nullable<int> ProPicId = d.TouristProfilePicId;
                            if (ProPicId != null)
                            {
                                byte[] pic = (from c in entity.TouristPictures where c.TouristId == touristId && c.AlbumTypeId == 1 orderby c.TouristPictureId descending select c.TouristPictureData).First();
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

                        tp.TouristPictureData = new byte[AddPicture.ContentLength];
                        AddPicture.InputStream.Read(tp.TouristPictureData, 0, AddPicture.ContentLength);

                    }
                    else
                    {
                        Session["message"] = "no picture has been selected";
                        return RedirectToAction("UserPanel", "Home");
                    }

                    TouristAlbum ta = new TouristAlbum();
                    ta.TouristAlbumName = "Profile Picture";
                    ta.TouristId = touristId;
                    ta.AlbumTypeId = albumTypeId;
                    ta.CityId = tour.CityId;
                    ta.CountryId = tour.CountryId;
                   
                    ta.TouristAlbumAdminPermit = true;
                    ta.TouristAlbumPrivacy = true;
                    ta.TouristAlbumDateOfCreated = DateTime.Now;
                    entity.TouristAlbums.Add(ta);
                    entity.SaveChanges();
                    tp.TouristId = touristId;
                    var albumId = (from c in entity.TouristAlbums where c.TouristId == touristId && c.AlbumTypeId == albumTypeId select c.TouristAlbumId).Single();
                    int AlbumId = Convert.ToInt32(albumId);
                    tp.TouristAlbumId = AlbumId;
                    tp.AlbumTypeId = albumTypeId;
                    tp.TouristPictureAdminPermit = true;
                    tp.CountryId = tour.CountryId;
                    tp.CityId = tour.CityId;
                  
                    tp.TouristPicturePrivarcy = true;
                    tp.TouristPictureDateOfAdded = DateTime.Now;
                    entity.TouristPictures.Add(tp);
                    if (entity.SaveChanges() > 0)
                    {
                        int pictureId = (from c in entity.TouristPictures
                                         where c.TouristId == touristId && c.AlbumTypeId == albumTypeId
                                         orderby c.TouristPictureId descending
                                         select c.TouristPictureId).First();
                        tour.TouristProfilePicId = pictureId;
                        entity.SaveChanges();

                            var d = (from c in entity.Tourists where c.TouristId == touristId select c).FirstOrDefault();
                            Nullable<int> ProPicId = d.TouristProfilePicId;
                            if (ProPicId != null)
                            {
                                byte[] pic = (from c in entity.TouristPictures where c.TouristId == touristId && c.AlbumTypeId == 1 orderby c.TouristPictureId descending select c.TouristPictureData).First();
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
                return View("Error", new HandleErrorInfo(ex, "Tourist", "AddProfilePic"));
            }
        }

        #endregion


        #region DeactivateAccountByTourist

        public ActionResult DeactivateAccountByTourist()
        {
            try
            {
                if (Session["userCode"] != null)
                {
                    int TouristId = (int)Session["TouristId"];
                    Tourist tour = (from c in entity.Tourists where c.TouristId == TouristId select c).FirstOrDefault();
                    tour.TouristsStatus = false;
                    if (tour.TouristsHostStatus == true)
                    {
                        var offer = (from c in entity.Hosts where c.TouristId == TouristId && c.HostingStatus == true select c).ToList();
                        foreach (var i in offer)
                        {
                            i.HostingStatus = false;
                        }
                    }
                    entity.SaveChanges();
                   
                    return RedirectToAction("LogOut", "Home");
                }
                else
                {
                    Session["message"] = "please log in to complete your task.";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Tourist", "DeactivateAccountByTourist"));
            }
        }

        #endregion

    }
}