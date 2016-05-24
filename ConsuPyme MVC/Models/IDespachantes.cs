using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
   public interface IDespachantes
    {
        void add(Despachantes despachante);
        IEnumerable<ConsuPyme_MVC.Models.Despachantes> grid();
        Despachantes Editar(int id);
        void update(Despachantes o,List<string> j);
        void borrar(string id);
        List<Productos> Productos();
        List<Despachos> Despachos1(string busqueda);
        Despachantes Despachos();
        List<Productos> Buscar_Id_Productos(Despachantes id);
        List<Despachos> MarcarGrilla(Despachantes des, List<Despachos> lista);
       

    }
}
