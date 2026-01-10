namespace lab1_gr1.ViewModels.IngredientVM
{
    /// <summary>
    /// ViewModel reprezentujący składnik na liście składników z dodatkowymi statystykami.
    /// </summary>
    public class IngredientListVM
    {
        /// <summary>
        /// Identyfikator składnika.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa składnika.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Liczba przepisów, w których składnik jest używany.
        /// </summary>
        public int UsedInRecipesCount { get; set; }

        /// <summary>
        /// Liczba elementów listy zakupów, w których składnik występuje.
        /// </summary>
        public int ShoppingListItemsCount { get; set; }
    }
}
