using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ConsuPyme_MVC.Annotations;

namespace ConsuPyme_MVC.Models
{
    [UsedImplicitly]
    public class DespachantesServicios:IDespachantes
    {
        private ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public void add(Despachantes despachante)
        {
            
                var ac = new Despachante
                         {
                    Id = despachante.Id,
                    Nombre = despachante.ProveedorId,
                    Numero_Factura = despachante.Numero_Factura,
                     Ley= despachante.Ley,
                     Gastos_Despacho = despachante.Gastos_Despacho,
                     AD_Sim = despachante.AD_Sim,
                     Desconsolidado = despachante.Desconsolidado,
                     Djai = despachante.Djai,
                     DespachoId = despachante.Despacho_Id,
                     Servicios = despachante.Servicios,
                     Gestion_Urgente = despachante.Gestion_Urgente,
                     Federal_Express = despachante.Federal_Express,
                     Fecha = despachante.Fecha
                    
                };

                datos.Despachante.AddObject(ac);
                datos.SaveChanges();
        }

        public IEnumerable<Despachantes> grid()
        {
            var acarreoDB = datos.Despachante.ToList();
            var lista_Acarreos =
                acarreoDB.Select(
                    o =>
                    new Despachantes { Nombre = o.Nombre, Numero_Factura = o.Numero_Factura, Id = o.Id, AD_Sim = o.AD_Sim, Desconsolidado = o.Desconsolidado, Djai = o.Djai, Gastos_Despacho = o.Gastos_Despacho, Ley = o.Ley, Servicios = o.Servicios }).ToList();
            Dictionary<string, Despachantes> dictionary = new Dictionary<string, Despachantes>();
            foreach (var element in lista_Acarreos)
            {
                if (!dictionary.ContainsKey(element.Nombre))
                {
                    dictionary[element.Nombre] = new Despachantes { Nombre = element.Nombre, Numero_Factura = element.Numero_Factura, Id = element.Id, AD_Sim = element.AD_Sim, Desconsolidado = element.Desconsolidado, Djai = element.Djai, Gastos_Despacho = element.Gastos_Despacho, Ley = element.Ley, Producto_Id = element.Producto_Id, Servicios = element.Servicios };
                }
            }
            Despachantes ac = new Despachantes {lista_productos = dictionary};
            return lista_Acarreos.AsEnumerable();
        }

        public Despachantes Editar(int id)
        {
            var pos = from op in datos.Despachante join de in datos.Despacho on op.DespachoId equals de.Id where op.Id == id select new Despachantes { Id = op.Id, Numero_Factura = op.Numero_Factura, Nombre= op.Nombre, AD_Sim= op.AD_Sim, Desconsolidado= op.Desconsolidado, Djai= op.Djai,Ley = op.Ley,Servicios = op.Servicios,Gastos_Despacho = op.Gastos_Despacho,Fecha = op.Fecha,Gestion_Urgente = op.Gestion_Urgente,Federal_Express = op.Federal_Express,
                Despachos = new Despachos{Dc = de.Dc,Id = de.Id}};
            return pos.SingleOrDefault();
        }

        public void update(Despachantes o,List<string> list )
        {
            var op = datos.Despachante.Single(despa => despa.Id == o.Id);
            datos.Despachante.DeleteObject(op);
            datos.SaveChanges();
            foreach (var elementos in list)
            {
              
                    op.Nombre = o.ProveedorId;
                    op.Numero_Factura = o.Numero_Factura;
                    op.Ley = o.Ley;
                    op.Gastos_Despacho = o.Gastos_Despacho;
                    op.AD_Sim = o.AD_Sim;
                    op.Desconsolidado = o.Desconsolidado;
                    op.Djai = o.Djai;
                    op.Servicios = o.Servicios;
                    op.DespachoId = Convert.ToInt32(elementos);
                    op.Fecha = o.Fecha;
                    op.Gestion_Urgente = o.Gestion_Urgente;
                    op.Federal_Express = o.Federal_Express;
                    datos.Despachante.AddObject(op);
                    datos.SaveChanges();
            }
            
        }

