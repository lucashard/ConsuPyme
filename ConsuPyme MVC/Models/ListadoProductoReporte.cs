using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Models
{
    public class ListadoProductoReporte
    {
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public decimal PrecioDolar { get; set; }
        public string ImageUrl { get; set; }
        public string NumeroFactura { get; set; }
        public string Factura { get; set; }
    }

    public class ReportesFechas : Listado
    {
        public Dictionary<string, List<ListadoProductoReporte>> FacturaProducto { get; set; } 
    }

    public class Listado : List<ListadoProductoReporte>
    {
        public string ImageUrl { get; set; }
        public string Dolares { get; set; }
    }
}