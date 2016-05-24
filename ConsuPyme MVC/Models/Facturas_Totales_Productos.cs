using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Models
{
    public class Facturas_Totales_Productos
    {
        public int Id { get; set; }
        public string Num_Lote { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }
        public int Producto_Id { get; set; }
        public int Factura_TotalId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal? Total { get; set; }
        public bool Visible { get; set; }
        public Productos productos { get; set; }

    }
}