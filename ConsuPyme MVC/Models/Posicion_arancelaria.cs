using System;
using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Posicion_arancelaria
    {
        public int Id { set; get; }
        [Required(ErrorMessage = "Por favor ingrese la pocicion arancelaria")]
        public string Numero_Posicion { set; get; }
        [Required(ErrorMessage = "Por favor ingrese el iva")]
        public decimal Porcentaje_Iva { set; get; }
        [Required(ErrorMessage = "Por favor ingrese la taza de estadistica")]
        public decimal Porcentaje_Taza_Estadistica { set; get; }
        [Required(ErrorMessage = "Por favor ingrese el porcentaje de importacion")]
        public decimal Porcentaje_Importacion { set; get; }
    }
}