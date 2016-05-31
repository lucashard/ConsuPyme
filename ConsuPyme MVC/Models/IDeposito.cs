using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public interface IDeposito
    {
        List<Depositos> Index();
        void add(Depositos _deposito);
        Depositos Editar(int id);
        int update(Depositos o);
        void borrar(string id);
        List<Productos> Productos(List<int> id);
        List<Despachos> Despachos(string id);
        List<Productos> Buscar_Id_Productos(Depositos id);
    }
}
