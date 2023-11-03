using DataLayer.Entities.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.DTOs.General
{
   public class ProductDatailsVM
    {
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int CountInCart { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="نام")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="نام خانوادگی")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Family { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="تلفن همراه")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [StringLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Cellphone { get; set; }
        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید")]
        [Display(Name ="لطفا نظر خود را وارد کنید")]
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Comment { get; set; }
        public string ProductName { get; set; }
        public string AddedComment { get; set; }
        public List<Product> GroupProducts { get; set; }

        public bool CartHasProduct { get; set; }

        

    }
}
