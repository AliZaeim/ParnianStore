using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Supplementary
{
    public class AboutUsfaq
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="پرسش")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Question { get; set; }
        [Display(Name ="پاسخ")]
        [StringLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Answer { get; set; }
        [Display(Name ="فعال / غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name ="تاریخ ثبت")]
        public DateTime? CreatedDate { get; set; }

        [NotMapped]
        public IList<string> AnswerLines
        {
            get { return (Answer ?? string.Empty).Split(Environment.NewLine); }
        }

    }
}
