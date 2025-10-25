using System.ComponentModel.DataAnnotations;

namespace lab1_gr1.ViewModels.RecipeVM
{
    public class CreateRecipeVM
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string Instructions { get; set; }

        public List<CreateRecipeIngredientVM> Ingredients { get; set; } = new List<CreateRecipeIngredientVM>();
        public List<CreateRecipeScheduleVM> Schedules { get; set; } = new();

        public int UserId { get; set; }
    }

    public class CreateRecipeIngredientVM
    {
        public string IngredientName { get; set; }
        public string Quantity { get; set; }
    }

    public class CreateRecipeScheduleVM
    {
        public int DayOfWeek { get; set; }
    }
}
