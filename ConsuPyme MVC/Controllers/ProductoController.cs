using System;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;
using PagedList;

namespace ConsuPyme_MVC.Controllers
{
    public class ProductoController : Controller
    {
        private IProducto _producto;
        public ProductoController(IProducto producto)
        {
            this._producto = producto;
        }

        public ActionResult Index()
        {
            @ViewBag.Error = Error;
            return View(_producto.grid());
        }

        public ActionResult Create()
        {
            Error = "";
            ViewBag.Posicion = new SelectList(_producto.Posicion_Arancelaria(), "Id", "Numero_Posicion");
            return View();
        } 
        
        [HttpPost]
        public ActionResult Create(Productos producto)
        {
            Error = "";
            if (ModelState.IsValid)
            {
                try
                {
                    producto.Posicion_Arancelaria_Id = Convert.ToInt32(Request.Form["Posicion"]);
                    _producto.add(producto);
                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
        
        public ActionResult Edit(int id)
        {
            SelectList seleccion = new SelectList(_producto.Posicion_Arancelaria(), "Id", "Numero_Posicion", _producto.Editar(id).Posicion_Arancelaria_Id);
            ViewData["Posicion"] = seleccion;
            return View(_producto.Editar(id));
        }
        
        [HttpPost]
        public ActionResult Edit(Productos producto)
        {
            Error = "";
            try
            {
                producto.Posicion_Arancelaria_Id = Convert.ToInt32(Request.Form["Posicion"]);
                _producto.update(producto);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private static string Error { get; set; }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                _producto.borrar(id);
            }
            catch (Exception)
            {
                Error = "No se puede borrar el producto porque ya esta asociado a una factura";
            }
            
            return RedirectToAction("Index");
        }
    }
}
