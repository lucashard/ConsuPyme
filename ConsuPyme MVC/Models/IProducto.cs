using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IProducto
    {
        void add(Productos producto);
        IEnumerable<Productos> grid();
        Productos Editar(int id);
        int update(Productos o);
        void borrar(int id);
        IEnumerable<Posicion_arancelaria> Posicion_Arancelaria();

    }
}
