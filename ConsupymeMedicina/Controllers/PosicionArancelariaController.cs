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
    public class PosicionArancelariaController : Controller
    {
        private ConsuPymeDesarrolloEntities db = new ConsuPymeDesarrolloEntities();

        //
        // GET: /PosicionArancelario/

        public ActionResult Index()
        {
            return View(db.Posicion_Arancelaria.ToList());
        }

        //
        // GET: /PosicionArancelario/Details/5

        public ActionResult Details(int id = 0)
        {
            Posicion_Arancelaria posicion_arancelaria = db.Posicion_Arancelaria.Find(id);
            if (posicion_arancelaria == null)
            {
                return HttpNotFound();
            }
            return View(posicion_arancelaria);
        }

        //
        // GET: /PosicionArancelario/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PosicionArancelario/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Posicion_Arancelaria posicion_arancelaria)
        {
            if (ModelState.IsValid)
            {
                db.Posicion_Arancelaria.Add(posicion_arancelaria);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(posicion_arancelaria);
        }

        //
        // GET: /PosicionArancelario/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Posicion_Arancelaria posicion_arancelaria = db.Posicion_Arancelaria.Find(id);
            if (posicion_arancelaria == null)
            {
                return HttpNotFound();
            }
            return View(posicion_arancelaria);
        }

        //
        // POST: /PosicionArancelario/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Posicion_Arancelaria posicion_arancelaria)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posicion_arancelaria).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(posicion_arancelaria);
        }

        //
        // GET: /PosicionArancelario/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Posicion_Arancelaria posicion_arancelaria = db.Posicion_Arancelaria.Find(id);
            if (posicion_arancelaria == null)
            {
                return HttpNotFound();
            }
            return View(posicion_arancelaria);
        }

        //
        // POST: /PosicionArancelario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Posicion_Arancelaria posicion_arancelaria = db.Posicion_Arancelaria.Find(id);
            db.Posicion_Arancelaria.Remove(posicion_arancelaria);
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