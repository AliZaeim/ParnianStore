using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Store
{
    public class Product
    {
        public Product()
        {
            WareHouses = new HashSet<WareHouse>();
            ProductComments = new HashSet<ProductComment>();
            ProductIngredients = new HashSet<ProductIngredient>();
            PackageProducts = new HashSet<PackageProduct>();
        }
        [Key]
        public int ProductId { get; set; }
        [Display(Name = "کد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(6, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [MinLength(2, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        public string ProductCode { get; set; } 
        [Display(Name = "نام فارسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string ProductName { get; set; }
        [Display(Name = "نام انگلیسی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string ProductEnName { get; set; }
        [Display(Name ="موارد منع مصرف")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Contraindications { get; set; }
        [Display(Name ="عوارض جانبی")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string SideEffects { get; set; }
        [Display(Name ="تداخل دارویی")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string DrugInteractions { get; set; }
        [Display(Name ="توضیحات کوتاه")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [NotMapped]
        public IList<string> DescriptionList
        {
            get { return (Description ?? string.Empty).Split(Environment.NewLine); }
        }

        [Display(Name ="پرهیزات")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Regimens { get; set; }

        [Display(Name ="عنوان سئو صفحه محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductPageTitle { get; set; }
        [Display(Name ="شرح سئو صفحه محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(320, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductPageDescription { get; set; }
        [Display(Name = "قیمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int ProductPrice { get; set; } = 0;
        /// <summary>
        /// اطلاعات محصول
        /// </summary>
        [Display(Name = "اطلاعات محصول")]
        
        public string ProductInfoComment { get; set; }
        /// <summary>
        /// توضیحات تکمیلی
        /// </summary>
        [Display(Name = "توضیحات تکمیلی")]        
        public string ProductComment { get; set; }
        [Display(Name = "درصد تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        
        public float? ProductPercentDiscount { get; set; } = 0;
        [Display(Name = "مبلغ تخفیف")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        
        public int? ProductValueDiscount { get; set; } = 0;
        [Display(Name = "واحد شمارش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductUnit { get; set; }
        [Display(Name = "تگها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductTagsList { get; set; }
        [Display(Name ="مهمترین تگ")]        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductTopTag { get; set; }
        [Display(Name = "ویژگیها")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductFeatures { get; set; }
        [Display(Name = "مهترین ویژگی محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductTopFeature { get; set; } 
        [Display(Name = "ویژه؟")]
        public bool ProductIsFeatured { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "حداقل موجودی انبار جهت هشدار")]
        public int? ProductMinimumInventory { get; set; } = 1;
        [Display(Name = "حداکثر تعداد قابل خرید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? MaximumNumberofPurchases { get; set; } = 1;
        
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "تصویر اسلایدی، w:240 و h:300")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductListImage { get; set; }
        [Display(Name ="توضیح عکس اسلایدی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductListImageAlt { get; set; }
        [Display(Name = "تصویر، w:565 و h:690")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string ProductImage { get; set; }
        [Display(Name = "توضیح عکس اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductImageAlt { get; set; }
        [Display(Name = "تصویر بندانگشتی بلاگ")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductBlogThumb { get; set; }
        [Display(Name = "محبوبیت")]
        public int Popularity { get; set; } = 0;
        [Display(Name = "برچسب روی عکس اصلی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string ProductLabel { get; set; }
        [Display(Name = "تاریخ ایجاد")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "طریقه مصرف")]
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string HowToUse { get; set; }
        [Display(Name = "مشارکت در تخفیفهای افزایشی")]
        public bool Participate_in_IncrementalDiscounts { get; set; }
        [Display(Name = "گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int? ProductGroupId { get; set; }
        [Display(Name ="بازدید")]
        public int Views { get; set; }
        [Display(Name ="نمایش در صفحه اصلی")]

        public bool ShowinIndex { get; set; }
        [Display(Name ="وزن محصول به گرم")]
        public float Product_Weight { get; set; }
        [Display(Name ="حــذف")]
        public bool IsDeleted { get; set; }
        [Display(Name ="عدم نمایش قیمت محصول")]
        public bool NoPriceDisplay { get; set; }
        [NotMapped]
        public IList<string> ProductNameList
        {
            get { return (ProductName ?? string.Empty).Split(" "); }
        }
        [NotMapped]
        public IList<string> TagsList
        {
            get { return (ProductTagsList ?? string.Empty).Split("-"); }
        }
        [NotMapped]
        public IList<string> FeaturessList
        {
            get { return (ProductFeatures ?? string.Empty).Split("-"); }
        }
        [NotMapped]
        public IList<string> FeaturessListcompleteSplit
        {
            get { return (ProductFeatures ?? string.Empty).Split(new char[] { '-', ' ' }, StringSplitOptions.RemoveEmptyEntries); }
        }
       
        #region Relations
        [Display(Name = "گروه")]
        public ProductGroup ProductGroup { get; set; }
        
        public ICollection<WareHouse> WareHouses  { get; set; }

        public virtual ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<ProductIngredient> ProductIngredients { get; set; }
        public ICollection<PackageProduct> PackageProducts { get; set; }
        #endregion

    }
}
