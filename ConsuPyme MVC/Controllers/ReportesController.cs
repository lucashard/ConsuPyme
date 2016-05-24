using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ConsuPyme_MVC.Models;
using ReportManagement;

namespace ConsuPyme_MVC.Controllers
{
    public class ReportesController : PdfViewController
    {
        private Ireporte _Reporte;
        public ReportesController(Ireporte reporte)
        {
            _Reporte = reporte;
        }
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Reporte = new SelectList(_Reporte.ObtenerNombreDespacho(), "Id", "Nombre");
            var f = new Fechas();
            return View(f);
        }



        public static Int32 IdDespacho { get; set; }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult Index(Fechas f,FormCollection form)
        {
            if (!f.FechaDesde.HasValue || !f.FechaHasta.HasValue)
            {
                IdDespacho = Convert.ToInt32(Request.Form["Reporte"]);
                return RedirectToAction("PrintCustomers", "Reportes");
            }
            else
            {
                fDesde = f.FechaDesde;
                fHasta = f.FechaHasta;
                return RedirectToAction("ReportesByFecha", "Reportes");
            }

        }
        public static DateTime? fDesde { get; set; }
        public static DateTime? fHasta { get; set; }

        public ActionResult ReportesByFecha()
        {
            List<ListadoProductoReporte> listado = _Reporte.ObtenerReporteDespacho(fDesde, fHasta);
            List<decimal> dolarpromedio = new List<decimal>();
            Dictionary<string,string> dictionary=new Dictionary<string, string>();
            foreach (var elem in listado)
            {
                if(!dictionary.ContainsKey(elem.NumeroFactura))
                {
                    dictionary[elem.NumeroFactura]=elem.NumeroFactura;
                    dolarpromedio.Add(elem.PrecioDolar);
                }
            }

            decimal promedio = dolarpromedio.Sum() / dolarpromedio.Count();
            string dolares = "Cotizacion Precio promedio del dolar DOLAR: " + promedio.ToString("#.##");
            Listado h = new Listado();
            h.AddRange(listado);
            FillImageUrl(h, "report.jpg", dolares);
            return this.ViewPdf("Reporte", "ReporteByFecha", h, dolares);
        }
        
        public ActionResult PrintCustomers()
        {
            Listado h=new Listado();
            List<ListadoProductoReporte> customerList = _Reporte.ObtenerReporteDespacho(IdDespacho);
            decimal dolar = Convert.ToDecimal(customerList[0].PrecioDolar);
            h.AddRange(customerList);
            string dolares = "Cotizacion Precio DOLAR: " + dolar.ToString("#.##");
            FillImageUrl(h, "report.jpg",dolares);
            return this.ViewPdf("Reporte", "PrintDemo", h,dolares);
        }

        private void FillImageUrl(Listado customerList, string imageName,string dolares)
        {
            string url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            customerList.ImageUrl = url + "Content/" + imageName;
            customerList.Dolares = dolares;
        }

        private CustomerList CreateCustomerList()
        {

            var h=new CustomerList();
                h.Add(new Customer { Id = 1, Nombre = "Patrick", Address = "Geuzenstraat 29", Place = "Amsterdam" });
            return h;


        }
        
    }
}
