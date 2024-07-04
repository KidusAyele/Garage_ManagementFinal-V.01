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
    public class purchasesController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: purchases
        public ActionResult Index()
        {
            var purchases = db.purchases.Include(p => p.Stock);
            return View(purchases.ToList());
        }

        // GET: purchases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: purchases/Create
        public ActionResult Create()
        {
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name");
            return View();
        }

        // POST: purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,quantity,price,total_price,date,StockId")] purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name", purchase.StockId);
            return View(purchase);
        }

        // GET: purchases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name", purchase.StockId);
            return View(purchase);
        }

        // POST: purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,quantity,price,total_price,date,StockId")] purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name", purchase.StockId);
            return View(purchase);
        }

        // GET: purchases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            purchase purchase = db.purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            purchase purchase = db.purchases.Find(id);
            db.purchases.Remove(purchase);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "StockManager")]
        public ActionResult Searchbydate()
        {
            return View("Searchbydate");
        }

        [Authorize(Roles = "StockManager")]
        public ActionResult SearchFordate(DateTime searchPhrase, DateTime searchPhrase2)
        {
            string date = Convert.ToString(searchPhrase2);
            var result = db.purchases.Where(x => x.date >= searchPhrase && x.date <= searchPhrase2).ToList();
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
