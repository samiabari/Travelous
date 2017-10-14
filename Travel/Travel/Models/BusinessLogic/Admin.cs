using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Travel.Models.DB;

namespace Travel.Models.BusinessLogic
{
    public class Admin
    {
        TourDBEntities entity = new TourDBEntities();


        public static string AdminName = "samia";
        public static string AdminPassword = "123";
        public static string userCode = "admin";
        public static string AdminEmail = "samiabari5.sb@gmail.com";

        //public static byte[] ImageToByteArray(string imagefilePath)
        //{
        //    System.Drawing.Image image = System.Drawing.Image.FromFile(imagefilePath);
        //    byte[] imageByte = ImageToByteArraybyImageConverter(image);
        //    return imageByte;
        //}

        //private static byte[] ImageToByteArraybyImageConverter(System.Drawing.Image image)
        //{
        //    ImageConverter imageConverter = new ImageConverter();
        //    byte[] imageByte = (byte[])imageConverter.ConvertTo(image, typeof(byte[]));
        //    return imageByte;
        //}



        #region Admin SignIn
        public bool SignInAdmin(string Name, string Password)
        {
            bool result;
            if (Name == AdminName && Password == AdminPassword)
            {
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




        #region mail

        public void SentMail(string Mail, string Message, string Subject)
        {
            string FromAddrss = "samia@codexen.net";

            string body = Message;
            string pass = "Samia#Admin1";
            using (MailMessage mm = new MailMessage(FromAddrss, Mail))
            {
                mm.Subject = Subject;
                mm.Body = body;

                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "mail.codexen.net";
                    smtp.EnableSsl = false;
                    NetworkCredential NetworkCred = new NetworkCredential(FromAddrss, pass);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 26;
                    smtp.Send(mm);

                }

            }

        }

        #endregion

        #region Check subcribed email
        public bool Check(string mail)
        {
            bool result;
            result = (from c in entity.Subscribers where c.SubscriberEmail == mail select c).Any();
            return result;
        }
        #endregion

    }
}