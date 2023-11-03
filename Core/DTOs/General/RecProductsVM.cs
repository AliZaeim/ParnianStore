using DataLayer.Entities.Store;
using System.Collections.Generic;

namespace Core.DTOs.General
{
    public class RecProductsVM
    {
        public List<Product> Products { get; set; }
        public string Title { get; set; }
    }
}
