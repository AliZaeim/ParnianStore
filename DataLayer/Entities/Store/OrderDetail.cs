using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Store
{
    public class OrderDetail
    {
        public OrderDetail()
        {
        }
        [Key]
        public int OrderDetailId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Display(Name ="نوع محصول")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ProductKind { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string OrderDatailProductName { get; set; }
        
        [Required]
        public int OrderDetailCount { get; set; }
        [MaxLength(300)]
        public string OrderDetailComment { get; set; }
        [Required]
        public int OrderDetailPrice { get; set; }
        [Required]
        public int OrderDetailNetPrice { get; set; }
        [MaxLength(50)]
        public string OrderDetailDiscountCode { get; set; }

        public int OrderDetailDiscountValue { get; set; } = 0;
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Note { get; set; }

        public Guid Order_Id { get; set; }
        #region Relations
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey(nameof(Order_Id))]
        public Order Order { get; set; }
        #endregion

    }
}
