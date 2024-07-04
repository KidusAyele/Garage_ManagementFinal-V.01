using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Garage_ManagementFinal;

namespace Garage_ManagementFinal.Controllers
{
    public class StocksController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: Stocks

        [Authorize(Roles = "StockManager")]
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(db.Stocks.ToList());
        }
        [Authorize(Roles = "Admin")]
        public ActionResult ViewAdmin()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            return View(db.Stocks.ToList());
        }
        // GET: Stocks/Details/5
        [Authorize(Roles = "StockManager")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        [Authorize(Roles = "Admin")]
        // GET: Stocks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Stocks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,name,quantity,price,type")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                stock.buy_price = 0;
                stock.price = 0;
                stock.quantity = 0;
                db.Stocks.Add(stock);
                db.SaveChanges();
                TempData["SuccessMessage"] = "SuccessFuly Create";
                return RedirectToAction("Create");
            }

            return View(stock);
        }
        [Authorize(Roles = "Admin")]
        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name,quantity,price,type")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stock);
        }
        [Authorize(Roles = "StockManager")]
        // GET: Stocks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stock stock = db.Stocks.Find(id);
            try
            {
                db.Stocks.Remove(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["SuccessMessage"] = "Please delete First from orders!";
                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "StockManager")]
        public async Task<ActionResult> Viewadd(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = await db.Stocks.FindAsync(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Viewadd([Bind(Include = "Id,name,quantity,price,type,buy_price")] Stock stock, int id)
        {

            using (var context = new GaragedbEntities())
            {

                var product = context.Stocks.Find(id);
                if (product != null)
                {
                    decimal m = Convert.ToDecimal(Request["quantity"]);
                    decimal l = System.Convert.ToDecimal(product.quantity);
                    decimal sum = m + l;
                    product.quantity = System.Convert.ToDouble(sum);
                    product.buy_price = Convert.ToDouble(Request["buy_price"]);
                    product.price = Convert.ToDouble(Request["price"]);
                    context.SaveChanges();
                    decimal buyprice = Convert.ToDecimal(Request["buy_price"]);
                    decimal f= Convert.ToDecimal(Request["quantity"]);
                    decimal Total_price = f * buyprice;
                    int ids = stock.Id;
                    DateTime currentDate = DateTime.Now;
                  
                        purchase purchase = new purchase();
                        purchase.old_quantity = System.Convert.ToDouble(l);
                        purchase.new_quantity = System.Convert.ToDouble(m);
                        purchase.remain_quantity = System.Convert.ToDouble(sum);
                        purchase.price = System.Convert.ToDouble(buyprice);
                        purchase.total_price = System.Convert.ToDouble(Total_price);
                        purchase.date = DateTime.Now;
                        purchase.StockId = ids;
                        db.purchases.Add(purchase);
                        await db.SaveChangesAsync();
                        TempData["SuccessMessage"] = "SuccessFuly Purchase!";
                        return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
           
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
