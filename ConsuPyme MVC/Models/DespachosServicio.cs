using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using ConsuPyme_MVC.Annotations;

namespace ConsuPyme_MVC.Models
{
    [UsedImplicitly]
    public class DespachosServicio : IDespachos
    {
        private ConsuPymeEntities1 datos = new ConsuPymeEntities1();

        public void add(Despachos despachos)
        {
            var NumberFormatInfo = new
                CultureInfo("en-US", false).NumberFormat;
            NumberFormatInfo.NumberDecimalSeparator = ",";
            Despacho despa = new Despacho
            {
                Fob_Total = Convert.ToDecimal(despachos.Fob_Total.ToString().Replace(".", ",")),
                Flete_Total = Convert.ToDecimal(despachos.Flete_Total.ToString().Replace(".", ",")),
                Dc = despachos.Dc,
                Arancel_Sim = despachos.Arancel_Sim,
                Codigo_Afip = despachos.Codigo_Afip,
                //Costo_despacho_producto = despachos.Costo_despacho_producto,
                Cotizacion = Convert.ToDecimal(despachos.Cotizacion.ToString().Replace(".", ",")),
                //ProductoId = despachos.Producto_Id,
                Seguro_Total = Convert.ToDecimal(despachos.Seguro_Total.ToString().Replace(".", ",")),
                Servicio_Guarda = Convert.ToDecimal(despachos.Servicio_Guarda.ToString().Replace(".", ",")),
                Oficializacion = despachos.Oficializacion,
                Tipo_CambioId = despachos.TipoCambioId,
                Gasto_Aduana = despachos.Gasto_Aduanero,
                Multa = despachos.Multa,
                Derechos = despachos.DerechosImportacion,
                Taza_Estadistica = despachos.TazaEstadistica
                //Derechos = despachos.im
            };
            datos.Despacho.AddObject(despa);
            datos.SaveChanges();
            foreach (var element in despachos.Lista_Factura)
            {
                int id = (from f in datos.Factura where f.Factura_Total_Id == element select f.Id).Single();

                Factura_Despacho facturaDespacho = new Factura_Despacho
                {
                    DespachoId = despa.Id,
                    FacturaId = id
                };
                datos.Factura_Despacho.AddObject(facturaDespacho);
                datos.SaveChanges();
            }
        }

        public Despachos grid()
        {
            var despachosDB = datos.Despacho.ToList();
            var lista_Acarreos =
                despachosDB.Select(
                    despachos =>
                        new Despachos
                        {
                            Id = despachos.Id,
                            Fob_Total = despachos.Fob_Total,
                            Flete_Total = despachos.Flete_Total,
                            Dc = despachos.Dc,
                            Arancel_Sim = despachos.Arancel_Sim,
                            Codigo_Afip = despachos.Codigo_Afip,
                            //Costo_despacho_producto = despachos.Costo_despacho_producto,
                            Cotizacion = despachos.Cotizacion,
                            Seguro_Total = despachos.Seguro_Total,
                            Servicio_Guarda = despachos.Servicio_Guarda,
                            Oficializacion = despachos.Oficializacion,
                        }).ToList();
            Dictionary<string, Despachos> dictionary = new Dictionary<string, Despachos>();
            foreach (var element in lista_Acarreos)
            {
                if (!dictionary.ContainsKey(element.Dc))
                {
                    dictionary[element.Dc] = new Despachos
                    {
                        Id = element.Id,
                        Fob_Total = element.Fob_Total,
                        Flete_Total = element.Flete_Total,
                        Dc = element.Dc,
                        Arancel_Sim = element.Arancel_Sim,
                        Codigo_Afip = element.Codigo_Afip,
                        //Costo_despacho_producto = element.Costo_despacho_producto,
                        Cotizacion = element.Cotizacion,
                        Producto_Id = element.Producto_Id,
                        Seguro_Total = element.Seguro_Total,
                        Servicio_Guarda = element.Servicio_Guarda,
                        Oficializacion = element.Oficializacion
                    };
                }
            }
            Despachos ac = new Despachos {Diccionario_Index = dictionary};
            return ac;
        }

