using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje użytkownika aplikacji, który może posiadać przepisy,
    /// harmonogramy przepisów i listy zakupów.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identyfikator użytkownika (klucz główny).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa użytkownika (login).
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Hash hasła użytkownika.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Data rejestracji użytkownika w systemie.
        /// </summary>
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Kolekcja przepisów stworzonych przez użytkownika.
        /// </summary>
        public ICollection<Recipe> Recipes { get; set; }

        /// <summary>
        /// Kolekcja harmonogramów przepisów użytkownika.
        /// </summary>
        public ICollection<RecipeSchedule> RecipeSchedules { get; set; }

        /// <summary>
        /// Kolekcja list zakupów użytkownika.
        /// </summary>
        public ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
