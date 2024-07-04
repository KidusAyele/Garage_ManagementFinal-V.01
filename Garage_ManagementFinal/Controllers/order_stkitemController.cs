using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Garage_ManagementFinal;

namespace Garage_ManagementFinal.Controllers
{
    public class order_stkitemController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: order_stkitem
        [Authorize(Roles = "Mechanic")]
        public ActionResult Index()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            var order_stkitem = db.order_stkitem.Include(o => o.Stock).Include(o => o.Entry_Form).Where(o=>o.status!=true);
            return View(order_stkitem.ToList());
        }
        [Authorize(Roles = "Mechanic")]
        public ActionResult ViewAccepteditem()
        {
          
            var order_stkitem = db.order_stkitem.Include(o => o.Stock).Include(o => o.Entry_Form).Where(o => o.status == true);
            return View(order_stkitem.ToList());
        }
        [Authorize(Roles = "StockManager")]
        public ActionResult allview()
        {
            var order_stkitem = db.order_stkitem.Include(o => o.Stock).Include(o => o.Entry_Form);
            return View(order_stkitem.ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult View_toAdmin()
        {
            var stockks = db.order_stkitem.Where(o => o.status == true).ToList();
            return View(stockks);
        }
        [Authorize(Roles = "StockManager")]
        public ActionResult View_toStockM()
        {
            var stockks = db.order_stkitem.Where(o => o.status == false).ToList();
            return View(stockks);
        }

        [Authorize(Roles = "StockManager")]
        // GET: order_stkitem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_stkitem order_stkitem = db.order_stkitem.Find(id);
            if (order_stkitem == null)
            {
                return HttpNotFound();
            }
            return View(order_stkitem);
        }
        [Authorize(Roles = "Mechanic")]
        // GET: order_stkitem/Details/5
        public ActionResult Detail1(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_stkitem order_stkitem = db.order_stkitem.Find(id);
            if (order_stkitem == null)
            {
                return HttpNotFound();
            }
            return View(order_stkitem);
        }

        [Authorize(Roles = "Mechanic")]
        // GET: order_stkitem/Create
        public ActionResult Create()
        {
            ViewBag.SuccessMessage = TempData["ErrorMessage"];
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name","quantity");
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form.Where(x => x.Status != true), "Id", "plate_no");
            return View();
        }
      
        // POST: order_stkitem/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Mechanic")]
        public ActionResult Create([Bind(Include = "Id,quantity,status,Entry_FormId,StockId,date")] order_stkitem order_stkitem)
        {
            int id = order_stkitem.StockId;
         
            if (ModelState.IsValid)
            {
                Stock quantity = db.Stocks.Find(id);

                if (quantity.quantity >= order_stkitem.quantity)
                {
                    order_stkitem.Orderedby=(User.Identity.IsAuthenticated ?User.Identity.Name : "Guest");
                    db.order_stkitem.Add(order_stkitem);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Successfuly send request";
                    return RedirectToAction("Create");
                }
                else
                {
                    TempData["ErrorMessage"] = "out of quantity";
                    return RedirectToAction("Create");
                }

            }

            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name","quantity", order_stkitem.StockId);
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form, "Id", "plate_no", order_stkitem.Entry_FormId);
            return View(order_stkitem);
        }
        [Authorize(Roles = "Mechanic")]
        // GET: order_stkitem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_stkitem order_stkitem = db.order_stkitem.Find(id);
            if (order_stkitem == null)
            {
                return HttpNotFound();
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name", order_stkitem.StockId);
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form, "Id", "plate_no", order_stkitem.Entry_FormId);
            return View(order_stkitem);
        }

        // POST: order_stkitem/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Mechanic")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,quantity,price,status,Entry_FormId,StockId,date")] order_stkitem order_stkitem)
        {
            if (ModelState.IsValid)
            {
               
                db.Entry(order_stkitem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            ViewBag.StockId = new SelectList(db.Stocks, "Id", "name", order_stkitem.StockId);
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form, "Id", "plate_no", order_stkitem.Entry_FormId);
            return View(order_stkitem);
        }
        [Authorize(Roles = "Mechanic")]
        // GET: order_stkitem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_stkitem order_stkitem = db.order_stkitem.Find(id);
            if (order_stkitem == null)
            {
                return HttpNotFound();
            }
            return View(order_stkitem);
        }

        // POST: order_stkitem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order_stkitem order_stkitem = db.order_stkitem.Find(id);
            db.order_stkitem.Remove(order_stkitem);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Successfuly Delete";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "StockManager")]
        public async Task<ActionResult> Acceptorder(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_stkitem order_Stkitem = await db.order_stkitem.FindAsync(id);
            if (order_Stkitem == null)
            {
                return HttpNotFound();
            }
            else
            {

                return View(order_Stkitem);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Acceptorder([Bind(Include = "Id,quantity,price,status,Entry_FormId,StockId")] order_stkitem order_Stkitem, int id)
        {

            using (var context = new GaragedbEntities())
            {

                var product = context.order_stkitem.Find(id);
                int stockid = product.StockId;
                var st = db.Stocks.Find(stockid);
                var st1 = db.Entry_Form.Find(product.Entry_FormId);
                double m = Convert.ToDouble(Request["quantity"]);
                if (product != null && product.status != true && st.quantity >= m && st1.Status==false)
                {
                    product.status = true;

                    context.SaveChanges();

                    await updatestock(id);

                    /* Savetodatabase(quantity,price,Total_price,DateTime.Now,ids);*/


                    return RedirectToAction("View_toStockM");

                }

            }
            return RedirectToAction("View_toStockM");
        }

        public async Task<ActionResult> updatestock(int id)
        {
            using (var db = new GaragedbEntities())
            {
                var order = db.order_stkitem.Find(id);
                int stockid = order.StockId;
                var product = db.Stocks.Find(stockid);
                if (order != null)
                {

                    if (product.quantity >= order.quantity)
                    {
                        product.quantity -= order.quantity;

                        order.status = true;
                        db.order_stkitem.AddOrUpdate(order);
                        db.SaveChanges();
                        String name = Convert.ToString(order.Entry_Form.Owner_name);
                        String plate = order.Entry_Form.code + "/" + order.Entry_Form.region +"/" +order.Entry_Form.Alpha+"/" + order.Entry_Form.plate_no ;
                        string des = order.Stock.name;
                        double m = order.quantity;
                        double p = order.Stock.price;
                        int entry = order.Entry_FormId;
                        double tot = m * p;
                        double vat = 0.15 * tot;
                        double sub = tot + vat;
                        string alp = order.Entry_Form.Alpha;
                        DateTime currentDate = DateTime.Now;
                        recipt receiptt = new recipt();
                        if (ModelState.IsValid)
                        {
                            receiptt.date = DateTime.Now;
                            receiptt.customer_Fullname = name;
                            receiptt.Plate_no = plate;
                            receiptt.description = des;
                            receiptt.quantity = m;
                            receiptt.price = p;
                            receiptt.total = tot;
                            receiptt.vat = vat;
                            receiptt.tot_withvat = sub;
                            receiptt.status = false;
                            receiptt.Alpha = alp;
                            receiptt.EntryFormId = entry;
                            db.recipts.Add(receiptt);
                            await db.SaveChangesAsync();
                            TempData["SuccessMessage"] = "Accepted!";
                            return RedirectToAction("View_toStockM");
                        }
                        else
                        {
                            return View();
                        }

                    }
                }



            }
            return RedirectToAction("View_toStockM");

        }


        public static class PageUtility
        {
            public static void MessageBox(System.Web.UI.Page page, string strmsg)
            {
                ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertmessage", "alert('" + strmsg + "')", true);
            }
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
            var result = db.order_stkitem.Where(x => x.date >= searchPhrase && x.date <= searchPhrase2 && x.status==true).ToList();
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
