using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IPosicion_Arancelaria
    {
        IEnumerable<Posicion_arancelaria> Index();
        void add(ConsuPyme_MVC.Models.Posicion_arancelaria posicion);
        ConsuPyme_MVC.Models.Posicion_arancelaria Editar(int id);
        int update(Posicion_arancelaria posicion);
        void borrar(int id);
    }
}
