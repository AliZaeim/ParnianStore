using DataLayer.Entities.Store;
using System.Collections.Generic;

namespace Core.DTOs.Admin
{
    public class ProductIngredientVM
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<int> SelectedIngredients { get; set; }
        public string Title { get; set; }
    }
}
