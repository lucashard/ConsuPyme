using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace ConsuPyme_MVC.Models
{
    public class AcarreoServicio:IAcarreos
    {
        private ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public void add(Acarreos acarreo)
        {
                var ac = new Acarreo
                             {
                                 Id = acarreo.Id,
                                 Nombre = acarreo.ProveedorId,
                                 Numero_Factura = acarreo.Numero_Factura,
                                 Importe = acarreo.Importe,
                                 DespachoId = acarreo.DespachoId,
                                 Fecha_Factura = acarreo.Fecha
                                 //ProductoId = i
                             }; 

                datos.Acarreo.AddObject(ac);
                datos.SaveChanges();
        }

        public IEnumerable<Acarreos> grid()
        {
            var acarreoDB = datos.Acarreo.ToList();
            List<Acarreos> listaDB= acarreoDB.Select(o => new Acarreos {Nombre = o.Nombre, Importe = o.Importe, Numero_Factura = o.Numero_Factura, Id = o.Id}).ToList();
            Dictionary<string,Acarreos> dictionary=new Dictionary<string, Acarreos>();
            foreach (var element in listaDB)
            {
                if (!dictionary.ContainsKey(element.Nombre))
                {
                    dictionary[element.Nombre]=new Acarreos {Id = element.Id,Importe=element.Importe,Nombre = element.Nombre,Numero_Factura = element.Numero_Factura,Producto_Id = element.Producto_Id};
                }
            }
            Acarreos ac=new Acarreos {Diccionario = dictionary};

            return listaDB.AsEnumerable();
        }

        public Acarreos Editar(int id)
        {
            var oOperario = (from op in datos.Acarreo where op.Id == id select new Acarreos {Id = op.Id, Importe= op.Importe, Nombre = op.Nombre, Numero_Factura= op.Numero_Factura,DespachoId = op.DespachoId,Fecha = op.Fecha_Factura}).Distinct();
            return oOperario.SingleOrDefault();
        }

        public int update(Acarreos o)
        {
            var op = datos.Acarreo.Single(acarreos => acarreos.Id == o.Id);
            datos.Acarreo.DeleteObject(op);
            datos.SaveChanges();
        
            foreach (int acarreo in o.Producto_Id1)
            {
                op.Nombre = o.ProveedorId;
                op.Numero_Factura = o.Numero_Factura;
                op.Importe = o.Importe;
                op.DespachoId = acarreo;
                op.Fecha_Factura = o.Fecha;
                datos.Acarreo.AddObject(op);
                datos.SaveChanges();
            }
            
            return op.Id;
        }

        public void borrar(string id)
        {
            int id1 = Convert.ToInt32(id);
            var acarr = datos.Acarreo.SingleOrDefault(X => X.Id == id1);
            datos.Acarreo.DeleteObject(acarr);
            datos.SaveChanges();


        }

       


        public List<Despachos> Despachos(string busqueda)
        {
            var despachos =new List<Despachos>();
            if(!string.IsNullOrEmpty(busqueda))
            {
                despachos = (from despacho in datos.listado_acarreo(busqueda)
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
                                 //Costo_despacho_producto = despacho.Costo_despacho_producto,
                                 Servicio_Guarda = despacho.Servicio_Guarda
                             }).ToList();
                }
            else
            {
                despachos = (from despacho in datos.listado_acarreo(null)
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
                                 //Costo_despacho_producto = despacho.Costo_despacho_producto,
                                 Servicio_Guarda = despacho.Servicio_Guarda
                             }).ToList();
            }
            return despachos;
        }

        private bool buscar_Producto(int? id)
        {
            List<Productos> lista = (from productos in datos.Producto //join acarreos in datos.Acarreo  on productos.Id equals acarreos.ProductoId
                                     where productos.Id==id
                                     select
                                         new Productos
                                         {
                                             Descripcion = productos.Descripcion,
                                             Codigo = productos.Codigo,
                                             Id = productos.Id,
                                             Visible = false
                                         }).ToList();
            if (lista.Count >= 1) return true;
            return false;
        }




        public List<Despachos> Buscar_Id_Productos(Acarreos id, List<Despachos> des)
        {
            foreach (Despachos elementos in des)
            {
                if (elementos.Id.Equals(id.DespachoId))
                {
                    elementos.Visible = true;
                }
            }
            return des;
        }
    }
}