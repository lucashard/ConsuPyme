using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsuPyme_MVC.Models
{
    public class ProductoServicio:IProducto
    {
        private ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public void add(Productos producto)
        {
            var ac = new Producto
                     {
                Id = producto.Id,
                Descripcion= producto.Descripcion,
                Codigo = producto.Codigo,
                Posicion_ArancelariaId = producto.Posicion_Arancelaria_Id
            };

            datos.Producto.AddObject(ac);
            datos.SaveChanges();
        }

        public IEnumerable<Productos> grid()
        {
            var producto =
                (from p in datos.Producto
                    join poscion in datos.Posicion_Arancelaria on p.Posicion_ArancelariaId equals poscion.Id select new {p,poscion}
                ).AsEnumerable().Select(p => new Productos() { Id=p.p.Id,Codigo = p.p.Codigo, Descripcion = p.p.Descripcion, Posicion_Arancelaria_Id = ConvertirNumero(p.poscion.Id) });
            return producto.Select(o => new Productos { Id = o.Id, Descripcion = o.Descripcion, Codigo = o.Codigo, Posicion_Arancelaria_Id=o.Posicion_Arancelaria_Id}).ToList();
        }

        private static int ConvertirNumero(long p)
        {
            return Convert.ToInt32(p);
        }

        public IEnumerable<Posicion_arancelaria> Posicion_Arancelaria()
        {
            List<Posicion_arancelaria> list =
                datos.Posicion_Arancelaria.Select(acarreo => new Posicion_arancelaria { Id = acarreo.Id, Numero_Posicion = acarreo.Numero_Posicion,Porcentaje_Iva = acarreo.Porcentaje_Iva,Porcentaje_Importacion = acarreo.Porcentaje_Importacion,Porcentaje_Taza_Estadistica = acarreo.Porcentaje_Taza_Estadistica}).ToList();
            return list;
        }

        public Productos Editar(int id)
        {
            var pos = from op in datos.Producto where op.Id == id select new Productos { Descripcion = op.Descripcion, Id= op.Id, Codigo= op.Codigo,Posicion_Arancelaria_Id=op.Posicion_ArancelariaId};
            return pos.SingleOrDefault();
        }

        public int update(Productos o)
        {
            var posi = datos.Producto.Single(depo => depo.Id == o.Id);
            posi.Codigo= o.Codigo;
            posi.Descripcion = o.Descripcion;
            posi.Posicion_ArancelariaId = o.Posicion_Arancelaria_Id;
            
            datos.SaveChanges();

            return posi.Id;
        }

        public void borrar(int id)
        {
            var po = datos.Producto.SingleOrDefault(x => x.Id.Equals(id));
            datos.Producto.DeleteObject(po);
            datos.SaveChanges();
        }
    }
}