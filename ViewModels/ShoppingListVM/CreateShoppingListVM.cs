using lab1_gr1.ViewModels.ShoppingListItemVM;

namespace lab1_gr1.ViewModels.ShoppingListVM
{
    /// <summary>
    /// ViewModel reprezentujący listę zakupów wraz z jej pozycjami.
    /// </summary>
    public class CreateShoppingListVM
    {
        /// <summary>
        /// Identyfikator listy zakupów.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Identyfikator użytkownika, do którego należy lista.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Data utworzenia listy zakupów.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Lista pozycji na liście zakupów.
        /// </summary>
        public List<CreateShoppingListItemVM> Items { get; set; } = new();
    }
}