        public List<TipoCambio> buscarTipoCambio()
        {
            var tipo = (from tp in datos.Tipo_Cambio select new TipoCambio() {Id = tp.Id, Cambio = tp.Cambio}).ToList();
            return tipo;
        }

        public List<Productos> Productos(List<int> productoId)
        {
            List<Productos> lista_Productos = new List<Productos>();
            foreach (var elemnt in productoId)
            {
                int Id = elemnt;
                var elemento_A_agregar = from f in datos.Factura
                    join ft in datos.Factura_Total on f.Factura_Total_Id equals ft.Id
                    join p in datos.Factura_Total_Producto on ft.Id equals p.Factura_TotalId
                    join produ in datos.Producto on p.ProductoId equals produ.Id
                    where ft.Id == Id
                    select
                        new Productos
                        {
                            Id = produ.Id,
                            Codigo = produ.Codigo,
                            Descripcion = produ.Descripcion,
                            Posicion_Arancelaria_Id = produ.Posicion_Arancelaria.Id
                        };
                lista_Productos.AddRange(elemento_A_agregar);
            }
            return lista_Productos.ToList();
        }

        public Despachos Editar(int id)
        {
            var editar_Despacho = (from despa in datos.Despacho
                join cam in datos.Tipo_Cambio on despa.Tipo_CambioId equals cam.Id
                where despa.Id == id
                select
                    new Despachos
                    {
                        Id = despa.Id,
                        //Producto_Id = despa.ProductoId,
                        Dc = despa.Dc,
                        Arancel_Sim = despa.Arancel_Sim,
                        Codigo_Afip = despa.Codigo_Afip,
                        Cotizacion = despa.Cotizacion,
                        //Costo_despacho_producto = despa.Costo_despacho_producto,
                        Flete_Total = despa.Flete_Total,
                        Fob_Total = despa.Fob_Total,
                        Gasto_Aduanero = despa.Gasto_Aduana,
                        Oficializacion = despa.Oficializacion,
                        Seguro_Total = despa.Seguro_Total,
                        Servicio_Guarda = despa.Servicio_Guarda,
                        TipoCambioId = cam.Id,
                        Multa = despa.Multa,
                        DerechosImportacion = despa.Derechos,
                        TazaEstadistica = despa.Taza_Estadistica
                    });
            return editar_Despacho.SingleOrDefault();
        }

        public void update(Despachos o, List<string> lista)
        {
            if (datos.Factura_Despacho.Where(J => J.DespachoId == o.Id).ToList().Any())
            {
                var elimi = datos.Factura_Despacho.Single(J => J.DespachoId == o.Id);
                datos.Factura_Despacho.DeleteObject(elimi);
                datos.SaveChanges();
            }


            foreach (var elemCrear in lista)
            {
                int id = Convert.ToInt32(elemCrear);
                var objCrear = new Factura_Despacho() {DespachoId = o.Id, FacturaId = id};
                datos.Factura_Despacho.AddObject(objCrear);
                datos.SaveChanges();
            }

            var objEditar =
                (from d in datos.Despacho where d.Id == o.Id select d).
                    SingleOrDefault();


            objEditar.Arancel_Sim = o.Arancel_Sim;
            objEditar.Codigo_Afip = o.Codigo_Afip;
            objEditar.Cotizacion = o.Cotizacion;
            objEditar.Dc = o.Dc;
            objEditar.Flete_Total = o.Flete_Total;
            objEditar.Fob_Total = o.Fob_Total;
            objEditar.Oficializacion = o.Oficializacion;
            objEditar.Seguro_Total = o.Seguro_Total;
            objEditar.Servicio_Guarda = o.Servicio_Guarda;
            objEditar.Tipo_CambioId = o.TipoCambioId;
            objEditar.Multa = o.Multa;
            objEditar.Derechos = o.DerechosImportacion;
            objEditar.Taza_Estadistica = o.TazaEstadistica;
            datos.SaveChanges();
        }

