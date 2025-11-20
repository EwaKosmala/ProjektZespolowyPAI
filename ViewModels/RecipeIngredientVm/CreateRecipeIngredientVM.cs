using lab1_gr1.ViewModels.RecipeIngredientVM;
using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.RecipeIngredientVM
{
    public class CreateRecipeIngredientVM
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        [Required(ErrorMessage = "Musisz podać nazwę składniku")]
        public string IngredientName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Musisz podać ilość")]
        public string Quantity { get; set; } = string.Empty;
    }
}