namespace ListaZakupow.Model.DataModels
{
    public class ShoppingListItem
    {
        public int ShoppingListId { get; set; }
        public int IngredientId { get; set; }
        public string Quantity { get; set; }

        public ShoppingList ShoppingList { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
