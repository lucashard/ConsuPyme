using System.Collections.Generic;
using System.Linq;

namespace ConsuPyme_MVC.Models
{
    public class Posicion_ArancelariaServicio : IPosicion_Arancelaria
    {
        private readonly ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public IEnumerable<Posicion_arancelaria> Index()
        {
            var pocicion = datos.Posicion_Arancelaria.ToList();
            return pocicion.Select(o => new Posicion_arancelaria { Numero_Posicion = o.Numero_Posicion, Id= o.Id, Porcentaje_Importacion= o.Porcentaje_Importacion,Porcentaje_Iva = o.Porcentaje_Iva,Porcentaje_Taza_Estadistica = o.Porcentaje_Taza_Estadistica}).ToList();
        }

        public void add(Posicion_arancelaria posicion)
        {
            var ac = new Posicion_Arancelaria
                     {
                Id = posicion.Id,
                Porcentaje_Importacion = posicion.Porcentaje_Importacion,
                Porcentaje_Taza_Estadistica= posicion.Porcentaje_Taza_Estadistica,
                Porcentaje_Iva = posicion.Porcentaje_Iva,
                Numero_Posicion = posicion.Numero_Posicion
            };

            datos.Posicion_Arancelaria.AddObject(ac);
            datos.SaveChanges();
        }

        public Posicion_arancelaria Editar(int id)
        {
            var pos = from op in datos.Posicion_Arancelaria where op.Id == id select new Posicion_arancelaria { Porcentaje_Taza_Estadistica = op.Porcentaje_Taza_Estadistica, Porcentaje_Importacion= op.Porcentaje_Importacion,Id = op.Id,Porcentaje_Iva = op.Porcentaje_Iva,Numero_Posicion = op.Numero_Posicion};
            return pos.SingleOrDefault();
        }

        public int update(Posicion_arancelaria posicion)
        {
            var posi = datos.Posicion_Arancelaria.Single(depo => depo.Id == posicion.Id);
            posi.Porcentaje_Iva = posicion.Porcentaje_Iva;
            posi.Porcentaje_Taza_Estadistica= posicion.Porcentaje_Taza_Estadistica;
            posi.Porcentaje_Importacion= posicion.Porcentaje_Importacion;
            posi.Numero_Posicion = posicion.Numero_Posicion;
            datos.SaveChanges();

            return posi.Id;
        }

        public void borrar(int id)
        {
            var po = datos.Posicion_Arancelaria.SingleOrDefault(x => x.Id.Equals(id));
           datos.Posicion_Arancelaria.DeleteObject(po);
            datos.SaveChanges();
        }
    }
}