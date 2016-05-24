using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IDespachos
    {
        void add(Despachos despachos);
        Despachos grid();
        Despachos Editar(int id);
        void update(Despachos o,List<string> facturas );
        void borrar(string id);
        List<FacturasTotales> Facturas(string busqueda);
        List<TipoCambio> buscarTipoCambio();
        List<Productos> Productos(List<int> productoId);
        Dictionary<string, FacturasTotales> Facturas(int id, List<FacturasTotales> listaCompleta);


        List<Facturas> verFacturas(int id);
    }
}
