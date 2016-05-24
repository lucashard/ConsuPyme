using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class FacturasTotales
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor ingrese el vencimiento de factura")]
        public DateTime Vencimiento_Factura { get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el nombre de factura")]
        public string Nombre { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor ingrese la fecha de la factura")]
        [Display(Name = "Fecha Factura")]
        public DateTime Creacion { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el numero de factura")]
        public string Numero_Factura { get; set; }
        public List<Facturas_Totales_Productos> FacturasTotalesProductos { get; set; } 
        public decimal Total { get; set; }
        public decimal Cantidad_Despacho { get; set; }
        public decimal Cantidad_Acarreo { get; set; }
        public decimal Cantidad_Deposito { get; set; }

        public Int32 ProveedorId { get; set; }
        public bool Visible { get; set; }
    }
}