using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.AspNetCore.Mvc;
using lab1_gr1.Controllers;

namespace lab1_gr1.Controllers
{
    public class RecipeScheduleController : BaseController
    {
        private readonly IRecipeScheduleService _recipeSchedule;
        public RecipeScheduleController(IRecipeScheduleService recipeSchedule)
        {
            _recipeSchedule = recipeSchedule;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Week");
        }

        public async Task<IActionResult> Week()
        {
            var userId = GetUserId();
            var model = await _recipeSchedule.GetRecipesPerWeekAsync(userId);
            return View(model); // ✅ model typu WeekVM
        }
        [HttpGet]
        public async Task<IActionResult> Add(int day)
        {
            var userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var recipes = await _recipeSchedule.GetAvailableRecipesForUserAsync(userId);
            ViewBag.Day = day;
            return PartialView("_AddRecipeToDayPartial", recipes); // modal content
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
    }
}
