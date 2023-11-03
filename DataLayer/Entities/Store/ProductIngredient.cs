using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Entities.Store
{
    public class ProductIngredient
    {
        [Key]
        public int Id { get; set; }
        [Display(Name ="محصول")]
        public int ProductId { get; set; }
        [Display(Name ="ماده تشکیل دهنده")]        
        public int IngredientId { get; set; }        
        #region Relations
        [ForeignKey(nameof(ProductId))]
        [Display(Name = "محصول")]
        public Product Product { get; set; }
        [ForeignKey(nameof(IngredientId))]
        [Display(Name = "ماده تشکیل دهنده")]
        public Ingredient Ingredient { get; set; }
        #endregion
    }
}
