using System.ComponentModel.DataAnnotations;
using lab1_gr1.ViewModels.RecipeIngredientVM;

namespace lab1_gr1.ViewModels.RecipeVM
{
    /// <summary>
    /// ViewModel do tworzenia lub edycji przepisu.
    /// </summary>
    public class CreateRecipeVM
    {
        /// <summary>
        /// Id przepisu (u¿ywane przy edycji).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nazwa przepisu.
        /// </summary>
        [Required(ErrorMessage = "Musisz podaæ nazwê przepisu")]
        public string Name { get; set; }

        /// <summary>
        /// Opis przepisu.
        /// </summary>
        [Required(ErrorMessage = "Musisz podaæ opis przepisu")]
        public string Description { get; set; }

        /// <summary>
        /// Instrukcje przygotowania przepisu.
        /// </summary>
        [Required(ErrorMessage = "Musisz podaæ instrukcje przygotowania")]
        public string Instructions { get; set; }

        /// <summary>
        /// Lista sk³adników przepisu.
        /// </summary>
        [MinLength(1, ErrorMessage = "Musisz dodaæ przynajmniej jeden sk³adnik")]
        public List<CreateRecipeIngredientVM> Ingredients { get; set; } = new List<CreateRecipeIngredientVM>();

        /// <summary>
        /// Lista harmonogramów przepisu (dni tygodnia, kiedy przepis jest planowany).
        /// </summary>
        public List<CreateRecipeScheduleVM> Schedules { get; set; } = new();

        /// <summary>
        /// Lista wybranych dni tygodnia (1=Poniedzia³ek, ..., 7=Niedziela).
        /// </summary>
        public List<int> SelectedDays { get; set; } = new();

        /// <summary>
        /// Id u¿ytkownika, do którego nale¿y przepis.
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// ViewModel reprezentuj¹cy pojedynczy wpis harmonogramu przepisu.
    /// </summary>
    public class CreateRecipeScheduleVM
    {
        /// <summary>
        /// Dzieñ tygodnia, do którego przypisany jest przepis (1=Poniedzia³ek, ..., 7=Niedziela).
        /// </summary>
        public int DayOfWeek { get; set; }
    }
}
