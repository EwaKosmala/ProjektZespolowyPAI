namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    /// <summary>
    /// ViewModel używany do dodawania przepisu do określonego dnia tygodnia w harmonogramie.
    /// </summary>
    public class AddRecipeToDayVM
    {
        /// <summary>
        /// Numer dnia tygodnia, do którego przypisujemy przepis (1 = Poniedziałek, ..., 7 = Niedziela).
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Identyfikator przepisu, który ma zostać dodany do harmonogramu.
        /// </summary>
        public int RecipeId { get; set; }
    }
}
