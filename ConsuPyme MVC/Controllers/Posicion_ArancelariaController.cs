using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;
using PagedList;

namespace ConsuPyme_MVC.Controllers
{
    public class Posicion_ArancelariaController : Controller
    {
        private IPosicion_Arancelaria _posicion;
        private static List<string> selectList;
        public Posicion_ArancelariaController(IPosicion_Arancelaria po)
        {
            this._posicion = po;
        }
        public ActionResult Index()
        {
            @ViewBag.Error = Error;
            
            ViewData["SelectList"] = HttpContext.Session["SelectList"] ?? new List<String>();
            return View(_posicion.Index());
        }

        //Persistencia de Checkbox
        [HttpPost]
        public ActionResult Select(bool isChecked, String id)
        {
            selectList = (List<String>)HttpContext.Session["SelectList"] ?? new List<String>();
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

        

        public ActionResult Create()
        {
            Error = "";
            return View();
        } 

        [HttpPost]
        public ActionResult Create(Posicion_arancelaria posicion_arancelaria)
        {
            Error = "";
            if (ModelState.IsValid)
            {
                try
                {
                    _posicion.add(posicion_arancelaria);
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
            Error = "";
            return View(_posicion.Editar(id));
        }

        [HttpPost]
        public ActionResult Edit(Posicion_arancelaria poci)
        {
            Error = "";
            try
            {
                _posicion.update(poci);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        private static string Error { get; set; }
        [HttpGet]
        public ActionResult Delete(string id)
        {
            try
            {
                _posicion.borrar(Convert.ToInt32(id));
            }
            catch (Exception)
            {

                Error = "No se puede borrar la posicion arancelaria porque ya la usa un producto";
            }
            
            return RedirectToAction("Index");
        }
    }
}
