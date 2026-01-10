using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje składnik, który może być użyty w przepisach i listach zakupów.
    /// </summary>
    public class Ingredient
    {
        /// <summary>
        /// Identyfikator składnika (klucz główny).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa składnika.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kolekcja powiązań składnika z przepisami.
        /// </summary>
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        /// <summary>
        /// Kolekcja elementów list zakupów, w których składnik się znajduje.
        /// </summary>
        public ICollection<ShoppingListItem> ShoppingListItems { get; set; }
    }
}
