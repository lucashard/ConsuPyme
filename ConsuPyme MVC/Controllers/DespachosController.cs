using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;

namespace ConsuPyme_MVC.Controllers
{
    public class DespachosController : Controller
    {
        //
        // GET: /Despachos/

        private static readonly List<String> selectList = new List<String>();
        private readonly IDespachos Despachos;

        public DespachosController(IDespachos despachos)
        {
            Despachos = despachos;
        }

        public ActionResult Index()
        {
            return View(Despachos.grid());
        }

        //
        // GET: /Despachos/Create
        [HttpGet]
        public ActionResult Create()
        {
            selectList.Clear();
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
            ViewBag.Facturas = Despachos.Facturas(null);
            ViewBag.TipoCambio = new SelectList(Despachos.buscarTipoCambio(), "Id", "Cambio", "SELECCIONE");
            return View();
        }

        [HttpGet]
        [ActionName("Busqueda")]
        public ActionResult Busqueda(string bus)
        {
            var busqueda = Despachos.Facturas(bus);
            
            foreach (var elemento in selectList)
            {
                var id1 = Convert.ToInt32(elemento);
                foreach (var elementos in busqueda)
                {
                    if (elementos.Id == id1)
                    {
                        elementos.Visible = true;
                    }
                }
            }
            ViewBag.Facturas = busqueda;
            return PartialView("Grilla");
        }

        [HttpGet]
        [ActionName("BusquedaEdicion")]
        public ActionResult BusquedaEdicion(string bus)
        {
            List<Facturas> despa = Despachos.verFacturas(Id);
            List<Facturas> despacho= despa.Where(elem => elem.FacturasTotales.Numero_Factura.Contains(bus)).ToList();
            ViewBag.Facturas = despacho.Count==0?despa:despacho;
            return PartialView("GrillaEdicion");
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

        public JsonResult Productos(IEnumerable<string> vector)
        {
            List<int> listaId =
                vector.Select(element => Convert.ToInt32(element.ToString().TrimStart('[').TrimEnd(']'))).ToList();
            List<Productos> lista = Despachos.Productos(listaId).ToList();
            return Json(new {Lista = lista});
        }

        //
        // POST: /Despachos/Create

        [HttpGet]
        public ActionResult Create1(string bus)
        {
            ViewBag.Facturas = Despachos.Facturas(bus);
            return PartialView("Grilla");
        }

        [HttpPost]
        public ActionResult Create(Despachos ft)
        {
            //ModelState[""].Errors = "";
            foreach (var key in ModelState.Keys.ToList().Where(key => ModelState.ContainsKey("Visible")))
            {
                //ModelState.Remove(key); //This was my solution before
                ModelState[key].Errors.Clear(); //This is my new solution. Thanks bbak
            }
            if (selectList.Count == 0)
            {
                ModelState.AddModelError("Productos", "Por favor seleccione las factura");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (string element in selectList)
                    {
                        ft.Lista_Factura = new List<int>(0) {Convert.ToInt32(element)};
                        Despachos.add(ft);
                    }
                    ViewBag.Facturas = Despachos.Facturas(null);
                    ViewBag.TipoCambio = new SelectList(Despachos.buscarTipoCambio(), "Id", "Cambio", "SELECCIONE");
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            ViewBag.Facturas = Despachos.Facturas(null);
            ViewBag.TipoCambio = new SelectList(Despachos.buscarTipoCambio(), "Id", "Cambio", "SELECCIONE");
            return View(ft);
        }

        private static int Id { get; set; }
        //
        // GET: /Despachos/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {
            selectList.Clear();
            Despachos despacho = Despachos.Editar(id);
            Id = id;
            List<Facturas> despa = Despachos.verFacturas(id);
            ViewBag.Facturas = despa;
            ViewBag.TipoCambio = new SelectList(Despachos.buscarTipoCambio(), "Id", "Cambio",despacho.TipoCambioId.ToString());

            ViewBag.ValorPorcentaje ="El valor del porcentaje es: "+ Convert.ToDecimal((despacho.Fob_Total*despacho.Gasto_Aduanero)/100).ToString("##.###");

            foreach (Facturas elemento in despa)
            {
                if (elemento.Visible) selectList.Add(elemento.Id.ToString());
            }

            return View(despacho);
        }

        //
        // POST: /Despachos/Edit/5

        [HttpPost]
        public ActionResult Edit(Despachos despa, FormCollection collection)
        {
            try
            {
                if (!Request.IsAjaxRequest())
                {
                    Despachos.update(despa, selectList);
                    
                    return RedirectToAction("Index");
                }
                else
                {
                    return PartialView("GrillaEdicion");
                }
            }
            catch
            {
                ViewBag.TipoCambio = new SelectList(Despachos.buscarTipoCambio(), "Id", "Cambio", "SELECCIONE");
                ViewBag.ValorPorcentaje = "El valor del porcentaje es: " + Convert.ToDecimal((despa.Fob_Total * despa.Gasto_Aduanero) / 100).ToString("##.###");
                List<Facturas> despas = Despachos.verFacturas(despa.Id);
                ViewBag.Facturas = despas;
                
                
                return View(despa);
            }


        }


        [HttpGet]
        public ActionResult Delete(string id)
        {
            Despachos.borrar(id);
            return RedirectToAction("Index");
        }
    }
}