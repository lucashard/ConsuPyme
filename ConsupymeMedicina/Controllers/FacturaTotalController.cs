using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Modelo;

namespace ConsupymeMedicina.Controllers
{
    public class FacturaTotalController : Controller
    {
        private ConsuPymeDesarrolloEntities db = new ConsuPymeDesarrolloEntities();

        //
        // GET: /FacturaTotal/

        public ActionResult Index()
        {
            return View(db.Factura_Total.ToList());
        }

        //
        // GET: /FacturaTotal/Details/5

        public ActionResult Details(int id = 0)
        {
            Factura_Total factura_total = db.Factura_Total.Find(id);
            if (factura_total == null)
            {
                return HttpNotFound();
            }
            return View(factura_total);
        }

        //
        // GET: /FacturaTotal/Create

        public ActionResult Create()
        {
            ViewBag.Producto = db.Producto.ToList();
            return View();
        }

        //
        // POST: /FacturaTotal/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Factura_Total factura_total)
        {
            if (ModelState.IsValid)
            {
                db.Factura_Total.Add(factura_total);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(factura_total);
        }

        //
        // GET: /FacturaTotal/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Factura_Total factura_total = db.Factura_Total.Find(id);
            if (factura_total == null)
            {
                return HttpNotFound();
            }
            return View(factura_total);
        }

        //
        // POST: /FacturaTotal/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Factura_Total factura_total)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura_total).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura_total);
        }

        //
        // GET: /FacturaTotal/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Factura_Total factura_total = db.Factura_Total.Find(id);
            if (factura_total == null)
            {
                return HttpNotFound();
            }
            return View(factura_total);
        }

        //
        // POST: /FacturaTotal/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Factura_Total factura_total = db.Factura_Total.Find(id);
            db.Factura_Total.Remove(factura_total);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}