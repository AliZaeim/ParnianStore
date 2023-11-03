using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities.Supplementary
{
    public class State
    {
        public State()
        {
            this.Counties = new HashSet<County>();
        }
        [Key]
        public int StateId { get; set; }
        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(30, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string StateName { get; set; }
        [Display(Name = "کرایه")]
        public int? Freight { get; set; } = 200000;

        public bool IsDeleted { get; set; }
        #region Relations
        public ICollection<County> Counties { get; set; }
        #endregion



    }
}
