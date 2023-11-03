using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class Package
    {
        public Package()
        {
            PackageProducts = new HashSet<PackageProduct>();
            WareHouses = new HashSet<WareHouse>();
        }
        [Key]
        public int PkId { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name ="نام")]
        public string PkTitle { get; set; }
        [Display(Name ="نام انگلیسی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkEnTitle { get; set; }
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name ="ویژگی")]
        public string PkFeature { get; set; }
        [Display(Name ="کد پکیج")]
        [StringLength(20, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkCode { get; set; }
        [Display(Name ="تصویر اسلایدی | عرض 240، ارتفاع 300")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkSliderImage { get; set; }
        [Display(Name ="توضیحات تصویر اسلایدی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkSliderImage_Alt { get; set; }
        [Display(Name ="تصویر | عرض 565، ارتفاع 690")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkImage { get; set; }
        [Display(Name ="توضیحات تصویر")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkImage_Alt { get; set; }
        [Display(Name ="شرح کوتاه")]
        [StringLength(600, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkAbstract { get; set; }
        [Display(Name ="توضیحات")]        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkComment { get; set; }
        
        [Display(Name ="برچسب ها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkTags { get; set; }
        [Display(Name ="طریقه مصرف")]
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkHowToUse { get; set; }
        [Display(Name ="قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PkPrice { get; set; }
        [Display(Name ="واحد شمارش")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PkUnit { get; set; }
        [Display(Name ="حداقل موجودی جهت هشدار")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PkMin_Inventory_ForAlarm { get; set; }
        [Display(Name ="برچسب روی عکس")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string PkLabel { get; set; }
        [Display(Name ="وزن به گرم")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PkWeight { get; set; }
        [Display(Name ="تخفیف مبلغی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PkDiscountValue { get; set; }
        [Display(Name = "تخفیف درصدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]        
        public float? PkDiscountPercent { get; set; } = 0f;
        [Display(Name ="متا تگ دسکریپشن")]
        [StringLength(320, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Pk_SeoDescription { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "حداکثر تعداد قابل خرید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? MaximumNumberofPurchases { get; set; } = 1;
        [Display(Name = "محبوبیت")]
        public int Popularity { get; set; } = 0;
        [Display(Name = "تاریخ ایجاد")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "بازدید")]
        public int Views { get; set; }
        [Display(Name = "گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? PgId { get; set; }
        #region Relations
        [ForeignKey(nameof(PgId))]
        [Display(Name = "گروه")]
        public PackageGroup PackageGroup { get; set; }
        public ICollection<PackageProduct> PackageProducts { get; set; }
        public ICollection<WareHouse> WareHouses { get; set; }
        public ICollection<PackageComment> PackageComments { get; set; }
        #endregion
        [NotMapped]
        public IList<string> TagsList
        {
            get { return (PkTags ?? string.Empty).Split("-"); }
        }
        [NotMapped]
        public string FullPro
        {
            get { return PkTitle + " " + PkFeature; }
        }
    }
}
