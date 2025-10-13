using System;
using System.Collections.Generic;

namespace ListaZakupow.Model.DataModels
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User User { get; set; }
        public ICollection<ShoppingListItem> Items { get; set; }
    }
}
