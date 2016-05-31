using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;
using Ninject;

namespace ConsuPyme_MVC.Controllers
{
    public class DepositoController : Controller
    {
        private static List<String> selectList = new List<String>();
        //
        // GET: /Deposito/

        private IDeposito _Deposito;

        public DepositoController(IDeposito o)
        {
            _Deposito = o;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.productos = _Deposito.Index();
            return View();
        }

        [HttpGet]
        public ActionResult Busqueda(string bus)
        {
            var prod=_Deposito.Despachos(bus);
            if (selectList.Any())
            {
                var id = Convert.ToInt32(selectList[0]);
                if (prod.Where(x => x.Id == id).ToList().Any())
                {
                    prod.Single(x => x.Id == id).Visible = true;
                }
            }

            ViewBag.Productos = prod;
            return PartialView("Grilla");
        }

        [HttpGet]
        public ActionResult Busqueda1(string bus)
        {
            var prod = _Deposito.Despachos(bus);
            if (selectList.Any())
            {
                var id = Convert.ToInt32(selectList[0]);
                if (prod.Where(x => x.Id == id).ToList().Any())
                {
                    prod.Single(x => x.Id == id).Visible = true;
                }
            }

            ViewBag.Productos = prod;
            return PartialView("Grilla");
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
        // GET: /Deposito/Create

        public ActionResult Create()
        {
            selectList.Clear();
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
            var lista = new List<NombreDeposito>()
            {
                new NombreDeposito()
                {
                    Id = System.Configuration.ConfigurationManager.AppSettings["Deposito"],
                    Nombre = System.Configuration.ConfigurationManager.AppSettings["Deposito"]
                }
            };
            ViewBag.NombreProveedor = new SelectList(lista, "Id", "Nombre", "SELECCIONE");
            ViewBag.Productos = _Deposito.Despachos(null);
            return View();
        }

        //
        // POST: /Deposito/Create

        [HttpPost]
        public ActionResult Create(Depositos deposito)
        {
            if (selectList.Count == 0)
            {
                ModelState.AddModelError("Productos", "Por favor seleccione el Despacho");
            }
            if (ModelState.IsValid)
            {
                foreach (string element in selectList)
                {
                    deposito.Despacho_Id = Convert.ToInt32(element);
                    _Deposito.add(deposito);
                }
                return RedirectToAction("Index");
            }
            var lista = new List<NombreDeposito>()
            {
                new NombreDeposito()
                {
                    Id = System.Configuration.ConfigurationManager.AppSettings["Deposito"],
                    Nombre = System.Configuration.ConfigurationManager.AppSettings["Deposito"]
                }
            };
            ViewBag.NombreProveedor = new SelectList(lista, "Id", "Nombre", "SELECCIONE");
            ViewBag.Productos = _Deposito.Despachos(null);
            return View(deposito);
        }

        ////
        //// GET: /Deposito/Edit/5

        public ActionResult Edit(int id)
        {
            var acarreos = _Deposito.Editar(id);
            var depocito = _Deposito.Despachos(null);
            var id1 = acarreos.Despacho_Id;
            depocito.Where(c => c.Id == id1).FirstOrDefault().Visible = true;
            var lista = new List<NombreAcarreo>()
            {
                new NombreAcarreo()
                {
                    Id = System.Configuration.ConfigurationManager.AppSettings["Deposito"],
                    Nombre = System.Configuration.ConfigurationManager.AppSettings["Deposito"]
                }
            };
            acarreos.ProveedorId = acarreos.Nombre;
            ViewBag.Proveedor = new SelectList(lista, "Id", "Nombre", acarreos.ProveedorId);
            ViewBag.Productos = depocito;

            return View(acarreos);
        }

        //
        // POST: /Deposito/Edit/5

        [HttpPost]
        public ActionResult Edit(Depositos depo, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                List<int> lista_Id_productos = selectList.Select(element => int.Parse(element)).ToList();
                depo.Producto_Id1 = lista_Id_productos;
                depo.Nombre = depo.ProveedorId;
                _Deposito.update(depo);
            }
            else
            {
                var lista = new List<NombreAcarreo>()
                {
                    new NombreAcarreo()
                    {
                        Id = System.Configuration.ConfigurationManager.AppSettings["Deposito"],
                        Nombre = System.Configuration.ConfigurationManager.AppSettings["Deposito"]
                    }
                };
                ViewBag.Proveedor = new SelectList(lista, "Id", "Nombre", depo.ProveedorId);
                List<Despachos> resultante = new List<Despachos>();
                foreach (string element in selectList)
                {
                    var id = Convert.ToInt32(element);
                    var obj = _Deposito.Despachos(null);
                    obj.Single(X => X.Id == id).Visible = true;
                    resultante.AddRange(obj);
                }
                ViewBag.Productos = _Deposito.Despachos(null);
                return View(depo);
            }


            return RedirectToAction("Index");

            // return View(acarreos);
        }

        ////
        //// GET: /Deposito/Delete/5
        [HttpGet]
        public ActionResult Delete(string id)
        {
            _Deposito.borrar(id);
            return RedirectToAction("Index");
        }
    }
}