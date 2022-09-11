using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsManagement.Repository.Models
{
    public class Product
    {
        public int ProductCode { get; set; }

        public string Name { get; set; }

        public Category Category { get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime ManufactureDate { get; set; }

        public bool Active { get; set;}

        public DateTime AddedDate { get; set; }

        public DateTime? LastEditedDate { get; set; }
    }
}
