using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ProductsManagement.Models
{
    public class ListProductViewModel
    {
        public List<ProductViewModel> Products { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public int CategoryCode { get; set; }
    }
}
