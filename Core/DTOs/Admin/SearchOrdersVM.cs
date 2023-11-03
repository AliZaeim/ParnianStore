using DataLayer.Entities.Store;
using DataLayer.Entities.Supplementary;
using DataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class SearchOrdersVM
    {
        [Display(Name ="از تاریخ")]
        [RegularExpression(@"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|31|([1-2][0-9])|(0[1-9]))))$", ErrorMessage ="تاریخ نامعتبر است !")]
        public string StartDate { get; set; }
        [Display(Name ="تا تاریخ")]
        [RegularExpression(@"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|31|([1-2][0-9])|(0[1-9]))))$", ErrorMessage = "تاریخ نامعتبر است !")]
        public string EndDate { get; set; }
        
        [Display(Name ="نام خریدار")]
        public string BuyerName { get; set; }
        [Display(Name = "فامیلی خریدار")]
        public string BuyerFamily { get; set; }
        [Display(Name ="تلفن همراه خریدار")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        public string BuyerCellphone { get; set; }
        [Display(Name ="کد پستی")]
        public string PostalCode { get; set; }
        [Display(Name ="وضعیت پرداخت")]
        public bool? IsFinished { get; set; }
        [Display(Name ="وضعیت لغو")]
        public bool? IsCancled { get; set; }
        [Display(Name ="تحویل به پست")]
        public bool? DeliverdToPost { get; set; }
        [Display(Name ="تحویل به مشتری")]
        public bool? DeliveredToCustomer { get; set; }
        [Display(Name ="دارای کوپن تخفیف")]
        public bool? HasDicCoupen { get; set; }
        [Display(Name ="کوپن تخفیف")]
        public string DisCoupen { get; set; }
        [Display(Name ="شماره اختصاصی سفارش")]
        public string DedicatedNumber { get; set; }
        [Display(Name ="کاربر سفارش دهنده")]
        public int? URId { get; set; }
        [Display(Name = "تعداد در صفحه")]
        public int RecPerPage { get; set; } = 20;
        [Display(Name = "صفحه")]
        public int PageN { get; set; } = 1;
        public int TotalRecCount { get; set; }
        public long TotalOrdersSum { get; set; }
        public int UnPaidCount { get; set; }
        public long UnPaidSum { get; set; }
        public int PaidCount { get; set; }
        public long PaidSum { get; set; }
        public List<Order> Orders { get; set; }
        public List<UserRole> UserRoles { get; set; }

        public string ReportTitle { get; set; }



    }
}
