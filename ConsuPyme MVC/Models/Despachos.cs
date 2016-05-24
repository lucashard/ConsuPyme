using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Despachos
    {
        public int Id { get; set; }
        public int Producto_Id { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el numero de despacho")]
        public string Dc { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Por favor ingrese la oficializacion")]
        public DateTime Oficializacion { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el fob")]
        [Display(Name= "Fob Total")]
        public decimal Fob_Total { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el flete")]
        [Display(Name = "Flete Total")]
        public decimal Flete_Total { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el seguro total")]
        [Display(Name = "Seguro Total")]
        public decimal Seguro_Total { get; set; }
        [Required(ErrorMessage = "Por favor ingrese la cotizacion")]
        public decimal Cotizacion { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el codigo afip")]
        public string Codigo_Afip { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el arancel sim")]
        [Display(Name = "Arancel Sim")]
        public decimal Arancel_Sim { get; set; }
        public string TipoCambio { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el tipo de cambio")]
        public int TipoCambioId { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el servicio de guarda")]
        [Display(Name = "Servicio de Guarda")]
        public decimal Servicio_Guarda { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el gasto aduanero")]
        [Display(Name = "Gasto Aduanero")]
        public decimal Gasto_Aduanero { get; set; }

        [Required(ErrorMessage = "Por favor los derechos de importacion")]
        [Display(Name = "Derechos de Importacion")]
        public decimal DerechosImportacion { get; set; }

        [Required(ErrorMessage = "Por favor ingrese la taza estadistica")]
        [Display(Name = "Taza Estadistica")]
        public decimal TazaEstadistica { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el impuesto de la multa")]
        [Display(Name = "Multa")]
        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public decimal Multa { get; set; }

        public List<int> Lista_Factura { get; set; }
        public bool Visible { get; set; }
        
        public Dictionary<string,Despachos> Diccionario_Index { get; set; }

    }
    public class NombreDespacho
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}