        public void borrar(string id)
        {
            var id1 = Convert.ToInt32(id);
            var el=datos.Despachante.Where(X => X.Id == id1).Select(X => X).SingleOrDefault();
            datos.Despachante.DeleteObject(el);
            datos.SaveChanges();
        }





        public List<Productos> Productos()
        {
            var lista_Productos = from p in datos.Producto
                                  select
                                      new Productos
                                      {
                                          Id = p.Id,
                                          Codigo = p.Codigo,
                                          Descripcion = p.Descripcion,
                                          Posicion_Arancelaria_Id = p.Posicion_Arancelaria.Id
                                      };
            List<Productos> lista = lista_Productos.ToList();
            return lista;
        }

        public Despachantes Despachos()
        {
            var acarreoDB = datos.Despachante.ToList();
            var lista_Acarreos =
                acarreoDB.Select(
                    o =>
                    new Despachantes { Nombre = o.Nombre, Numero_Factura = o.Numero_Factura, Id = o.Id, AD_Sim = o.AD_Sim, Desconsolidado = o.Desconsolidado, Djai = o.Djai, Gastos_Despacho = o.Gastos_Despacho, Ley = o.Ley, Servicios = o.Servicios/*,Producto_Id = o.ProductoId*/}).ToList();

            return lista_Acarreos.SingleOrDefault();
        }

        private bool buscar_Productos(int? id)
        {
            List<Productos> lista = (from productos in datos.Producto //join acarreos in datos.Acarreo  on productos.Id equals acarreos.ProductoId
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

        public List<Productos> Buscar_Id_Productos(Despachantes id)
        {
            //TODO VERIFICAR PRODUCTO ID
            //List<Despachante> produc = (from obj in datos.Despachante where obj.Nombre == id.Nombre select obj).ToList();
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
            List<Productos> p=new List<Productos>();
            return p;
        }


        public List<Despachos> Despachos1(string busqueda)
        {
            var d = new List<Despachos>();
            if (!string.IsNullOrEmpty(busqueda))
            {
                 d = (from despacho in datos.listado_despachante(busqueda)
                    //where despacho.Dc.Contains(busqueda)
                    select new Despachos()
                    {
                        Id = despacho.Id,
                        Dc = despacho.Dc,
                        Cotizacion = despacho.Cotizacion,
                        Codigo_Afip = despacho.Codigo_Afip,
                        Arancel_Sim = despacho.Arancel_Sim,
                        Flete_Total = despacho.Flete_Total,
                        Oficializacion = despacho.Oficializacion,
                        Fob_Total = despacho.Fob_Total,
                        Seguro_Total = despacho.Seguro_Total,

                        Servicio_Guarda = despacho.Servicio_Guarda
                    }).ToList();
            }
            else
            {
                d = (from despacho in datos.listado_despachante(null)
                     select new Despachos()
                     {
                         Id = despacho.Id,
                         Dc = despacho.Dc,
                         Cotizacion = despacho.Cotizacion,
                         Codigo_Afip = despacho.Codigo_Afip,
                         Arancel_Sim = despacho.Arancel_Sim,
                         Flete_Total = despacho.Flete_Total,
                         Oficializacion = despacho.Oficializacion,
                         Fob_Total = despacho.Fob_Total,
                         Seguro_Total = despacho.Seguro_Total,

                         Servicio_Guarda = despacho.Servicio_Guarda
                     }).ToList();
            }

        
            return d;
        }


        public List<Despachos> MarcarGrilla(Despachantes des, List<Despachos> lista)
        {
            foreach (Despachos ele in lista)
            {
                if (ele.Id.Equals(des.Despachos.Id))
                {
                    ele.Visible = true;
                }
            }
            return lista;
        }
    }
}