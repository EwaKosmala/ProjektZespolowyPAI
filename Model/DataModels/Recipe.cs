using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    public class Recipe
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; }
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public ICollection<RecipeSchedule> RecipeSchedules { get; set; }
    }
}
