using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListaZakupow.Model.DataModels
{
    /// <summary>
    /// Reprezentuje powiązanie między przepisem a składnikiem (relacja wiele-do-wielu)
    /// wraz z określoną ilością składnika w przepisie.
    /// </summary>
    public class RecipeIngredient
    {
        /// <summary>
        /// Identyfikator przepisu.
        /// </summary>
        public int RecipeId { get; set; }

        /// <summary>
        /// Identyfikator składnika.
        /// </summary>
        public int IngredientId { get; set; }

        /// <summary>
        /// Ilość składnika w przepisie (np. "2 szklanki", "3 łyżki").
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// Przepis, do którego przypisany jest składnik.
        /// </summary>
        public Recipe Recipe { get; set; }

        /// <summary>
        /// Składnik przypisany do przepisu.
        /// </summary>
        public Ingredient Ingredient { get; set; }
    }
}
