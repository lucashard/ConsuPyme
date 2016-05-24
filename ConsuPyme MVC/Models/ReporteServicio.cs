using System;
using System.Collections.Generic;
using System.Linq;
using LeerDolar;

namespace ConsuPyme_MVC.Models
{
    public class ReporteServicio:Ireporte
    {
        private readonly ConsuPymeEntities1 datos = new ConsuPymeEntities1();
        public List<Reporte> ObtenerNombreDespacho()
        {
            var dato=datos.Ver_Despacho();
            return dato.Select(elemento => new Reporte() {Id = elemento.id, Nombre = elemento.Nombre}).ToList();
        }

        public List<ListadoProductoReporte> ObtenerReporteDespacho(int id)
        {
            decimal dolares = datos.ver_despacho_fecha(id).Select(X=>X.cotizacion).Single();
            //Dictionary<DateTime, double> dolar = new ReportManager().generar(fecha);
            //var dolares=Convert.ToDecimal(dolar.First().Value);
            return Reporte(id, dolares);
        }

        private List<ListadoProductoReporte> Reporte(int id, decimal dolares)
        {
            var de = datos.cantidad_Producto(id, dolares).ToList();
            return
                de.Select(
                    elemento =>
                        new ListadoProductoReporte()
                        {
                            Nombre = elemento.producto,
                            Precio = Convert.ToDecimal(elemento.total.ToString()),
                            NumeroFactura = elemento.Nombre,
                            PrecioDolar = dolares
                        }).ToList();
        }

        public  List<ListadoProductoReporte> ObtenerReporteDespacho(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            List<producto_Fechas_Result> listado=new List<producto_Fechas_Result>(datos.producto_Fechas(fechaDesde, fechaHasta));
            Dictionary<DateTime, DolarFacturas> dolar=new Dictionary<DateTime, DolarFacturas>();
            List<ListadoProductoReporte> listaResultante = new List<ListadoProductoReporte>();
            foreach (producto_Fechas_Result elem in listado)
            {
                DateTime fecha = elem.Creacion;
                var fechaDolar = new ReportManager().generar(fecha);
                foreach (var elemento in fechaDolar)
                {
                    dolar.Add(elemento.Key,new DolarFacturas(){Dolar = Convert.ToDecimal(elemento.Value),Id = elem.Id});
                }
            }
            foreach (var elemento in dolar)
            {
                var resultadoReporte=Reporte(elemento.Value.Id, elemento.Value.Dolar);
                var listadoProductoReportes = resultadoReporte.Select(elem => new ListadoProductoReporte() {Nombre = elem.Nombre, Precio = elem.Precio, PrecioDolar = elemento.Value.Dolar}).ToList();
                int id1 = elemento.Value.Id;
              //  string factura = datos.Factura_Total.Where(x => x.Id == id1).Select(x => x.Nombre).Single();
                string factura = (from element in datos.Factura_Total
                    join prov in datos.Proveedor on element.ProveedorId equals prov.Id select prov.Nombre).ToString();
                foreach (ListadoProductoReporte elem in listadoProductoReportes)
                {
                    elem.NumeroFactura = factura;
                }
                listaResultante.AddRange(listadoProductoReportes );
            }
            return listaResultante;
        }
    }
}