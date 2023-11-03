using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class WareHouseVM
    {
        public int WId { get; set; }
        public Product  Product { get; set; }
        public Package Package { get; set; }
        
        public List<WHouse> WHouses { get; set; }

    }
    public class WHouse
    {
        public int Id { get; set; }
        public DateTime WDate { get; set; }
        public int Import { get; set; }
        public int Export { get; set; }
        public int? OD_Id { get; set; }
        public string Comment { get; set; }
        public Product Product { get; set; }
        public Package Package { get; set; }

    }
}
