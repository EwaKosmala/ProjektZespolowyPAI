using System.ComponentModel.DataAnnotations;
using lab1_gr1.ViewModels.RecipeIngredientVM;

namespace lab1_gr1.ViewModels.RecipeVM
{
    public class CreateRecipeVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Musisz podaæ nazwê przepisu")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Musisz podaæ opis przepisu")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Musisz podaæ instrukcje przygotowania")]
        public string Instructions { get; set; }

        [MinLength(1, ErrorMessage = "Musisz dodaæ przynajmniej jeden sk³adnik")]
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