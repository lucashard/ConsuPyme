using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ConsuPyme_MVC.Annotations;

namespace ConsuPyme_MVC.Models
{
    [UsedImplicitly]
    public class FacturaServicio:IFacturas
    {
        private readonly ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public Facturas grid()
        {
            IQueryable<Facturas> lista = from element in datos.Factura_Total
                                         join fc in datos.Factura on element.Id equals fc.Factura_Total_Id
                join prov in datos.Proveedor on element.ProveedorId equals prov.Id
            
            
                                       //  join fp in datos.Factura_Total_Producto on element.Id equals fp.Factura_TotalId
            
            
                                         select
                                             new Facturas
                                                 {
                                                     FacturasTotales =
                                                         new FacturasTotales
                                                         {
                                                                 Vencimiento_Factura = element.Vencimiento_Factura,
                                                                 Nombre = prov.Nombre,
                                                                 Creacion = element.Creacion,
                                                                 Numero_Factura = element.Numero_Factura,
                                                                 Id = element.Id
                                                             },
                                                     Envalaje = fc.Envalaje,
                                                     Flete = fc.Fecha_Embarque,
                                              //       Cantidad = fp.Cantidad,
                                                     Id = fc.Id,
                                                //     Producto_Id = fp.ProductoId,
                                                     Factura_Total_Id = fc.Factura_Total_Id
                                                 };
            List<Facturas> lista1 = lista.ToList();
            Dictionary<string,Facturas> dictionary=new Dictionary<string, Facturas>();
            foreach (var element in lista1)
            {
                if (!dictionary.ContainsKey(element.Id.ToString()))
                {
                    dictionary[element.Id.ToString()] = new Facturas { Id = element.Id, Flete = element.Flete, Envalaje= element.Envalaje, Cantidad= element.Cantidad, Producto_Id = element.Producto_Id ,Factura_Total_Id = element.Factura_Total_Id,FacturasTotales = new FacturasTotales {Creacion = element.FacturasTotales.Creacion,Nombre = element.FacturasTotales.Nombre,Total = (decimal) element.Total,Cantidad_Acarreo = 0,Cantidad_Deposito=0,Cantidad_Despacho = 0,Id = element.FacturasTotales.Id,Vencimiento_Factura = element.FacturasTotales.Vencimiento_Factura,Numero_Factura = element.FacturasTotales.Numero_Factura}};
                }
            }
// ReSharper disable once UnusedVariable
            foreach (var element in dictionary.Keys)
            {
                foreach (var valores in dictionary.Values)
                {
                    valores.Total = Convert.ToDecimal( Total_Factura(valores.FacturasTotales.Id));    
                }
                
            }
            Facturas facturas = new Facturas {Diccionario = dictionary};
            facturas.Diccionario=dictionary;

            return facturas;
        }

        private  decimal Total_Factura(int id)
        {
            decimal total = 0;
            IQueryable<Total> lista = from element in datos.Factura
                                      join facturaProducto in datos.Factura_Total on element.Factura_Total_Id equals facturaProducto.Id
                                      join factura in datos.Factura_Total_Producto on facturaProducto.Id equals factura.Factura_TotalId
            
            
                                      where element.Factura_Total_Id == id
                                      select
                                          new Total {Cantidad = factura.Cantidad,Id_Producto = factura.ProductoId};
            foreach (var element in lista)
            {
                element.Precio = buscar_Precio(element.Id_Producto);
                total+= element.Cantidad*element.Precio;
            }
            return total;
        }

        private decimal buscar_Precio(int id_Producto)
        {
            decimal precio=0;
            IQueryable<Productos> lista_productos = from element in datos.Producto
                                                    where element.Id == id_Producto
                                                    select new Productos();
            foreach (var elementos in lista_productos)
            {
                precio = elementos.Precio_Unitario;
            }
            return precio;
        }


        

        public void add(Facturas _facturas)
        {
            
        }

