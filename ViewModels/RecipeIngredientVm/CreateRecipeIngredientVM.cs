using lab1_gr1.ViewModels.RecipeIngredientVM;

namespace lab1_gr1.ViewModels.RecipeIngredientVM
{
    public class CreateRecipeIngredientVM
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string Quantity { get; set; } = string.Empty;
    }
}