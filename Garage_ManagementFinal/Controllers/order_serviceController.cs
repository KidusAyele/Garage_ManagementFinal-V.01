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
    public class order_serviceController : Controller
    {
        private GaragedbEntities db = new GaragedbEntities();

        // GET: order_service
        public ActionResult Index()
        {
            var order_service = db.order_service.Include(o => o.Entry_Form).Include(o => o.service_table).Where(o => o.status == true).ToList();
            return View(order_service.ToList());
            

        }

        // GET: order_service/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_service order_service = db.order_service.Find(id);
            if (order_service == null)
            {
                return HttpNotFound();
            }
            return View(order_service);
        }

        // GET: order_service/Create
        public ActionResult Create()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage1"];
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form.Where(x =>x.Status==false), "Id", "plate_no");
            ViewBag.service_tableId = new SelectList(db.service_table.Where(x => x.status == true), "Id", "service_name");
            return View();
        }

        // POST: order_service/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,status,service_tableId,Entry_FormId,quantity,Assistant_Mechanic,date")] order_service order_service)
        {
            if (ModelState.IsValid)
            {
                using (var context = new GaragedbEntities())
                {

                    var servicee = context.service_table.Find(order_service.service_tableId);
                    if (servicee.type == true)
                    {
                        db.order_service.Add(order_service);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Successfuly Create";
                        return RedirectToAction("Create");
                    }
                    else
                    {
                        order_service.quantity = 1;
                        db.order_service.Add(order_service);
                        db.SaveChanges();
                        TempData["SuccessMessage"] = "Successfuly Create";
                        return RedirectToAction("Create");
                    }
                }
            }

            ViewBag.Entry_FormId = new SelectList(db.Entry_Form, "Id", "plate_no", order_service.Entry_FormId);
            ViewBag.service_tableId = new SelectList(db.service_table, "Id", "service_name", order_service.service_tableId);
            return View(order_service);
        }

        // GET: order_service/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_service order_service = db.order_service.Find(id);
            if (order_service == null)
            {
                return HttpNotFound();
            }
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form.Where(x => x.Status != true), "Id", "plate_no", order_service.Entry_FormId);
            ViewBag.service_tableId = new SelectList(db.service_table.Where(x => x.status != true), "Id", "service_name", order_service.service_tableId);
            return View(order_service);
        }

        // POST: order_service/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,status,service_tableId,Entry_FormId,quantity,Assistant_Mechanic")] order_service order_service)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order_service).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Successfuly Edited";
                return RedirectToAction("View_Mechanic");
            }
            ViewBag.Entry_FormId = new SelectList(db.Entry_Form.Where(x => x.Status != true), "Id", "plate_no", order_service.Entry_FormId);
            ViewBag.service_tableId = new SelectList(db.service_table.Where(x => x.status != true), "Id", "service_name", order_service.service_tableId);
            return View(order_service);
        }

        // GET: order_service/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_service order_service = db.order_service.Find(id);
            if (order_service == null)
            {
                return HttpNotFound();
            }
            return View(order_service);
        }

        // POST: order_service/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            order_service order_service = db.order_service.Find(id);
            db.order_service.Remove(order_service);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Deleted SuccessFuly!";
            return RedirectToAction("View_Mechanic");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        [Authorize(Roles = "Mechanic")]
        public ActionResult View_Mechanic()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.SuccessMessage = TempData["SuccessMessage1"];
            var services = db.order_service.Where(o => o.status == false).ToList();
            return View(services);
        }
        [Authorize(Roles = "Mechanic")]
        public async Task<ActionResult> Acceptorderservice(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order_service order_service = await db.order_service.FindAsync(id);
            if (order_service == null)
            {
                return HttpNotFound();
            }
            else
            {

                return View(order_service);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Acceptorderservice([Bind(Include = "Id,status,service_tableId,Entry_FormId,quantity")] order_service order_service, int id)
        {

            using (var context = new GaragedbEntities())
            {

                var servicee = context.order_service.Find(id);
                int sid = servicee.service_tableId;
                var st = db.service_table.Find(sid);
                var st2 = db.Entry_Form.Find(servicee.Entry_FormId);
                //  double m = Convert.ToDouble(Request["quantity"]);
                if (servicee != null && servicee.status != true && st2.Status!=true)
                {
                    servicee.status = true;

                    context.SaveChanges();

                    await updateservice(id);

                    /* Savetodatabase(quantity,price,Total_price,DateTime.Now,ids);*/

                    TempData["SuccessMessage"] = "Work Done!";
                    return RedirectToAction("View_Mechanic");

                }
                else
                {
                    TempData["SuccessMessage2"] = "the car is out!";
                    return RedirectToAction("View_Mechanic");

                }

            }
            return RedirectToAction("View_Mechanic");
        }

        public async Task<ActionResult> updateservice(int id)
        {
            using (var db = new GaragedbEntities())
            {
                var order = db.order_service.Find(id);
                int sid = order.service_tableId;
                var product = db.Stocks.Find(sid);
                if (order != null)
                {
                    String name = Convert.ToString(order.Entry_Form.Owner_name);
                    String plate = order.Entry_Form.code + "/" + order.Entry_Form.region + "/" + order.Entry_Form.Alpha + "/" + order.Entry_Form.plate_no;
                
                    string des = order.service_table.service_name;
                    double m = Convert.ToDouble(order.quantity);
                    double p = order.service_table.price;
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
                        receiptt.EntryFormId = order.Entry_FormId;
                        db.recipts.Add(receiptt);
                        await db.SaveChangesAsync();
                        return RedirectToAction("View_Mechanic", "receipts");
                    }
                    else
                    {
                        return View();
                    }

                }
            }


            return RedirectToAction("Index");
        }




    }
}
