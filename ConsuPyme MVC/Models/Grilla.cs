using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Models
{
    public class Grilla : IEquatable<Grilla>
    {
        public int facturaId { get; set; }
        public String id { get; set; }
        public String lote { get; set; }
        public String precio { get; set; }
        public String cantidad { get; set; }
        public String Codigo { get; set; }
        public String Descripcion { get; set; }

        public bool Equals(Grilla other)
        {
            return this.id == other.id;
        }
    }
}