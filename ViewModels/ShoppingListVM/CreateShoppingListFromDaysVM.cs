namespace lab1_gr1.ViewModels.ShoppingListVM
{
    /// <summary>
    /// ViewModel używany do tworzenia listy zakupów na podstawie wybranych dni tygodnia.
    /// </summary>
    public class CreateShoppingListFromDaysVM
    {
        /// <summary>
        /// Lista wybranych dni tygodnia (1 = poniedziałek, ..., 7 = niedziela),
        /// dla których mają zostać wygenerowane składniki do listy zakupów.
        /// </summary>
        public List<int> SelectedDays { get; set; } = new();
    }
}
