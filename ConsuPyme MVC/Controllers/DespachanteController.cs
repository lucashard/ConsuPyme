using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;

namespace ConsuPyme_MVC.Controllers
{
    #region New region

    public class DespachoController : Controller
    {
        private static List<String> selectList = new List<String>();
        private IDespachantes Despachante;
        public DespachoController(IDespachantes despachante)
        {

            Despachante = despachante;
        }
        public ActionResult Index()
        {
            return View(Despachante.grid());
        }

        [HttpGet]
        public ActionResult Create()
        {
            selectList.Clear();
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
            ViewBag.Despachos = this.Despachante.Despachos1(null);
            var lista = new List<NombreDespacho>() { new NombreDespacho() { Id = System.Configuration.ConfigurationManager.AppSettings["Despachante"], Nombre = System.Configuration.ConfigurationManager.AppSettings["Despachante"] } };
            ViewBag.Proveedor = new SelectList(lista, "Id", "Nombre", "SELECCIONE");
            return View();
        }

        [HttpPost]
        public ActionResult Select(bool isChecked, String id)
        {
            selectList.Clear();
            if (isChecked && !selectList.Contains(id))
            {
                selectList.Add(id);
            }
            else if (!isChecked && selectList.Contains(id))
            {
                selectList.RemoveAll(s => s == id);
            }
            HttpContext.Session["SelectList"] = selectList;

            return Content("OK");
        }
        //
        // POST: /Despacho/Create

        [HttpGet]
        public ActionResult Busqueda(string bus)
        {
            //string bus = Request.Form["Busqueda"];
            ViewBag.Despachos = Despachante.Despachos1(bus);//.ObtenerGrilla(bus);
            return PartialView("Grilla");
        }

        [HttpPost]
        public ActionResult Create(Despachantes despachante)
        {
            if (selectList.Count == 0)
            {
                ModelState.AddModelError("Productos", "Por favor seleccione lo/s producto de la factura");
            }
            if (ModelState.IsValid)
            {
                foreach (string element in selectList)
                {

                    despachante.Despacho_Id = Convert.ToInt32(element);
                    Despachante.add(despachante);

                }
                return RedirectToAction("Index");
            }
            ViewBag.Despachos = this.Despachante.Despachos1(null);
            var lista = new List<NombreDespacho>() { new NombreDespacho() { Id = System.Configuration.ConfigurationManager.AppSettings["Despachante"], Nombre = System.Configuration.ConfigurationManager.AppSettings["Despachante"] } };
            ViewBag.Proveedor = new SelectList(lista, "Id", "Nombre", "SELECCIONE");
            return View(despachante);
        }


        //EDITAR!!!
        [HttpGet]
        [ActionName("BusquedaEdicion")]
        public ActionResult BusquedaEdicion(string bus)
        {
             Despachantes despacho = Despachante.Editar(Id);
            var grilla=Despachante.MarcarGrilla(despacho, this.Despachante.Despachos1(null));
            var despresult = grilla.Where(elem => elem.Dc.Contains(bus)).ToList();
            ViewBag.Despachos = despresult;
            return PartialView("Grilla");
        }

        private static int Id { get; set; }
        //
        // GET: /Despacho/Edit/5
        private List<NombreDespacho> NombreDespacho
        {
            get
            {
               return new List<NombreDespacho>() { new NombreDespacho() { Id = System.Configuration.ConfigurationManager.AppSettings["Despachante"], Nombre = System.Configuration.ConfigurationManager.AppSettings["Despachante"] } };   }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Id = id;
            Despachantes despacho = Despachante.Editar(id);
            ViewBag.Proveedor = new SelectList(NombreDespacho, "Id", "Nombre", despacho.ProveedorId);
            ViewBag.Despachos = this.Despachante.MarcarGrilla(despacho, this.Despachante.Despachos1(null)); 
            ViewBag.Productos = Despachante.Buscar_Id_Productos(despacho);
            despacho.Fecha = Convert.ToDateTime(despacho.Fecha.ToString("dd-MM-yyyy"));
            return View(despacho);
        }

        //
        // POST: /Despacho/Edit/5

        [HttpPost]
        public ActionResult Edit(Despachantes despachante, FormCollection collection)
        {
            try
            {
                Despachante.update(despachante, selectList);

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ViewBag.Proveedor = new SelectList(NombreDespacho, "Id", "Nombre", despachante.ProveedorId);
                ViewBag.Despachos = Despachante.MarcarGrilla(despachante, this.Despachante.Despachos1(null));
                ViewBag.Productos = Despachante.Buscar_Id_Productos(despachante);
                return View(despachante);
            }
        }

        //
        // GET: /Despacho/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            Despachante.borrar(id);
            return RedirectToAction("Index");
        }
    }

    #endregion
}
