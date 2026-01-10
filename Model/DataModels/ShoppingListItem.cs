namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje pojedynczą pozycję na liście zakupów, powiązaną ze składnikiem i jego ilością.
    /// </summary>
    public class ShoppingListItem
    {
        /// <summary>
        /// Identyfikator listy zakupów, do której należy pozycja.
        /// </summary>
        public int ShoppingListId { get; set; }

        /// <summary>
        /// Identyfikator składnika przypisanego do pozycji na liście zakupów.
        /// </summary>
        public int IngredientId { get; set; }

        /// <summary>
        /// Ilość składnika na liście zakupów (np. "2 szklanki", "3 łyżki").
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Lista zakupów, do której należy pozycja.
        /// </summary>
        public ShoppingList ShoppingList { get; set; }

        /// <summary>
        /// Składnik przypisany do pozycji na liście zakupów.
        /// </summary>
        public Ingredient Ingredient { get; set; }
    }
}
