using DataLayer.Entities.Supplementary;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities.Store
{
    public class ProductGroup
    {
        public ProductGroup()
        {           
            Products = new HashSet<Product>();
        }
        [Key]
        public int ProductGroupId { get; set; }
        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductGroupTitle { get; set; }
        [Display(Name = "عنوان انگلیسی گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductEnGroupTitle { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(500, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string ProductGroupComment { get; set; }
        [Display(Name = "درصد تخفیف")]
        [DefaultValue(0)]
        public float ProductGroupDiscountPercent { get; set; } = 0;
        [Display(Name = "مبلغ تخفیف")]
        [DefaultValue(0)]
        public int ProductGroupDiscountValue { get; set; } = 0;
        [Display(Name = "تصویر")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string ProductGroupImage { get; set; }
        [Display(Name = "متن نمایشی")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string ProductGroupText { get; set; }
        
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]        
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [MinLength(1, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        [Display(Name = "علامت یا شناسه گروه")]
        public string ProductGroupMark { get; set; }
        
        [Display(Name ="نمایش اسلایدر در صفحه اصلی")]
        public bool ShowinMainPage { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        [NotMapped]
        public IList<string> GroupTitleList
        {
            get { return (ProductGroupTitle ?? string.Empty).Split(" "); }
        }
        [NotMapped]
        public string FullPro
        {
            get
            {
                if(Parent == null)
                {
                    return string.Format("{0} {1}","گروه ", ProductGroupTitle);
                }
                else
                {
                    return string.Format("{0} {1}","زیرگروه ", ProductGroupTitle);
                }
               
            }
        }
        
        [Display(Name = "والد")]
        public int? ParentId { get; set; }
        [Display(Name ="بنــر")]
        public int? BannerId { get; set; }
        #region Relations  
        [ForeignKey("ParentId")]
        [Display(Name = "والد")]
        public ProductGroup Parent { get; set; }
        [ForeignKey(nameof(BannerId))]
        [Display(Name = "بنــر")]
        public Banner Banner { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        
        //[ForeignKey(nameof(BannerId))]
        //public Banner Baner { get; set; }
        #endregion
    }
}
