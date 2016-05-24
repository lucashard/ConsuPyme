using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConsuPyme_MVC.Data
{
    public class AjaxPagingContext : DbContext
    {
        public DbSet<ConsuPyme_MVC.Models.Productos> People { get; set; }
    }
}