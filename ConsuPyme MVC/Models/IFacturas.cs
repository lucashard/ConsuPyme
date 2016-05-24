using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IFacturas
    {
        Facturas grid();
        Facturas creacion();
        int Alta(FacturasTotales factura);
        void Alta_Factura(Facturas factura);
        void Baja(int id);
        void Update(Facturas factura);
        Facturas Detalle(int id);
        List<Posicion_arancelaria> Posicion_Arancelaria();
        List<Despachantes> Despacho();
        List<Depositos> Depositos();
        List<Facturas_Totales_Productos>  Buscar_Id_Productos(int id);
        void add(Facturas _facturas);
        void alta_Factura_Producto(Grilla ftp);
        //public List<Factura_Total_Producto> Buscar_Id_Productos(int id)
        Facturas ObtenerGrilla(string busqueda);
        List<Proveedor> ObtenerProveedores();
    }
}
