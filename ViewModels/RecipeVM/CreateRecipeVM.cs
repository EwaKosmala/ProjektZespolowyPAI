using System.ComponentModel.DataAnnotations;
using lab1_gr1.ViewModels.RecipeIngredientVM;

namespace lab1_gr1.ViewModels.RecipeVM
{
    public class CreateRecipeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Musisz podać nazwę przepisu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Musisz podać opis przepisu")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Musisz podać instrukcje przygotowania")]
        public string Instructions { get; set; }

        [MinLength(1, ErrorMessage = "Musisz dodać przynajmniej jeden składnik")]
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