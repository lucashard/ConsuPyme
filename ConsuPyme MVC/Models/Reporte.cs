using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Models
{
    public class Reporte
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string producto { get; set; }
        public string valor { get; set; }
    }
}