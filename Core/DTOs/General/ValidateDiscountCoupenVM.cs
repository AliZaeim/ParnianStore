using DataLayer.Entities.Store;

namespace Core.DTOs.General
{
    public class ValidateDiscountCoupenVM
    {
        public bool Validity { get; set; }
        public string Comment { get; set; }
        public DiscountCoupen DiscountCoupen { get; set; }
    }
}
