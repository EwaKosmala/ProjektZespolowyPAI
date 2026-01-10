using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje listę zakupów użytkownika.
    /// </summary>
    public class ShoppingList
    {
        /// <summary>
        /// Identyfikator listy zakupów (klucz główny).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identyfikator użytkownika, do którego należy lista zakupów.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Data utworzenia listy zakupów.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Użytkownik, do którego należy lista zakupów.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Kolekcja pozycji na liście zakupów.
        /// </summary>
        public ICollection<ShoppingListItem> Items { get; set; }
    }
}
