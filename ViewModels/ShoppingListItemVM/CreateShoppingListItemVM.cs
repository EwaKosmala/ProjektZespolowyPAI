using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.ShoppingListItemVM
{
    /// <summary>
    /// ViewModel reprezentujący pojedynczy składnik w tworzeniu listy zakupów.
    /// </summary>
    public class CreateShoppingListItemVM
    {
        /// <summary>
        /// Identyfikator składnika.
        /// </summary>
        public int IngredientId { get; set; }

        /// <summary>
        /// Nazwa składnika.
        /// </summary>
        public string IngredientName { get; set; }

        /// <summary>
        /// Ilość składnika do zakupienia (np. "2 szt.", "100g").
        /// </summary>
        [Required(ErrorMessage = "Muszisz podać ilość (np. 2 szt. 100g).")]
        public string Quantity { get; set; }

        /// <summary>
        /// Flaga określająca, czy składnik został wybrany do listy zakupów.
        /// </summary>
        public bool IsSelected { get; set; }
    }
}
