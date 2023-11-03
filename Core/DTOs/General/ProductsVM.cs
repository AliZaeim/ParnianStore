using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTOs.General
{
    public class ProductsVM
    {
        public List<Product> Products { get; set; }
        public List<ProductGroup> ProductGroups { get; set; }
        public int? CurrentPoductGroupId { get; set; }
        public ProductGroup CurrentProductGroup { get; set; }
       
        public bool AllP { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int RecPerPage { get; set; }
    }
}