        public void borrar(string id)
        {
            Int32 id1 = Convert.ToInt32(id);
            datos.baja_despachos(id1);
            datos.SaveChanges();
        }

        public List<FacturasTotales> Facturas(string busqueda)
        {
            var lista_Productos = datos.listado_Facturas2(busqueda).ToList();
            List<FacturasTotales> lista =
                lista_Productos.Select(
                    o =>
                        new FacturasTotales
                        {
                            Id = o.Id,
                            Numero_Factura = o.Numero_Factura,
                            Nombre = o.Nombre,
                            Vencimiento_Factura = o.Vencimiento_Factura,
                            Creacion = o.Creacion,
                            Visible = false
                        }).ToList();

            return lista;
        }

        public Dictionary<string, FacturasTotales> Facturas(int id, List<FacturasTotales> listaCompleta)
        {
            //var lista_Productos = datos.Factura_Total.ToList();
            var facturas = new Dictionary<string, FacturasTotales>();
            return facturas;
            //TODO VERIFICAR
            //List<FacturasTotales> lista_Facturas = (from despa in datos.Despacho
            //                       join pro in datos.Producto on despa.ProductoId equals pro.Id
            //                       join factu in datos.Factura_Total_Producto on pro.Id equals factu.ProductoId
            //                       join facturatotal in datos.Factura_Total on factu.Id equals facturatotal.Id
            //                       where despa.Id == id
            //                       select
            //                           new FacturasTotales
            //                           {
            //                               Id = facturatotal.Id,
            //                               Numero_Factura = facturatotal.Numero_Factura,
            //                               Nombre = facturatotal.Nombre,
            //                               N_Orden = facturatotal.N_Orden,
            //                               Creacion = facturatotal.Creacion
            //                               }).ToList();
            //foreach (var elemento in listaCompleta)
            //{
            //    foreach (var imparcial in lista_Facturas)
            //    {
            //        if (elemento.Nombre.Equals(imparcial.Nombre))
            //        {
            //            elemento.Visible = true;
            //            break;
            //        }
            //    }
            //    if (!facturas.ContainsKey(elemento.Nombre))
            //    {
            //        facturas[elemento.Nombre]=new FacturasTotales {Id = elemento.Id,N_Orden = elemento.N_Orden,Creacion = elemento.Creacion,Numero_Factura = elemento.Numero_Factura,Nombre = elemento.Nombre,Visible = elemento.Visible};
            //    }
            //}
            ////Despachos ac = new Despachos { Diccionario_Facturas = facturas };

            //return facturas;
        }


        public List<Facturas> verFacturas(int id)
        {
            var query =
                (from fact in datos.Factura
                    join ft in datos.Factura_Total on fact.Factura_Total_Id equals ft.Id
                    join prov in datos.Proveedor on ft.ProveedorId equals prov.Id
                    //join fd in datos.Factura_Despacho on fact.Id equals fd.FacturaId
                    select new Facturas()
                    {
                        Id = fact.Id, //fd.Id,
                        Flete = fact.Fecha_Embarque,
                        Envalaje = fact.Envalaje,
                        FacturasTotales =
                            new FacturasTotales()
                            {
                                Nombre = prov.Nombre,
                                Creacion = ft.Creacion,
                                Numero_Factura = ft.Numero_Factura,
                                Vencimiento_Factura = ft.Vencimiento_Factura
                            },
                        Visible = false
                    }).ToList();

            foreach (Facturas ele in query)
            {
                var verFactura =
                    (from fd in datos.Factura_Despacho where fd.DespachoId == id select fd).ToList();
                if (verFactura.Any())
                {
                    foreach (Factura_Despacho fd in verFactura)
                    {
                        if (fd.FacturaId == ele.Id)
                            ele.Visible = true;
                    }
                }
            }
            return query;
        }
    }
}