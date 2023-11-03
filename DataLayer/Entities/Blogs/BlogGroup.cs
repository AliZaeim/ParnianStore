
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Entities.Blogs
{
    public class BlogGroup
    {
        public BlogGroup()
        {
            Blogs = new HashSet<Blog>();
        }
        [Key]
        public int BlogGroupId { get; set; }

        [Display(Name = "عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} باید {1} رقم باشد!")]
        public string BlogGroupTitle { get; set; }
        [Display(Name = "عنوان انگلیسی گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [StringLength(50, ErrorMessage = "{0} باید {1} رقم باشد!")]

        public string BlogGroupEnTitle { get; set; }
        [Display(Name = "فعال/غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "نمایش در منو")]
        public bool ShowinMenu { get; set; }
        [Display(Name = "عنوان قابل نمایش در منو")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string TitleinMenu { get; set; }
        public bool IsDeleted { get; set; }
        #region Relations
        public virtual ICollection<Blog> Blogs { get; set; }
        #endregion
    }
}
