using lab1_gr1.ViewModels.RecipeVM;

namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    public class DayRecipesVM
    {
        public int Day { get; set; }  // 1..7
        public string DayName { get; set; } = string.Empty;

        public List<RecipeListVM> Recipes { get; set; } = new();
    }
    
}
