using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class OrdersExcelReportVM
    {
        
        [Display(Name = "شماره سفارش")]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "تاریخ")]
        public string Order_Date { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "استان")]
        public string Order_StateName { get; set; }
        [Required]
        [MaxLength(30)]
        [Display(Name = "شهرستان")]
        public string Order_CountyName { get; set; }
        [Display(Name = "نام خریدار")]
        [MaxLength(100)]
        public string Order_BuyerName { get; set; }
        [Display(Name = "نام خانوادگی  خریدار")]
        [MaxLength(100)]
        public string Order_BuyerFamily { get; set; }
        [Display(Name = "تلفن همراه خریدار")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Order_BuyerCellphone { get; set; }
        [Display(Name ="نام تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Order_RecipientFullName { get; set; }
        [Display(Name ="تلفن تحویل گیرنده")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Order_RecipientPhone { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "آدرس ")]
        public string Order_Address { get; set; }

        [MaxLength(100)]
        [Display(Name = "کد پستی")]
        public string Order_PostalCode { get; set; }
        [MaxLength(30)]
        [Display(Name = "کد تخفیف")]
        public string Order_DiscountCode { get; set; }
        [Display(Name = "مبلغ تخفیف")]
        public int Order_DiscountValue { get; set; }
        [Display(Name = "هزینه ارسال")]
        public int Order_DeliveryCost { get; set; }
        [Display(Name = "تخفیف هزینه ارسال")]
        public int Order_DeliveryCostDiscount { get; set; }
        [Display(Name = "جمع کل")]
        public long Order_Sum { get; set; }
        [Display(Name = "پرداخت")]
        public bool Order_IsFinished { get; set; }
        [Display(Name = "حذف")]
        public bool Order_IsDeleted { get; set; }
        [Display(Name = "لغو")]
        public bool Order_IsCanceled { get; set; }
        [Display(Name = "توضیحات عملیات لغو")]
        [MaxLength(2000, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string CancellationComment { get; set; }
        [MaxLength(100)]
        [Display(Name = "نام تجاری کاربر")]
        public string UserBusinessName { get; set; }
        [Display(Name = "درصد تجاری کاربر")]
        public float UserBusinessPercent { get; set; }
        [MaxLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        [Display(Name = "شماره اختصاصی سفارش")]
        public string Order_DedicatedNumber { get; set; }
        [Display(Name = "نحوه دریافت")]
        [MaxLength(100)]
        public string DeliveryType { get; set; }
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name = "توضیحات")]
        public string Comment { get; set; }
        [Display(Name = "تحویل به پست")]
        public bool DeliveredToPost { get; set; }
        [Display(Name = "شماره سبد خرید")]
        public string CartId { get; set; }
        [Display(Name = "تحویل به مشتری")]
        public bool IsDeliveredCutomer { get; set; }
        [Display(Name = "واحد پول")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Currency { get; set; }
        [Display(Name = "سفارش دهنده")]
        public string ClientFullName { get; set; }
        [Display(Name ="جزئیات سفارش")]
        public string OrderDetailsString { get; set; }
    }
}
