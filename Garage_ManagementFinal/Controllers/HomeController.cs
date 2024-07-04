using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.ModelBinding;
using System.Web.Mvc;
using System.Web.Security;
using Garage_ManagementFinal.Models;
using Garage_ManagementFinalWithDatabase.Models;
using Microsoft.AspNet.Identity;

namespace Garage_ManagementFinal.Controllers
{
    public class HomeController : Controller
    {
        GaragedbEntities db = new GaragedbEntities();
        int V;

        [Authorize(Roles = "Stockmanager")]
        public ActionResult Stockmanager()
        {
            return RedirectToAction("View_toStockM", "order_stkitem", new { area = "" });

            //  string str = string.empty;
            //str = convert.tostring(session["role"])  
        }

        public ActionResult users()
        {
            return View();
        }

        [Authorize(Roles = "cashier")]
        public ActionResult cashier()
        {

            return RedirectToAction("Index", "reciepts", new { area = "" });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Dashboard2()
        {

            return RedirectToAction("Index", "users", new { area = "" });
        }

        [Authorize(Roles = "Mechanic")]
        public ActionResult Mechanic()
        {
            return RedirectToAction("unrepairedCar", "Entry_Form", new { area = "" });
        }





        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }








        public ActionResult login()
        {

            return View();


        }
        [HttpPost]
        public ActionResult login(user model, string returnurl)
        {
            GaragedbEntities db = new GaragedbEntities();

            try
            {
                var dataItem = db.users.Where(x => x.username == model.username && x.password == model.password).First();
                if (dataItem != null && dataItem.status == true)
                {
                    FormsAuthentication.SetAuthCookie(dataItem.username, false);
                    if (Url.IsLocalUrl(returnurl) && returnurl.Length > 1 && returnurl.StartsWith("/")
                        && !returnurl.StartsWith("//") && !returnurl.StartsWith("/\\"))
                    {
                        return Redirect(returnurl);
                    }
                    else
                    {
                        Session["Id"] = dataItem.Id;
                        V = Convert.ToInt32(Session["Id"]);
                        if (dataItem.role == "Admin")
                        {
                            return RedirectToAction("Dashboard2");

                        }
                        else if (dataItem.role == "Mechanic")

                        {
                            return RedirectToAction("Mechanic");

                        }
                        else if (dataItem.role == "StockManager")
                        {
                            return RedirectToAction("Stockmanager");
                        }

                        else if (model.role == "Cashier")
                        {
                            return RedirectToAction("cashier");
                        }
                        else
                        {
                            return RedirectToAction("cashier");
                        }

                    }

                }
                else
                {
                    ViewBag.ErrorMessage = "Wrong username or password";
                    return View();
                }
            }
            catch (InvalidOperationException ex)
            {
                ViewBag.ErrorMessage = "please enter username or password";
                return View();
            }


        }
          
        
        [Authorize]
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("login", "Home");
        }



        public ActionResult UserProfile()
        {
            GaragedbEntities db = new GaragedbEntities();
            string userName = Session["Id"].ToString();
            int id = Convert.ToInt16(userName);
   
            var stockks = db.users.Where(o => o.Id == id).ToList();

            if (userName == null)
            {
                return RedirectToAction("Login", "Home");
            }

            return View(stockks);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Firstname,Lastname,email,username,password,gender,phone_no,role,status,ResetPasswordCode")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return View(user);
            }
            return View(user);
        }


    }
}