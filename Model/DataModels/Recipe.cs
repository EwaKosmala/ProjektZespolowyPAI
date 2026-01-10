using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje przepis użytkownika, który może zawierać składniki
    /// i być przypisany do harmonogramu tygodniowego.
    /// </summary>
    public class Recipe
    {
        /// <summary>
        /// Identyfikator przepisu (klucz główny).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identyfikator użytkownika, do którego należy przepis.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Nazwa przepisu.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Krótki opis przepisu.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Szczegółowe instrukcje przygotowania przepisu.
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// Data utworzenia przepisu.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        /// <summary>
        /// Użytkownik, do którego należy przepis.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Kolekcja powiązań przepisu ze składnikami.
        /// </summary>
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }

        /// <summary>
        /// Kolekcja harmonogramów, w których przepis jest używany.
        /// </summary>
        public ICollection<RecipeSchedule> RecipeSchedules { get; set; }
    }
}
