using System.ComponentModel.DataAnnotations;

namespace ConsuPyme_MVC.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public int Posicion_Arancelaria_Id { get; set; }
        [Required(ErrorMessage = "Por favor ingrese el codigo")]
        public string Codigo{ get; set; }
        [Required(ErrorMessage = "Por favor ingrese la descripcion")]
        public string Descripcion { get; set; }
        public string Num_Lote { get; set; }
        public decimal Precio_Unitario { get; set; }
        public decimal Cantidad { get; set; }
        public bool Visible { get; set; }
        public decimal Total { get; set; }
    }
}