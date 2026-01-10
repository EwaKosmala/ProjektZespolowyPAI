namespace lab1_gr1.ViewModels.IngredientVM
{
    /// <summary>
    /// ViewModel służący do edycji istniejącego składnika.
    /// </summary>
    public class EditIngredientVM
    {
        /// <summary>
        /// Identyfikator edytowanego składnika.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa składnika.
        /// </summary>
        public string Name { get; set; }
    }
}
