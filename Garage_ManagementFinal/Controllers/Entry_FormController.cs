using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Garage_ManagementFinal;

namespace Garage_ManagementFinal.Controllers
{
    [Authorize(Roles = "Mechanic")]
    public class Entry_FormController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: Entry_Form
        public ActionResult Index()
        {
            return View(db.Entry_Form.ToList());
        }

        // GET: Entry_Form/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            if (entry_Form == null)
            {
                return HttpNotFound();
            }
            return View(entry_Form);
        }

        // GET: Entry_Form/Create
        public ActionResult Create()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View();
        }

        // POST: Entry_Form/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,plate_no,Brand,Owner_name,phone,Description,Status,date,region,code,Alpha")] Entry_Form entry_Form)
        {
            if (ModelState.IsValid)
            {
                db.Entry_Form.Add(entry_Form);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Created SuccessFuly!";
                return RedirectToAction("Create");
            }

            return View(entry_Form);
        }

        // GET: Entry_Form/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            if (entry_Form == null)
            {
                return HttpNotFound();
            }
            return View(entry_Form);
        }

        // POST: Entry_Form/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,plate_no,Brand,Owner_name,phone,Description,Status,date,region,code,Alpha")] Entry_Form entry_Form)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entry_Form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("unrepairedCar");
            }
            return View(entry_Form);
        }

        public ActionResult Editt(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            if (entry_Form == null)
            {
                return HttpNotFound();
            }
            return View(entry_Form);
        }

        // POST: Entry_Form/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editt([Bind(Include = "Id,plate_no,Brand,Owner_name,phone,Description,Status,date,region,code,Alpha")] Entry_Form entry_Form)
        {
            if (ModelState.IsValid)
            {
                entry_Form.Status = false;
                db.Entry(entry_Form).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Create", "order_service", new { area = "" });
            }
            return View(entry_Form);
        }





        // GET: Entry_Form/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            if (entry_Form == null)
            {
                return HttpNotFound();
            }
            return View(entry_Form);
        }

        // POST: Entry_Form/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            //  var st = db.Stocks.Find(stockid);
            // if ()
            try
            {
                db.Entry_Form.Remove(entry_Form);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Deleted SuccessFuly!";
                return RedirectToAction("unrepairedCar");
            
            }
            catch
            {
                TempData["SuccessMessage1"] = "You Can't Delete this data!";
                return RedirectToAction("unrepairedCar");

            }
          
        }
        public ActionResult leave(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry_Form entry_Form = db.Entry_Form.Find(id);
            if (entry_Form == null)
            {
                return HttpNotFound();
            }
            return View(entry_Form);
        }

        // POST: Entry_Form/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult leave(int id)
        {
            var st = db.Entry_Form.Find(id);
          
            st.Status = true;
         
            db.SaveChanges();
            TempData["SuccessMessage"] = "Checked Out!";
            return RedirectToAction("unrepairedCar");
        }

        public ActionResult unrepairedCar()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            var viewwithfalse = db.Entry_Form.Where(o => o.Status == false).ToList();
            return View(viewwithfalse);
        }
        public ActionResult repairedCar()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            var viewwithtrue = db.Entry_Form.Where(o => o.Status == true).ToList();
            return View(viewwithtrue);
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