        public List<Acarreos> Acarreos()
        {
            List<Acarreos> list =
                datos.Acarreo.Select(acarreo => new Acarreos {Id = acarreo.Id, Nombre = acarreo.Nombre}).ToList();
            return list;
        }

        public Facturas creacion()
        {
            Facturas factura=new Facturas();
            List<Productos> list = datos.Producto.Select(producto => new Productos {Descripcion = producto.Descripcion, Id = producto.Id, Codigo = producto.Codigo}).ToList();
            factura.lista_Productos=list;
            return factura;
        }


        public List<Posicion_arancelaria> Posicion_Arancelaria()
        {
            List<Posicion_arancelaria> list =
                datos.Posicion_Arancelaria.Select(acarreo => new Posicion_arancelaria { Id = acarreo.Id, Porcentaje_Taza_Estadistica = acarreo.Porcentaje_Taza_Estadistica,Numero_Posicion = acarreo.Numero_Posicion,Porcentaje_Importacion = acarreo.Porcentaje_Importacion,Porcentaje_Iva = acarreo.Porcentaje_Iva}).ToList();
            return list;
        }

        public List<Despachantes> Despacho()
        {
            List<Despachantes> list =
                datos.Despachante.Select(acarreo => new Despachantes { Id = acarreo.Id, Nombre = acarreo.Nombre }).ToList();
            return list;
        }

        public List<Depositos> Depositos()
        {
            List<Depositos> list =
                datos.Deposito.Select(acarreo => new Depositos { Id = acarreo.Id, Nombre = acarreo.Nombre }).ToList();
            return list;
        }


        public int Alta(FacturasTotales factura)
        {
            var ac = new Factura_Total
                     {
                             Id = factura.Id,
                            ProveedorId =factura.ProveedorId,
                             Vencimiento_Factura = factura.Vencimiento_Factura,
                             Numero_Factura = factura.Numero_Factura,
                             Creacion = factura.Creacion,
                             //Total = factura.Total,
                             //Cantidad_Acarreo = factura.Cantidad_Acarreo,
                             //Cantidad_Deposito = factura.Cantidad_Deposito,
                             //Cantidad_Despacho = factura.Cantidad_Despacho
                         };

            datos.Factura_Total.AddObject(ac);
            datos.SaveChanges();
            //datos.Factura_Total.Attach(ac);
            //datos.Entry(ac).State = EntityState.Added;
            //datos.SaveChanges();    
            return ac.Id;
        }

        public void Alta_Factura(Facturas factura)
        {
            var ac = new Factura
                     {
                Factura_Total_Id= factura.Factura_Total_Id,
                Envalaje = Convert.ToDecimal(factura.Envalaje),
                Fecha_Embarque = Convert.ToDateTime(factura.Flete)
                
            };
            
            datos.Factura.AddObject(ac);
            datos.SaveChanges();
            
        }

        public void Baja(int id)
        {
            datos.baja_facturacion(id);
        }


        public Facturas Detalle(int id)
        {
            var factura = from ft in datos.Factura_Total
                          join fc in datos.Factura on ft.Id equals fc.Factura_Total_Id
                          join prov in datos.Proveedor on ft.ProveedorId equals prov.Id
                          where fc.Id==id
                          select new Facturas
                                 {
                                         Id = fc.Id,
                                         Envalaje = fc.Envalaje,
                                         Factura_Total_Id = ft.Id,
                                         Flete = fc.Fecha_Embarque,
                                         ProveedorId = ft.ProveedorId,
                                         FacturasTotales = new FacturasTotales {Id = ft.Id,Creacion = ft.Creacion,Vencimiento_Factura = ft.Vencimiento_Factura,Nombre = prov.Nombre,Numero_Factura = ft.Numero_Factura,Total = 0},
                                         
                                     };
            List<Facturas> fact = factura.ToList();
            return fact[0];
        }

