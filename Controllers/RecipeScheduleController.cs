using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.AspNetCore.Mvc;
using lab1_gr1.Controllers;
using lab1_gr1.Services;

namespace lab1_gr1.Controllers
{
    public class RecipeScheduleController : BaseController
    {
        private readonly IRecipeScheduleService _recipeSchedule;
        private readonly IRecipeService _recipeService;
        public RecipeScheduleController(IRecipeScheduleService recipeSchedule, IRecipeService recipeService)
        {
            _recipeSchedule = recipeSchedule;
            _recipeService = recipeService;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Week");
        }

        public async Task<IActionResult> Week()
        {
            var userId = GetUserId();
            var model = await _recipeSchedule.GetRecipesPerWeekAsync(userId);
            return View(model); 
        }
        [HttpGet]
        public async Task<IActionResult> Add(int day)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var recipes = await _recipeSchedule.GetAvailableRecipesForUserAsync(userId);
            ViewBag.Day = day;
            return PartialView("_AddRecipeToDayPartial", recipes);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRecipeToDayVM vm)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _recipeSchedule.AddRecipeToDayAsync(userId, vm.RecipeId, vm.DayOfWeek);
            return RedirectToAction("Week");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(int day, int recipeId)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            await _recipeSchedule.RemoveRecipeFromDayAsync(userId, recipeId, day);
            return RedirectToAction("Week");
        }

        [HttpGet]
        public async Task<IActionResult> AddExisting(int recipeId)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            if (recipe == null) return NotFound();

            var model = new AddRecipeToScheduleVM
            {
                RecipeId = recipe.Id,
                RecipeName = recipe.Name
            };

            return View(model); 
        }

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
                TempData["Success"] = "Przepis został dodany do harmonogramu!";
            else
                TempData["Error"] = "Ten przepis już znajduje się w harmonogramie lub wystąpił błąd.";

            return RedirectToAction("Index");
        }


    }
}
