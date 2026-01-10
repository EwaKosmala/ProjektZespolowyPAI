using lab1_gr1.ViewModels.IngredientVM;

namespace lab1_gr1.ViewModels.RecipeVM
{
    /// <summary>
    /// ViewModel używany do filtrowania i wyświetlania listy przepisów.
    /// </summary>
    public class RecipeListFilterVM
    {
        // ---------------------------
        // Filtry
        // ---------------------------

        /// <summary>
        /// Lista identyfikatorów wybranych składników do filtrowania przepisów.
        /// </summary>
        public List<int> SelectedIngredientIds { get; set; } = new();

        /// <summary>
        /// Czy wyświetlać przepisy użytkownika zalogowanego.
        /// </summary>
        public bool ShowMyRecipes { get; set; } = true;

        /// <summary>
        /// Czy wyświetlać przepisy innych użytkowników.
        /// </summary>
        public bool ShowOthersRecipes { get; set; } = true;

        // ---------------------------
        // Dane do wyświetlenia
        // ---------------------------

        /// <summary>
        /// Lista przepisów spełniających wybrane filtry.
        /// </summary>
        public List<RecipeListVM> Recipes { get; set; } = new();

        /// <summary>
        /// Lista dostępnych składników do wyboru w filtrze.
        /// </summary>
        public List<IngredientListVM> AvailableIngredients { get; set; } = new();
    }
}
