using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Models
{
    public class Factura_Total_Producto
    {
        public int Id { get; set; }
        public string Num_Lote { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio_Unitario { get; set; }
        public int Producto_Id { get; set; }
        public int Factura_TotalId { get; set; }

    }
}