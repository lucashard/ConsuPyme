using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;
using Ninject;

namespace ConsuPyme_MVC.Controllers
{ 
    public class AcarreosController : Controller
    {
        private static List<String> selectList = new List<String>();
        private IAcarreos _Acarreos;
        [Inject]
        public AcarreosController(IAcarreos acarreo)
        { 
            _Acarreos = acarreo;
        }
        public ViewResult Index()
        {
            return View(_Acarreos.grid());
        }

        [HttpGet]
        public ActionResult Create()
        {
            selectList.Clear();
            Id = 0;
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? selectList;
            var lista = new List<NombreAcarreo>() { new NombreAcarreo() { Id = System.Configuration.ConfigurationManager.AppSettings["Acarreo"], Nombre = System.Configuration.ConfigurationManager.AppSettings["Acarreo"] } };
            ViewBag.Proveedor = new SelectList(lista, "Id", "Nombre", "SELECCIONE");
            ViewBag.Productos = _Acarreos.Despachos(null);
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

        [HttpGet]
        public ActionResult Busqueda(String bus)
        {

            if (Id != 0)
            {
                var acarreos = _Acarreos.Editar(Id);
                var ac = _Acarreos.Buscar_Id_Productos(acarreos, _Acarreos.Despachos(null));
                var prod=ac.Where(P=>P.Dc.Contains(bus)).ToList() ;
                foreach (var elem in prod)
                {
                    elem.Visible = false;
                }
                    var id = Convert.ToInt32(acarreos.DespachoId);
                    if (prod.Where(x => x.Id == id).ToList().Any() && !selectList.Any())
                    {
                        prod.Single(x => x.Id == id).Visible = true;
                    }
                    else if(selectList.Any())
                    {
                        id = Convert.ToInt32(selectList[0]);
                        if (prod.Where(x => x.Id == id).ToList().Any())
                        {
                            prod.Single(x => x.Id == id).Visible = true;
                        }
                    }
                
                ViewBag.Productos = prod;
            }
            else
            {
                var prod = _Acarreos.Despachos(bus);
                if (selectList.Any())
                {
                    var id = Convert.ToInt32(selectList[0]);
                    if (prod.Where(x => x.Id == id).ToList().Any())
                    {
                        prod.Single(x => x.Id == id).Visible = true;
                    }
                }
                ViewBag.Productos =prod ;    
            }
            
            return PartialView("Grilla");
        }

        private List<NombreAcarreo> NombreAcarreos
        {
            get
            {
                return new List<NombreAcarreo>() { new NombreAcarreo() { Id = System.Configuration.ConfigurationManager.AppSettings["Acarreo"], Nombre = System.Configuration.ConfigurationManager.AppSettings["Acarreo"] } };
            }
        }


        //[HttpPost]
        [HttpPost, ActionName("Create")]
        public ActionResult Create(Acarreos acarreos)
        {
            if (selectList.Count == 0)
            {
                ModelState.AddModelError("Productos", "Por favor seleccione lo/s producto de la factura");
            }
              
                if (ModelState.IsValid)
                {
                    foreach (var element in selectList)
                    {
                        acarreos.DespachoId = Convert.ToInt32(element);
                        _Acarreos.add(acarreos);
                    }
                    return RedirectToAction("Index");
                }
            
            

                ViewBag.Proveedor = new SelectList(NombreAcarreos, "Id", "Nombre", "SELECCIONE");
                ViewBag.Productos = _Acarreos.Despachos(null);
                
            
            return View(acarreos);
        }

        public ActionResult Menu()
        {
            return PartialView();
        }
        
        public ActionResult Inicio()
        {
            return PartialView("Menu");
        }

        public ActionResult Prueba()
        {
            return PartialView("Prueba");
        }
 

        private static int Id { get; set; }
        public ActionResult Edit(int id)
        {
            Id = id;
            var acarreos=_Acarreos.Editar(Id);
            acarreos.ProveedorId=acarreos.Nombre;
            ViewBag.Proveedor = new SelectList(NombreAcarreos, "Id", "Nombre", acarreos.ProveedorId);
            ViewBag.Productos = _Acarreos.Buscar_Id_Productos(acarreos, _Acarreos.Despachos(null));
            return View(acarreos);
        }

        [HttpPost]
        public ActionResult Edit(Acarreos acarreos,FormCollection collection)
        {

            if (ModelState.IsValid)
            {
                List<int> lista_Id_productos = selectList.Select(element => int.Parse(element)).ToList();
                acarreos.Nombre = acarreos.ProveedorId;
                acarreos.Producto_Id1 = lista_Id_productos;
                _Acarreos.update(acarreos);


                return RedirectToAction("Index");
            }
           
                ViewBag.Proveedor = new SelectList(NombreAcarreos, "Id", "Nombre", acarreos.ProveedorId);
                ViewBag.Productos = _Acarreos.Buscar_Id_Productos(acarreos, _Acarreos.Despachos(null));
                return View(acarreos);
           
        }

        [HttpGet, ActionName("Delete")]
        public ActionResult Delete(string id)
        {
            _Acarreos.borrar(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            //base.Dispose(disposing);
        }
    }
}