        private List<Productos> buscar_Producto(int id)
        {
            List<Productos> lista = (from obj in datos.Producto
                                     select
                                         new Productos
                                         {
                                                 Descripcion = obj.Descripcion,
                                                 Codigo = obj.Codigo,
                                                 Id = obj.Id,
                                                 Visible = false
                                             }).ToList();
            foreach (var elementos in lista)
            {
                elementos.Visible=elementos.Id == id;
            }
            return lista;
        }


        public void Update(Facturas factura)
        {
            int facturaId = datos.Factura.Where(X => X.Id == factura.Id).Select(X => X.Factura_Total_Id).Single();
            var fact_total = datos.Factura_Total.Single(depo => depo.Id == facturaId);
            fact_total.Numero_Factura = factura.FacturasTotales.Numero_Factura;
            fact_total.Vencimiento_Factura = factura.FacturasTotales.Vencimiento_Factura;
            fact_total.ProveedorId= factura.FacturasTotales.ProveedorId;
            fact_total.Creacion = factura.FacturasTotales.Creacion;
            datos.SaveChanges();

            var fact = datos.Factura.Single(depo => depo.Id == factura.Id);
            var listaEliminar = datos.Factura.Where(ac => ac.Factura_Total_Id== factura.Factura_Total_Id).ToList();
            var datosFp = datos.Factura_Total_Producto.Where(X => X.Factura_TotalId == factura.Id).ToList();
         
            foreach (var elementos in listaEliminar)
            {
                datos.Factura.DeleteObject(elementos);
                datos.SaveChanges();
                int id = elementos.Id;
                var lista_id_Factura=datos.Factura_Total_Producto.Where(x => x.Id == id).ToList();
                foreach (var element in lista_id_Factura)
                {
                    
                    datos.Factura_Total_Producto.DeleteObject(element);
                    datos.SaveChanges();    
                }
                datos.SaveChanges();
            }
            foreach (var item in datosFp)
            {
                datos.Factura_Total_Producto.DeleteObject(item);
                datos.SaveChanges();
            }

            foreach (var producto in factura.FacturasTotales.FacturasTotalesProductos)
            {
                fact.Fecha_Embarque = Convert.ToDateTime(factura.Flete);
                fact.Envalaje = factura.Envalaje;
                int Producto_Id = producto.Id;
                datos.SaveChanges();
                Factura_Producto(fact.Factura_Total_Id,Producto_Id,producto.Cantidad,producto.Precio_Unitario,producto.Num_Lote,producto.Id);
            }
            
        }

        private void Factura_Producto(int id_Factura, int id_producto, int cantidad,decimal precio,string lote,int id)
        {
            ConsuPyme_MVC.Factura_Total_Producto fp=new ConsuPyme_MVC.Factura_Total_Producto(){Num_Lote = lote,Precio_Unitario = precio,Cantidad = cantidad,Factura_TotalId= id_Factura,ProductoId = id_producto};
            datos.Factura_Total_Producto.AddObject(fp);
            datos.SaveChanges();
        }


        public List<Facturas_Totales_Productos> Buscar_Id_Productos(int id)
        {
            var query = datos.listado_factura_producto();
            var resul = datos.listado_factura_producto();
            var diccionario = new Dictionary<string, Facturas_Totales_Productos>();
            foreach (var elemento in query.Where(J=>J.id_factura_total!=id))
            {
                if (!diccionario.ContainsKey(elemento.Descripcion))
                {
                    var ftp=new Facturas_Totales_Productos
                            {
                        Cantidad = Convert.ToInt32("0"),
                        Num_Lote = string.Empty,
                        Precio_Unitario = Convert.ToDecimal("0"),
                        Factura_TotalId = 0,
                        productos =
                            new Productos()
                            {
                                Descripcion = elemento.Descripcion,
                                Visible = false ,
                                Codigo = elemento.Codigo,
                                Id = elemento.id_producto
                                
                            }
                    };
                    diccionario.Add(elemento.Descripcion,ftp);
                }
            }
            foreach (var elemento in resul.Where(J => J.id_factura_total == id))
            {
                if (!diccionario.ContainsKey(elemento.Codigo))
                {
                    appendProducto(id, elemento, diccionario);
                }
                else
                {
                    diccionario.Remove(elemento.Codigo);
                    appendProducto(id, elemento, diccionario);
                }
            }
            List<Facturas_Totales_Productos> lista = diccionario.Values.ToList().OrderBy(J => J.productos.Visible).ToList().AsEnumerable().ToList();
            lista.Reverse();
            return lista; 
        }

