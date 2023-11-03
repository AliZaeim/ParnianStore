using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.General
{
    public class PaymentResultVM
    {
        [Display(Name ="نام بانک")]
        public string BankName { get; set; }
        [Display(Name ="وضعیت پرداخت")]
        public bool PaymentStatus { get; set; }
        [Display(Name ="پیام")]
        public string PaymentMessage { get; set; }
        [Display(Name ="کد رهگیری")]
        public string SaleReferenceId { get; set; }

        public Order Order { get; set; }

    }
}
