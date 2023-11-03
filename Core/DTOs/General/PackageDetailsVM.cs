using DataLayer.Entities.Store;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.DTOs.General
{
    public class PackageDetailsVM
    {
        public Package Package { get; set; }
        public int PackageId { get; set; }
        public int CountInCart { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Family { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "تلفن همراه")]
        [RegularExpression("^[0][1-9]\\d{9}$|^[1-9]\\d{9}$", ErrorMessage = " شماره تلفن همراه نا معتبر است !")]
        [StringLength(11, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Cellphone { get; set; }
        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید")]
        [Display(Name = "لطفا نظر خود را وارد کنید")]
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Comment { get; set; }
        public string PackageName { get; set; }
        public string AddedComment { get; set; }
        public List<Package> GroupPackages { get; set; }
        public List<Product> Products { get; set; }

        public bool CartHasPackage { get; set; }


    }
}
