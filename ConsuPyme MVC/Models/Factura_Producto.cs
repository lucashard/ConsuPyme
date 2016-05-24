namespace ConsuPyme_MVC.Models
{
    public class Factura_Producto
    {
        public int Id { get; set; }
        public int ProductoId { get; set; }
        public int FacturaId { get; set; }
        public int Cantidad { get; set; }
    }
}