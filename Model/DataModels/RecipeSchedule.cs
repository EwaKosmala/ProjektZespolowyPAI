namespace ListaZakupow.Model.DataModels
{
    public class RecipeSchedule
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RecipeId { get; set; }
        public int DayOfWeek { get; set; }  // 1=Poniedzialek,..,7=Niedziela

        public User User { get; set; }
        public Recipe Recipe { get; set; }
    }
}
