namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    /// <summary>
    /// ViewModel reprezentujący cały tydzień przepisów.
    /// </summary>
    public class WeekVM
    {
        /// <summary>
        /// Lista dni tygodnia wraz z przypisanymi do nich przepisami.
        /// Każdy element listy jest instancją <see cref="DayRecipesVM"/>.
        /// </summary>
        public List<DayRecipesVM> Days { get; set; } = new();
    }
}
