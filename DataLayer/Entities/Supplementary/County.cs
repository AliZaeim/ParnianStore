using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace DataLayer.Entities.Supplementary
{
    public class County
    {       
        [Key]
        public int CountyId { get; set; }
        [Display(Name = "نام شهرستان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} باشد!")]
        public string CountyName { get; set; }
        [Display(Name = "نام استان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int StateId { get; set; }
        [Display(Name = "کرایه")]
        public int? Freight { get; set; } = 200000;
        public bool IsDeleted { get; set; }


        #region Relations
        [ForeignKey(nameof(StateId))]
        public State State { get; set; }
        public ICollection<User.User> Users { get; set; }


        #endregion


    }
}
