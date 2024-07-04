using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Garage_ManagementFinal;

namespace Garage_ManagementFinal.Controllers
{
    public class recieptsController : Controller
    {
        
        private GaragedbEntities db = new GaragedbEntities();

        // GET: reciepts
        [Authorize(Roles = "cashier")]
        public ActionResult Index()
        {
            return View(db.recipts.ToList());
        }
        [Authorize(Roles = "cashier")]
        // GET: reciepts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recipt reciept = db.recipts.Find(id);
            if (reciept == null)
            {
                return HttpNotFound();
            }
            return View(reciept);
        }

        // GET: reciepts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: reciepts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,date,customer_Fullname,plate_no,description,quantity,price,total,vat,tot_withvat")] recipt reciept)
        {
            if (ModelState.IsValid)
            {
                db.recipts.Add(reciept);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reciept);
        }

        // GET: reciepts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recipt reciept = db.recipts.Find(id);
            if (reciept == null)
            {
                return HttpNotFound();
            }
            return View(reciept);
        }

        // POST: reciepts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,date,customer_Fullname,plate_no,description,quantity,price,total,vat,tot_withvat")] recipt reciept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reciept).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reciept);
        }
        [Authorize(Roles = "cashier")]
        // GET: reciepts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            recipt reciept = db.recipts.Find(id);
            if (reciept == null)
            {
                return HttpNotFound();
            }
            return View(reciept);
        }

        // POST: reciepts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            recipt reciept = db.recipts.Find(id);
            db.recipts.Remove(reciept);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "cashier")]
        public ActionResult SearchForm()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            // ViewBag.Plate_no = new SelectList(db.recipts, "Id", "Plate_no");
            return View("SearchForm");
        }
      
        [Authorize(Roles = "cashier")]
        public ActionResult SearchForname(string region, string code, string Alpa, string plate_no)
        {
            string pl_no = code  + "/" + region + "/" +Alpa +"/"+ plate_no;

            var result = from g in db.recipts
                         where  g.status != true && g.Plate_no==pl_no
                         select g;
      
                return View(result);
          
               
       
            
           
        }
        [HttpPost]
       public ActionResult UpdateStatus(int []ids)
        {
            if (ids!=null) {
                using (var context = new GaragedbEntities()) {

                    var items = db.recipts.Where(x => ids.Contains(x.Id));
                    
                    foreach (var item in items)
                    {
               
                        item.status = true;
                        item.date = DateTime.Today;
                    }
                    db.SaveChanges();


                }
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "cashier")]
        public ActionResult Searchbydate()
        {
            return View("Searchbydate");
        }

        [Authorize(Roles = "cashier")]
        public ActionResult SearchFordate(DateTime searchPhrase , DateTime searchPhrase2)
        {
            string date = Convert.ToString(searchPhrase2);
            var result = db.recipts.Where(x=>x.date>=searchPhrase && x.date<= searchPhrase2).ToList();
            return View(result);
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
