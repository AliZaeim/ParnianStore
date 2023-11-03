using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataLayer.Entities.Store
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        
        [StringLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد!")]
        [Display(Name ="نوع محصول")]
        public string Kind { get; set; }
        public Guid CartId { get; set; }

        public int Quantity { get; set; }
        public int Price { get; set; }
      
        
        #region Relations
        
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        #endregion
    }
}
