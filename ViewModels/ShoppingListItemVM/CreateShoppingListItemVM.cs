using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.ShoppingListItemVM
{
    public class CreateShoppingListItemVM
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }

        [Required(ErrorMessage = "Muszisz podać ilość (np. 2 szt. 100g.")]
        public string Quantity { get; set; }
        public bool IsSelected { get; set; }
    }
}
