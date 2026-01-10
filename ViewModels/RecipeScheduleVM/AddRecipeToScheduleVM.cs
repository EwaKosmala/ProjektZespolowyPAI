namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    /// <summary>
    /// ViewModel używany do dodawania istniejącego przepisu do harmonogramu użytkownika.
    /// </summary>
    public class AddRecipeToScheduleVM
    {
        /// <summary>
        /// Identyfikator przepisu, który ma zostać dodany.
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Nazwa przepisu (do wyświetlenia w formularzu).
        /// </summary>
        public string RecipeName { get; set; }

        /// <summary>
        /// Wybrany dzień tygodnia, do którego przypisujemy przepis (1 = Poniedziałek, ..., 7 = Niedziela).
        /// </summary>
        public int SelectedDay { get; set; }
    }
}
