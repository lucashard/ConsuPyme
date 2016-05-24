using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Depositos
    {
        public int Id { get; set; }
        public int Producto_Id { get; set; }
        [Required(ErrorMessage = "Por favor seleccione el nombre del deposito")]
        public string ProveedorId { get; set; }
     //   [Required(ErrorMessage = "Por favor ingrese el nombre")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el numero de factura")]
        public string Numero_Factura { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el importe")]
        public decimal Importe { get; set; }
        public int Despacho_Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
        //public List<Productos> lista_productos=new  List<Productos>();
        public List<int> Producto_Id1 { get; set; }
        public Dictionary<string, Depositos> lista_productos { get; set; }
    }
    public class NombreDeposito
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}