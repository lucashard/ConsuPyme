using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsuPyme_MVC.Models
{
    public class DepositoServicio : IDeposito
    {
        private readonly ConsuPymeEntities1 datos = new ConsuPymeEntities1();

        public List<Depositos> Index()
        {
            List<Deposito> acarreoDB = datos.Deposito.ToList();
            List<Depositos> lista_Acarreos =
                acarreoDB.Select(
                    o =>
                        new Depositos
                        {
                            Nombre = o.Nombre,
                            Importe = o.Importe,
                            Numero_Factura = o.Numero_Factura,
                            Id = o.Id
                        }).ToList();
            return lista_Acarreos;
        }

        public void add(Depositos _deposito)
        {
            var ac = new Deposito
                     {
                         Id = _deposito.Id,
                         Nombre = _deposito.ProveedorId,
                         Numero_Factura = _deposito.Numero_Factura,
                         Importe = _deposito.Importe,
                         DespachoId = _deposito.Despacho_Id,
                         Fecha_Factura = _deposito.Fecha
                         //DespachoId = _deposito.
                         //   ProductoId = i
                     };

            datos.Deposito.AddObject(ac);
            datos.SaveChanges();
        }

        public Depositos Editar(int id)
        {
            IQueryable<Depositos> deposito = from op in datos.Deposito
                //join fc in datos.Factura on op.ProductoId equals fc.Producto_Id 
                where op.Id == id
                select
                    new Depositos {Importe = op.Importe, Nombre = op.Nombre, Numero_Factura = op.Numero_Factura,Id = op.Id,Despacho_Id = op.DespachoId,Fecha = op.Fecha_Factura};
            return deposito.SingleOrDefault();
        }


        public int update(Depositos o)
        {
            Deposito op = datos.Deposito.Single(deposito => deposito.Id == o.Id);
          datos.Deposito.DeleteObject(op);
            datos.SaveChanges();

            foreach (int acarreo in o.Producto_Id1)
            {
                op.Nombre = o.Nombre;
                op.Numero_Factura = o.Numero_Factura;
                op.Importe = o.Importe;
                op.DespachoId = acarreo;
                datos.Deposito.AddObject(op);
                datos.SaveChanges();
            }


            return op.Id;
        }

        public void borrar(string id)
        {
            int id1 = Convert.ToInt32(id);
            var deposito=datos.Deposito.SingleOrDefault(X => X.Id == id1);
            datos.Deposito.DeleteObject(deposito);
            datos.SaveChanges();
        }


        public List<Productos> Productos(List<int> id)
        {
            IQueryable<Productos> lista_Productos = from p in datos.Producto
                select
                    new Productos
                    {
                        Id = p.Id,
                        Codigo = p.Codigo,
                        Descripcion = p.Descripcion,
                        Posicion_Arancelaria_Id = p.Posicion_Arancelaria.Id,
                    };
            List<Productos> lista = lista_Productos.ToList();
            if (id != null)
            {
                int i = 0;
                foreach (Productos e in lista)
                {
                    //elemento.Visible= buscar_Producto(id[i]);
                    e.Visible = buscar_Productos(id[i]);
                    i = i + 1;
                }
            }
            return lista;
        }



        public List<Productos> Buscar_Id_Productos(Depositos id)
        {
            //TODO VERIFICAR
            //List<Deposito> produc = (from obj in datos.Deposito where obj.Nombre == id.Nombre select obj).ToList();
            //List<Productos> lista =
            //       (from productos in datos.Producto
            //        select new Productos
            //               {
            //            Descripcion = productos.Descripcion,
            //            Codigo = productos.Codigo,
            //            Id = productos.Id,
            //            Visible = false
            //        }).ToList();
            //foreach (var element in lista)
            //{
            //    foreach (var elementoLista in produc)
            //    {
            //        if (element.Id.Equals(elementoLista.ProductoId))
            //        {
            //            element.Visible = true;
            //            break;
            //        }
            //        element.Visible = false;
            //    }
            //}
            //return lista;
            var p = new List<Productos>();
            return p;
        }


        public List<Despachos> Despachos(string busqueda)
        {
            //var lista_Productos = datos.listado_Facturas(busqueda).ToList();
            //List<FacturasTotales> lista = lista_Productos.Select(o => new FacturasTotales { Id = o.Id, Numero_Factura = o.Numero_Factura, Nombre = o.Nombre, Vencimiento_Factura = o.Vencimiento_Factura, Creacion = o.Creacion, Visible = false }).ToList();
            //return lista;
            //
            var despachos = new List<Despachos>();

            List<listado_deposito_Result> query = datos.listado_deposito(busqueda).ToList();

            despachos = (from a in query
                select new Despachos
                       {
                           Id = a.Id,
                           Dc = a.Dc,
                           Cotizacion = a.Cotizacion,
                           Codigo_Afip = a.Codigo_Afip,
                           Arancel_Sim = a.Arancel_Sim,
                           Flete_Total = a.Flete_Total,
                           Oficializacion = a.Oficializacion,
                           Fob_Total = a.Fob_Total,
                           Seguro_Total = a.Seguro_Total,
                           Servicio_Guarda = a.Servicio_Guarda
                       }).ToList();

            return despachos;
        }

        private bool buscar_Productos(int? id)
        {
            List<Productos> lista =
                (from productos in datos.Producto
                    //join acarreos in datos.Acarreo  on productos.Id equals acarreos.ProductoId
                    where productos.Id == id
                    select
                        new Productos
                        {
                            Descripcion = productos.Descripcion,
                            Codigo = productos.Codigo,
                            Id = productos.Id,
                            Visible = false
                        }).ToList();
// ReSharper disable RedundantTernaryExpression
            return lista.Count >= 1 ? true : false;
// ReSharper restore RedundantTernaryExpression
        }
    }
}