using DataLayer.Entities.Store;
using System.Collections.Generic;

namespace Core.DTOs.General
{
    public class ProductsInColumnsVM
    {
        public List<Product> Products { get; set; }
        public string Title { get; set; }
        public string TitleClass { get; set; }
        public string TitleLineClass { get; set; }
        public string Link { get; set; }
        public int TotalCount { get; set; }
    }
}
