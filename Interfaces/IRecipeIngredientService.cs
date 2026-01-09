using lab1_gr1.ViewModels.RecipeIngredientVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu obsługującego relację
    /// między przepisami a składnikami (wiele-do-wielu).
    /// </summary>
    public interface IRecipeIngredientService
    {
        /// <summary>
        /// Pobiera składniki przypisane do wskazanego przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <returns>
        /// Kolekcja widoków składników przypisanych do przepisu 
        /// (<see cref="CreateRecipeIngredientVM"/>)
        /// </returns>
        Task<IEnumerable<CreateRecipeIngredientVM>> GetIngredientsForRecipeAsync(int recipeId);

        /// <summary>
        /// Dodaje składnik do przepisu z określoną ilością.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="ingredientId">Identyfikator składnika</param>
        /// <param name="quantity">Ilość składnika w przepisie</param>
        /// <returns>Task reprezentujący operację asynchroniczną</returns>
        Task AddIngredientToRecipeAsync(int recipeId, int ingredientId, string quantity);

        /// <summary>
        /// Usuwa składnik z przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="ingredientId">Identyfikator składnika do usunięcia</param>
        /// <returns>Task reprezentujący operację asynchroniczną</returns>
        Task RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId);
    }
}
