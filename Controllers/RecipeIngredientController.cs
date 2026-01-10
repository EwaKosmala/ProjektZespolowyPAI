using lab1_gr1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie relacją
    /// pomiędzy przepisami a składnikami.
    /// Umożliwia dodawanie oraz usuwanie składników
    /// przypisanych do konkretnego przepisu.
    /// </summary>
    public class RecipeIngredientController : Controller
    {
        /// <summary>
        /// Serwis obsługujący logikę biznesową relacji
        /// przepis–składnik.
        /// </summary>
        private readonly IRecipeIngredientService _recipeIngredientService;

        /// <summary>
        /// Serwis obsługujący logikę biznesową składników.
        /// </summary>
        private readonly IIngredientService _ingredientService;

        /// <summary>
        /// Konstruktor kontrolera relacji przepis–składnik.
        /// </summary>
        /// <param name="recipeIngredientService">
        /// Serwis relacji przepis–składnik
        /// </param>
        /// <param name="ingredientService">
        /// Serwis składników
        /// </param>
        public RecipeIngredientController(
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService)
        {
            _recipeIngredientService = recipeIngredientService;
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Wyświetla formularz dodawania składnika do przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <returns>
        /// Widok formularza z listą dostępnych składników
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Add(int recipeId)
        {
            ViewBag.RecipeId = recipeId;
            ViewBag.Ingredients = await _ingredientService.GetAllAsync();
            return View();
        }

        /// <summary>
        /// Dodaje wybrany składnik do wskazanego przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="ingredientId">Identyfikator składnika</param>
        /// <param name="quantity">Ilość składnika</param>
        /// <returns>
        /// Przekierowanie do widoku edycji przepisu
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Add(
            int recipeId,
            int ingredientId,
            string quantity)
        {
            await _recipeIngredientService
                .AddIngredientToRecipeAsync(recipeId, ingredientId, quantity);

            return RedirectToAction("Edit", "Recipe", new { id = recipeId });
        }

        /// <summary>
        /// Usuwa składnik z przypisanego przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="ingredientId">Identyfikator składnika</param>
        /// <returns>
        /// Przekierowanie do widoku edycji przepisu
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int recipeId, int ingredientId)
        {
            await _recipeIngredientService
                .RemoveIngredientFromRecipeAsync(recipeId, ingredientId);

            return RedirectToAction("Edit", "Recipe", new { id = recipeId });
        }
    }
}
