using lab1_gr1.ViewModels.RecipeVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    public interface IRecipeService
    {
        Task<Recipe?> GetByIdAsync(int id);
        Task<IEnumerable<RecipeListVM>> GetAllByUserIdAsync(int userId);
        Task<int> CreateAsync(CreateRecipeVM model, int userId);
        Task<bool> UpdateAsync(int id, CreateRecipeVM model);
        Task<bool> DeleteAsync(int id);
        Task<CreateRecipeVM?> GetForEditAsync(int id);
        Task<IEnumerable<RecipeListVM>> GetAllAsync();
        Task<IEnumerable<RecipeListVM>> FilterAsync(int userId, List<int> ingredientIds, bool showMine, bool showOthers);
    }
}