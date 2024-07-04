using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage_ManagementFinal.Models;
using Garage_ManagementFinalWithDatabase.Models;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Infrastructure;
using System.Web.Services.Description;

namespace Garage_ManagementFinal.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var context = new GaragedbEntities())
            {
                var getUser = (from s in context.users where s.email == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;

                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 

                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.Firstname + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.email, body, subject);

                   
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("sendale982@gmail.com", emailAddress))
            {
          
                try
                {
                    mm.Subject = subject;
                    mm.Body = body;

                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sendale982@gmail.com", "xmhryivymaxfkvce");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Reset password link has been sent to your email id.";

                }
                catch (Exception)
                {
                    ViewBag.Message = "please check your internet connection.";
                }
            }
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new GaragedbEntities())
            {
                var user = context.users.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword([Bind(Include = "NewPassword,ConfirmPassword,ResetCode")] ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new GaragedbEntities())
                {
                    var user = context.users.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {

                        //you can encrypt password here, we are not doing it
                        string password = model.NewPassword;
                        //make resetpasswordcode empty string now
                    
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;

                        //  context.Entry(user).State = EntityState.Modified;
                        int ids = user.Id;
                        updatePassword(ids,password);

                        ViewBag.Message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }
        public ActionResult updatePassword(int ids, string NewPassword)
        {
            using (var db = new GaragedbEntities())
            {
                var product = db.users.Find(ids);

                product.password = NewPassword;
                product.ResetPasswordCode = "";
                db.SaveChanges();

                return RedirectToAction("login", "Home");
            }

        }
    }
}