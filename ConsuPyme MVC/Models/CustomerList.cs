using System.Collections.Generic;

namespace ConsuPyme_MVC.Models
{
    public class CustomerList : List<Customer>
    {
        public string ImageUrl { get; set; }
    }
}