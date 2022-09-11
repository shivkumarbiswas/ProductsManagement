using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProductsManagement.Models
{
    public class ProductViewModel
    {
        public int Sr_No { get; set; }

        public int ProdCode { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string ProdName { get; set; }

        public int CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public bool Active {get; set; }

        public decimal UnitPrice { get; set; }

        public DateTime ManufactureDate { get; set; }

        public List<SelectListItem> Categories { get; set; }
    }
}
