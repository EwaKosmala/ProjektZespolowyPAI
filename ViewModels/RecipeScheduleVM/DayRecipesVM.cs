using lab1_gr1.ViewModels.RecipeVM;

namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    /// <summary>
    /// ViewModel reprezentujący przepisy przypisane do jednego dnia tygodnia.
    /// </summary>
    public class DayRecipesVM
    {
        /// <summary>
        /// Numer dnia tygodnia (1 = Poniedziałek, ..., 7 = Niedziela).
        /// </summary>
        public int Day { get; set; }

        /// <summary>
        /// Nazwa dnia tygodnia (np. "Poniedziałek", "Wtorek").
        /// </summary>
        public string DayName { get; set; } = string.Empty;

        /// <summary>
        /// Lista przepisów zaplanowanych na ten dzień.
        /// </summary>
        public List<RecipeListVM> Recipes { get; set; } = new();
    }
}
