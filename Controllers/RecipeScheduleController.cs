using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class RecipeScheduleController : Controller
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
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var week = await _recipeSchedule.GetRecipesPerWeekAsync(userId);
            return View(week);
        }

    }
}
