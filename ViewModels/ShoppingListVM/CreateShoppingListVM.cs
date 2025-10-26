using lab1_gr1.ViewModels.ShoppingListItemVM;

namespace lab1_gr1.ViewModels.ShoppingListVM
{
    public class CreateShoppingListVM
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CreateShoppingListItemVM> Items { get; set; } = new();

    }
}
