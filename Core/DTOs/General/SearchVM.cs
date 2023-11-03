using DataLayer.Entities.Store;
using System.Collections.Generic;

namespace Core.DTOs.General
{
    public class SearchVM
    {
        public List<Product> Products { get; set; }
        public List<Package> Packages { get; set; }
        public string Search { get; set; }
        public string Currency { get; set; }
    }
}
