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
    
    public partial class Factura_Total
    {
        public Factura_Total()
        {
            this.Factura_Despacho = new HashSet<Factura_Despacho>();
            this.Factura_Total_Producto = new HashSet<Factura_Total_Producto>();
        }
    
        public string N_Orden { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public System.DateTime Creacion { get; set; }
        public string Numero_Factura { get; set; }
        public decimal Envalaje { get; set; }
        public decimal Flete { get; set; }
    
        public virtual ICollection<Factura_Despacho> Factura_Despacho { get; set; }
        public virtual ICollection<Factura_Total_Producto> Factura_Total_Producto { get; set; }
    }
}
