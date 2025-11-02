using lab1_gr1.ViewModels.RecipeIngredientVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    public interface IRecipeIngredientService
    {
        Task<IEnumerable<CreateRecipeIngredientVM>> GetIngredientsForRecipeAsync(int recipeId);
        Task AddIngredientToRecipeAsync(int recipeId, int ingredientId, string quantity);
        Task RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId);
    }
}