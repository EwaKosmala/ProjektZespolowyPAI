namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje harmonogram przypisania przepisu do konkretnego dnia tygodnia dla użytkownika.
    /// </summary>
    public class RecipeSchedule
    {
        /// <summary>
        /// Identyfikator harmonogramu (klucz główny).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identyfikator użytkownika, do którego należy harmonogram.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Identyfikator przepisu przypisanego do harmonogramu.
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Dzień tygodnia, w którym przepis jest zaplanowany.
        /// 1 = Poniedziałek, ..., 7 = Niedziela
        /// </summary>
        public int DayOfWeek { get; set; }

        /// <summary>
        /// Użytkownik, do którego należy harmonogram.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Przepis przypisany do harmonogramu.
        /// </summary>
        public Recipe Recipe { get; set; }
    }
}
