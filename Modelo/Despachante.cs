//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Modelo
{
    using System;
    using System.Collections.Generic;
    
    public partial class Despachante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public long Numero_Factura { get; set; }
        public decimal Ley { get; set; }
        public decimal Djai { get; set; }
        public decimal AD_Sim { get; set; }
        public decimal Gastos_Despacho { get; set; }
        public decimal Desconsolidado { get; set; }
        public decimal Servicios { get; set; }
        public int DespachoId { get; set; }
    
        public virtual Despacho Despacho { get; set; }
    }
}
