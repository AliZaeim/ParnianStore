using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class ValidateCartForPayVM
    {
        public Guid CartId { get; set; }
        public long  CartTotal { get; set; }
        public int CartFreight { get; set; }
        public string BuyerName { get; set; }
        public string BuyerFamily { get; set; }
        public long PayerId { get; set; }
        public int DiscountValue { get; set; }
        public int MyProperty { get; set; }
        public bool IsPrepare { get; set; }
        public Cart Cart { get; set; }
        public string Curr { get; set; }

    }
}
