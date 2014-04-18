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
    public class DespachoController : Controller
    {
        private ConsuPymeDesarrolloEntities db = new ConsuPymeDesarrolloEntities();

        //
        // GET: /Despacho/

        public ActionResult Index()
        {
            return View(db.Despacho.ToList());
        }

        //
        // GET: /Despacho/Details/5


        //
        // GET: /Despacho/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Despacho/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Despacho despacho)
        {
            if (ModelState.IsValid)
            {
                db.Despacho.Add(despacho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(despacho);
        }

        //
        // GET: /Despacho/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Despacho despacho = db.Despacho.Find(id);
            if (despacho == null)
            {
                return HttpNotFound();
            }
            return View(despacho);
        }

        //
        // POST: /Despacho/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Despacho despacho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(despacho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(despacho);
        }

        //
        // GET: /Despacho/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Despacho despacho = db.Despacho.Find(id);
            if (despacho == null)
            {
                return HttpNotFound();
            }
            return View(despacho);
        }

        //
        // POST: /Despacho/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Despacho despacho = db.Despacho.Find(id);
            db.Despacho.Remove(despacho);
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