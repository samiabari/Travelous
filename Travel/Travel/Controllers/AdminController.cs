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
    public class AdminController : Controller
    {
        TourDBEntities entity = new TourDBEntities();

        Admin adminClass = new Admin();

        #region Admin signIn
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(AdminSignInViewModel admin)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    bool result = adminClass.SignInAdmin(admin.AdminName, admin.AdminPassword);
                    if (result == true)
                    {
                        return RedirectToAction("UserPanel", "Home");
                    }
                    else
                    {
                        ViewData["message"] = "Wrong Name or Password";
                        return View(admin);
                    }
                }
                catch (Exception ex)
                {
                    return View("Error", new HandleErrorInfo(ex, "Admin", "SignIn"));
                }
            }
            else
            {

                return View(admin);
            }

        }
        #endregion




    }
}