        private static void appendProducto(int id, listado_factura_producto_Result elemento, Dictionary<string, Facturas_Totales_Productos> diccionario)
        {
            if (diccionario.ContainsKey(elemento.Descripcion))
            {
                var ftp = new Facturas_Totales_Productos
                {
                    Cantidad = Convert.ToInt32(elemento.Cantidad),
                    Num_Lote = elemento.Num_Lote,
                    Precio_Unitario = Convert.ToDecimal(elemento.Precio_Unitario),
                    Factura_TotalId = elemento.id_factura_total.Value,
                    productos =
                        new Productos()
                        {
                            Descripcion = elemento.Descripcion,
                            Visible = elemento.id_factura_total.Value.Equals(id),
                            Codigo = elemento.Codigo,
                            Id = elemento.id_producto
                        }
                };
                diccionario[elemento.Descripcion]= ftp;
            }
            else
            {
                var ftp = new Facturas_Totales_Productos
                {
                    Cantidad = Convert.ToInt32(elemento.Cantidad),
                    Num_Lote = elemento.Num_Lote,
                    Precio_Unitario = Convert.ToDecimal(elemento.Precio_Unitario),
                    Factura_TotalId = elemento.id_factura_total.Value,
                    productos =
                        new Productos()
                        {
                            Descripcion = elemento.Descripcion,
                            Visible = elemento.id_factura_total.Value.Equals(id),
                            Codigo = elemento.Codigo,
                            Id = elemento.id_producto
                        }
                };
                diccionario.Add(elemento.Descripcion,ftp);
            }
        }


        public void alta_Factura_Producto(Grilla ftp)
        {
            ConsuPyme_MVC.Factura_Total_Producto fp=new ConsuPyme_MVC.Factura_Total_Producto()
                                    {Factura_TotalId = ftp.facturaId, ProductoId = Int32.Parse(ftp.id), Cantidad = int.Parse(ftp.cantidad),Precio_Unitario = decimal.Parse(ftp.precio),Num_Lote = ftp.lote};
            datos.Factura_Total_Producto.AddObject(fp);
            datos.SaveChanges();
        }


        public Facturas ObtenerGrilla(string busqueda)
        {
            List<Productos> lista =new List<Productos>();
            if(!string.IsNullOrEmpty(busqueda))
            {lista= (from d in datos.Producto //join ftp in datos.Factura_Total_Producto on d.Id equals ftp.ProductoId 
                
                where d.Descripcion.Contains(busqueda)
                select new Models.Productos() {Cantidad=0,Id = d.Id, Codigo = d.Codigo, Descripcion = d.Descripcion,Num_Lote = string.Empty,Precio_Unitario = 0,Total = 0}).ToList();}
            else
            {
                lista = (from d in datos.Producto /*join ftp in datos.Factura_Total_Producto 
                                                on d.Id equals ftp.ProductoId*/
                       select new Models.Productos() { Cantidad = 0, Num_Lote = string.Empty, Id = d.Id, Codigo = d.Codigo, Descripcion = d.Descripcion, Precio_Unitario = 0,Total = 0}).ToList();
            }
            
            var li=new Models.Facturas();
            li.lista_Productos = lista;
            return li;

        }


        public List<Proveedor> ObtenerProveedores()
        {
            return (from pro in datos.Proveedor select new Proveedor() {Id = pro.Id, Nombre = pro.Nombre}).ToList();
        }
    }
}