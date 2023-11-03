using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Admin
{
    public class DeliverLabel
    {
        [Display(Name = "نام تحویل گیرنده")]       
        public string RecipientName { get; set; }
        [Display(Name = "نام خانوادگی تحویل گیرنده")]       
        public string RecipientFamily { get; set; }
        [Display(Name = "تلفن تحویل گیرنده")]        
        public string RecipientPhone { get; set; }
        [Display(Name = "استان")]
        public string Order_StateName { get; set; }
        [Display(Name = "شهرستان")]
        public string Order_CountyName { get; set; }
        [Display(Name = "آدرس ")]
        public string Order_Address { get; set; }        
        [Display(Name = "کد پستی")]
        public string Order_PostalCode { get; set; }
        public string Order_DedicatedNumber { get; set; }
        public DateTime Order_Date { get; set; }
        public string BuyerName { get; set; }
        public string BuyerFamily { get; set; }
    }
}
