using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage_ManagementFinal;

namespace Garage_ManagementFinal.Controllers
{
    public class service_tableController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: service_table
        [Authorize(Roles = "Mechanic")]
        public ActionResult Index()
        {
            return View(db.service_table.ToList());
        }

        // GET: service_table/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_table service_table = db.service_table.Find(id);
            if (service_table == null)
            {
                return HttpNotFound();
            }
            return View(service_table);
        }

        // GET: service_table/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: service_table/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,service_name,price,status,type")] service_table service_table)
        {
            if (ModelState.IsValid)
            {
                db.service_table.Add(service_table);
                db.SaveChanges();
                TempData["SuccessMessage"] = "SuccessFuly Insert";
                return RedirectToAction("View_toAdmin");
            }

            return View(service_table);
        }

        // GET: service_table/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_table service_table = db.service_table.Find(id);
            if (service_table == null)
            {
                return HttpNotFound();
            }
            return View(service_table);
        }

        // POST: service_table/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,service_name,price,status,type")] service_table service_table)
        {
            if (ModelState.IsValid)
            {
                db.Entry(service_table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("View_toAdmin");
            }
            return View(service_table);
        }

        // GET: service_table/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service_table service_table = db.service_table.Find(id);
            if (service_table == null)
            {
                return HttpNotFound();
            }
            return View(service_table);
        }

        // POST: service_table/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            service_table service_table = db.service_table.Find(id);
            db.service_table.Remove(service_table);
            db.SaveChanges();
            return RedirectToAction("View_toAdmin");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult View_toAdmin()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            var stockks = db.service_table.ToList();
            return View(stockks);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
