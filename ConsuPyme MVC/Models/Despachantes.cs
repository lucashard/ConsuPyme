using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Despachantes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Por favor seleccione el nombre del despachante")]
        public string ProveedorId { get; set; }
        public int Producto_Id { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el numero de factura")]
        public string Numero_Factura { get; set; }
        
        [DataType(DataType.Currency)]
        [Display(Name = "Ley 25413")]
        [Required(ErrorMessage = "Por favor ingrese la ley 2541")]
        public decimal Ley{ get; set; }
        
        [DataType(DataType.Currency)]
        [Display(Name = "SIMI")]
        [Required(ErrorMessage = "Por favor ingrese el SIMI")]
        public decimal Djai { get; set; }
        
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Por favor ingrese el ad sim")]
        [Display(Name = "Ad Sim")]
        public decimal AD_Sim { get; set; }
        [Required(ErrorMessage = "Por favor ingrese los Gastos Operativos Aereos")]
        [Display(Name = "Gastos Operativos Aereos")]
        [DataType(DataType.Currency)]
        public decimal Gastos_Despacho { get; set; }
        [Required(ErrorMessage = "Por favor ingrese Gestion Urgente")]
        [DataType(DataType.Currency)]
        [Display(Name = "Gestion Urgente")]
        public decimal Gestion_Urgente { get; set; }

        [Required(ErrorMessage = "Por favor ingrese Federal Express")]
        [DataType(DataType.Currency)]
        [Display(Name = "Federal Express")]
        public decimal Federal_Express { get; set; }

        [Required(ErrorMessage = "Por favor ingrese Honorarios")]
        [Display(Name = "Honorarios")]
        [DataType(DataType.Currency)]
        public decimal Desconsolidado { get; set; }
        [Required(ErrorMessage = "Por favor ingrese los servicios")]
        
        [DataType(DataType.Currency)]
        public decimal Servicios { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }
        public Despachos Despachos { get; set; }
        public int Despacho_Id { get; set; }
        public List<Productos> Productos{get;set;}
        public List<int> Producto_Id1 { get; set; }
        public Dictionary<string, Despachantes> lista_productos { get; set; }
    }
}