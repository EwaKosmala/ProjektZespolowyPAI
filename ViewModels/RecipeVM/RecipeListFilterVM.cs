using lab1_gr1.ViewModels.IngredientVM;

namespace lab1_gr1.ViewModels.RecipeVM
{
    public class RecipeListFilterVM
    {
        // Filtry
        public List<int> SelectedIngredientIds { get; set; } = new();
        public bool ShowMyRecipes { get; set; } = true;
        public bool ShowOthersRecipes { get; set; } = true;

        // Dane do wyświetlenia
        public List<RecipeListVM> Recipes { get; set; } = new();
        public List<IngredientListVM> AvailableIngredients { get; set; } = new();
    }
}
