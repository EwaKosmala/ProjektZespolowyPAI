namespace lab1_gr1.ViewModels.RecipeScheduleVM
{
    public class AddRecipeToDayVM
    {
        public int DayOfWeek { get; set; }    // 1..7
        public int RecipeId { get; set; }     // wybrany przepis
    }
}
