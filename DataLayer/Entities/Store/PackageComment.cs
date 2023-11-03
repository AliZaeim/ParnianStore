using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class PackageComment
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Name { get; set; }
        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Family { get; set; }
        [Display(Name = "تلفن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Cellphone { get; set; }
        [Display(Name = "نظر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        [Display(Name = "تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }

        [NotMapped]
        public IList<string> CommentList
        {
            get { return (Comment ?? string.Empty).Split(Environment.NewLine); }
        }
        [Display(Name = "پکیج")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int PkId { get; set; }
        #region Relations
        [ForeignKey(nameof(PkId))]
        [Display(Name = "پکیج")]
        public Package Package { get; set; }

        #endregion
    }
}
