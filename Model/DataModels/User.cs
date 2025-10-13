using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        public ICollection<Recipe> Recipes { get; set; }
        public ICollection<RecipeSchedule> RecipeSchedules { get; set; }
        public ICollection<ShoppingList> ShoppingLists { get; set; }
    }
}
