using System.ComponentModel.DataAnnotations;
using lab1_gr1.ViewModels.RecipeIngredientVM;

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
        public List<int> SelectedDays { get; set; } = new();

        public int UserId { get; set; }
    }



    public class CreateRecipeScheduleVM
    {
        public int DayOfWeek { get; set; }
    }
}