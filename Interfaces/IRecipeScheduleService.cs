using lab1_gr1.ViewModels.RecipeVM;
using lab1_gr1.ViewModels.RecipeScheduleVM;

namespace lab1_gr1.Interfaces
{
    public interface IRecipeScheduleService
    {
        Task<List<DayRecipesVM>> GetRecipesPerWeekAsync(int userId);

    }
}
