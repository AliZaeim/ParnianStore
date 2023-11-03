using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Store
{
    public class WareHouse
    {
        public WareHouse()
        {

        }
        [Key]
        public int WareHouseId { get; set; }
        [Display(Name = "تاریخ")]
        
        public DateTime? WareHouse_Date { get; set; }
        
        [Display(Name = "تعداد ورود")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int WareHouse_Input { get; set; } = 0;
        [Display(Name = "تعداد خروج")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int WareHouse_Export { get; set; } = 0;
        [Display(Name ="محصول")]
        
        public int? ProductId { get; set; }
        [Display(Name ="پکیج")]
        public int? PkId { get; set; }
        [Display(Name = "جزئیات سفارش")]        
        public int? OrderdetialId { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name ="توضیحات")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string WareHouse_Comment { get; set; }
        #region Relations
        [Display(Name = "محصول")]        
        public Product Product { get; set; }
        [Display(Name ="پکیج")]
        [ForeignKey(nameof(PkId))]
        public Package Package { get; set; }
        [ForeignKey(nameof(OrderdetialId))]
        [Display(Name = "جزئیات سفارش")]        
        public OrderDetail OrderDetail { get; set; }
        #endregion

    }
}
