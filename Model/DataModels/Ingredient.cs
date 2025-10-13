using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}
