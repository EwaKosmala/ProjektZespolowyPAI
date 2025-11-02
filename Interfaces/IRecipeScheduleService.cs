using lab1_gr1.ViewModels.RecipeVM;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    public interface IRecipeScheduleService
    {
        Task<WeekVM> GetRecipesPerWeekAsync(int userId);
        Task AddRecipeToDayAsync(int userId, int recipeId, int dayOfWeek);
        Task RemoveRecipeFromDayAsync(int userId, int recipeId, int dayOfWeek);
        Task<List<RecipeListVM>> GetAvailableRecipesForUserAsync(int userId);
        Task<bool> AddExistingRecipeToScheduleAsync(int recipeId, int userId, int dayOfWeek);

    }
}