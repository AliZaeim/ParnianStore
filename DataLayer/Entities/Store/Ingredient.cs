using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class Ingredient
    {
        public Ingredient()
        {
            ProductIngredients = new HashSet<ProductIngredient>();
        }
        [Key]
        public int IngredientId { get; set; }
        [Display(Name ="نــام")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name ="تصویر")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        public string Image { get; set; }
        [Display(Name ="توضیحات")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Comment { get; set; }
        [Display(Name ="طبع")]
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Nature { get; set; }
        [Display(Name ="مواد شیمیایی")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ChemicalsMeterials { get; set; }
        [Display(Name ="خواص دارویی")]
        [StringLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string MedicinalProperties { get; set; }
        [Display(Name ="توصیه ها")]
        [StringLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Recommendations { get; set; }

        [NotMapped]
        public IList<string> ChemicalsMeterialsList
        {
            get { return (ChemicalsMeterials ?? string.Empty).Split("-"); }
        }
        [NotMapped]
        public IList<string> MedicinalPropertiesList
        {
            get { return (MedicinalProperties ?? string.Empty).Split("-"); }
        }
        [NotMapped]
        public IList<string> RecommendationsList
        {
            get { return (Recommendations ?? string.Empty).Split("-"); }
        }
        #region Relations
        public ICollection<ProductIngredient> ProductIngredients { get; set; }
        #endregion
    }
}
