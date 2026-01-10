using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.AspNetCore.Mvc;
using lab1_gr1.Controllers;
using lab1_gr1.Services;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie harmonogramem
    /// przepisów użytkownika w ujęciu tygodniowym.
    /// </summary>
    public class RecipeScheduleController : BaseController
    {
        /// <summary>
        /// Serwis obsługujący logikę harmonogramu przepisów.
        /// </summary>
        private readonly IRecipeScheduleService _recipeSchedule;

        /// <summary>
        /// Serwis obsługujący logikę przepisów.
        /// </summary>
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// Konstruktor kontrolera harmonogramu przepisów.
        /// </summary>
        /// <param name="recipeSchedule">Serwis harmonogramu przepisów</param>
        /// <param name="recipeService">Serwis przepisów</param>
        public RecipeScheduleController(
            IRecipeScheduleService recipeSchedule,
            IRecipeService recipeService)
        {
            _recipeSchedule = recipeSchedule;
            _recipeService = recipeService;
        }

        /// <summary>
        /// Domyślna akcja kontrolera.
        /// Przekierowuje do widoku tygodniowego harmonogramu.
        /// </summary>
        /// <returns>Przekierowanie do akcji <see cref="Week"/></returns>
        public IActionResult Index()
        {
            return RedirectToAction("Week");
        }

        /// <summary>
        /// Wyświetla tygodniowy harmonogram przepisów użytkownika.
        /// </summary>
        /// <returns>Widok harmonogramu tygodniowego</returns>
        public async Task<IActionResult> Week()
        {
            var userId = GetUserId();
            var model = await _recipeSchedule.GetRecipesPerWeekAsync(userId);
            return View(model);
        }

        /// <summary>
        /// Wyświetla formularz dodawania przepisu do wybranego dnia tygodnia.
        /// Widok renderowany jako częściowy (partial view).
        /// </summary>
        /// <param name="day">Dzień tygodnia (0–6)</param>
        /// <returns>Widok częściowy z listą dostępnych przepisów</returns>
        [HttpGet]
        public async Task<IActionResult> Add(int day)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var recipes = await _recipeSchedule.GetAvailableRecipesForUserAsync(userId);

            ViewBag.Day = day;
            return PartialView("_AddRecipeToDayPartial", recipes);
        }

        /// <summary>
        /// Dodaje wybrany przepis do harmonogramu użytkownika
        /// w określonym dniu tygodnia.
        /// </summary>
        /// <param name="vm">Model dodawania przepisu do dnia</param>
        /// <returns>Przekierowanie do widoku tygodniowego</returns>
        [HttpPost]
        public async Task<IActionResult> Add(AddRecipeToDayVM vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            await _recipeSchedule.AddRecipeToDayAsync(
                userId,
                vm.RecipeId,
                vm.DayOfWeek
            );

            return RedirectToAction("Week");
        }

        /// <summary>
        /// Usuwa przepis z harmonogramu użytkownika
        /// w określonym dniu tygodnia.
        /// </summary>
        /// <param name="day">Dzień tygodnia</param>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <returns>Przekierowanie do widoku tygodniowego</returns>
        [HttpPost]
        public async Task<IActionResult> Remove(int day, int recipeId)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            await _recipeSchedule.RemoveRecipeFromDayAsync(
                userId,
                recipeId,
                day
            );

            return RedirectToAction("Week");
        }

        /// <summary>
        /// Wyświetla formularz dodania istniejącego przepisu
        /// do harmonogramu użytkownika.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <returns>
        /// Widok dodawania przepisu lub kod 404,
        /// jeśli przepis nie istnieje
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> AddExisting(int recipeId)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            if (recipe == null)
                return NotFound();

            var model = new AddRecipeToScheduleVM
            {
                RecipeId = recipe.Id,
                RecipeName = recipe.Name
            };

            return View(model);
        }

        /// <summary>
        /// Dodaje istniejący przepis do harmonogramu użytkownika
        /// w wybranym dniu tygodnia.
        /// </summary>
        /// <param name="model">Model dodawania przepisu do harmonogramu</param>
        /// <returns>Przekierowanie do strony głównej harmonogramu</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExisting(AddRecipeToScheduleVM model)
        {
            int userId = GetUserId();

            var success = await _recipeSchedule.AddExistingRecipeToScheduleAsync(
                model.RecipeId,
                userId,
                model.SelectedDay
            );

            if (success)
                TempData["Success"] =
                    "Przepis został dodany do harmonogramu!";
            else
                TempData["Error"] =
                    "Ten przepis już znajduje się w harmonogramie lub wystąpił błąd.";

            return RedirectToAction("Index");
        }
    }
}
