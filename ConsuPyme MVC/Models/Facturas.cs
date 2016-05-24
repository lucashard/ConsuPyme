using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Facturas
    {
        public int Id { get;  set; }
        //[Required(ErrorMessage = "Por favor ingrese el flete")]
        
        //[DataType(DataType.Currency)]
        [Required(ErrorMessage = "Por favor ingrese la fecha de embarque")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Fecha de embarque")]
        public DateTime?  Flete { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el envalaje")]
        
        //[DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        [Display(Name = "Embalaje")]
        public decimal?  Envalaje { get; set; }
        public int Cantidad { get; set; }
        public int Producto_Id { get; set; }
        public int Posicion_ArancelariaId { get; set; }
        [Required(ErrorMessage = "Por favor seleccione el nombre del  proveedor")]
        public Int32 ProveedorId { get; set; }
        public decimal Total { get; set; }
        public int Factura_Total_Id { get; set; }
        public int Acarreo_Id { get; set; }
        public bool Visible { get; set; }
        public FacturasTotales FacturasTotales { get; set; }
        public List<Productos> lista_Productos { get; set; }
        [Required(ErrorMessage = "Por favor seleccione los meses de vencimiento de la factura")]
        public int Vencimientos { get; set; }
        public List<Facturas_Totales_Productos> Producto_Id1 { get; set; }
        public Dictionary<string, Facturas> Diccionario { get; set; }
    }
}