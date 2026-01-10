using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.RecipeIngredientVM
{
    /// <summary>
    /// ViewModel reprezentujący pojedynczy składnik przepisu przy tworzeniu przepisu.
    /// </summary>
    public class CreateRecipeIngredientVM
    {
        /// <summary>
        /// Identyfikator przepisu, do którego należy składnik.
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Identyfikator składnika.
        /// </summary>
        public int IngredientId { get; set; }

        /// <summary>
        /// Nazwa składnika.
        /// </summary>
        [Required(ErrorMessage = "Musisz podać nazwę składniku")]
        public string IngredientName { get; set; } = string.Empty;

        /// <summary>
        /// Ilość składnika w przepisie (np. "200 gr").
        /// </summary>
        [Required(ErrorMessage = "Musisz podać ilość")]
        public string Quantity { get; set; } = string.Empty;
    }
}
