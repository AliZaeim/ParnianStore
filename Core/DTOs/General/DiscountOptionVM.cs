
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.General
{
    public class DiscountOptionVM
    {
        public bool HasDiscount { get; set; }
        public float DiscountPercent { get; set; }
        public int DiscountValue { get; set; }
        public string DiscountType { get; set; }
        public string DiscountUnit { get; set; }
        public int NetPrice { get; set; }
        public string Note { get; set; }
        [Display(Name ="مبلغ تخفیف")]
        public int DiscountAmount { get; set; }
    }
}
