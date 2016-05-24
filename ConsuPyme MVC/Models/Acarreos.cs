using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Acarreos
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Por favor seleccione el nombre del acarreo")]
        public string ProveedorId { get; set; }
        public List<int> Producto_Id { get; set; }
        public List<int> Producto_Id1 { get; set; }
        [Required(ErrorMessage = "Por favor ingrese la fecha")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Fecha{ get; set; }
        //[Required(ErrorMessage = "Por favor ingrese el nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el numero de factura")]
        public string Numero_Factura { get; set; }
        public int DespachoId { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el importe")]
        public decimal Importe{ get; set; }
        public Dictionary<string, Acarreos> Diccionario { get; set; }
        public int Id3 { get; set; }
    }

    public class NombreAcarreo
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}