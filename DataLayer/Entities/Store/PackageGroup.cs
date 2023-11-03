using DataLayer.Entities.Supplementary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class PackageGroup
    {
        public PackageGroup()
        {
            Packages = new HashSet<Package>();
        }
        [Key]
        public int PgId { get; set; }
        [Display(Name ="عنوان")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PgTitle { get; set; }
        [Display(Name ="عنوان انگلیسی")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PgEnTitle { get; set; }
        [Display(Name = "مبلغ تخفیف")]
        [DefaultValue(0)]
        public int PgDiscountValue { get; set; }
        [Display(Name = "درصد تخفیف")]
        [DefaultValue(0)]
        public float PgDiscountPercent { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(4, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [MinLength(1, ErrorMessage = "{0} نمی تواند کمتر از {1} کاراکتر باشد!")]
        [Display(Name = "علامت یا شناسه گروه")]
        public string PgMark { get; set; }
        [Display(Name ="فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "والد")]
        public int? ParentId { get; set; }
        [Display(Name = "بنــر")]
        public int? BannerId { get; set; }
        #region Relations
        [ForeignKey("ParentId")]
        [Display(Name = "والد")]
        public PackageGroup Parent { get; set; }
        [ForeignKey(nameof(BannerId))]
        [Display(Name = "بنــر")]
        public Banner Banner { get; set; }
        public ICollection<Package> Packages { get; set; }
        #endregion
    }
}
