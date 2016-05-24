using System;
using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
   
    public interface Ireporte
    {
        List<Reporte> ObtenerNombreDespacho();
        List<ListadoProductoReporte> ObtenerReporteDespacho(int id);
        List<ListadoProductoReporte> ObtenerReporteDespacho(DateTime? fechaDesde, DateTime? fechaHasta);
    }
}
