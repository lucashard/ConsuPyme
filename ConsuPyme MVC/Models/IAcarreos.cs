using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IAcarreos
    {
        void add(Acarreos acarreo);
        IEnumerable<Acarreos> grid();
        Acarreos Editar(int id);
        int update(Acarreos o);
        void borrar(string id);
        List<Despachos> Despachos(string busqueda);
        List<Despachos> Buscar_Id_Productos(Acarreos id,List<Despachos> listaDespachos);
    }
}
