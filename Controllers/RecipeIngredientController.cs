using lab1_gr1.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class RecipeIngredientController : Controller
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IIngredientService _ingredientService;

        public RecipeIngredientController(
            IRecipeIngredientService recipeIngredientService,
            IIngredientService ingredientService)
        {
            _recipeIngredientService = recipeIngredientService;
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Add(int recipeId)
        {
            ViewBag.RecipeId = recipeId;
            ViewBag.Ingredients = await _ingredientService.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(int recipeId, int ingredientId, string quantity)
        {
            await _recipeIngredientService.AddIngredientToRecipeAsync(recipeId, ingredientId, quantity);
            return RedirectToAction("Edit", "Recipe", new { id = recipeId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int recipeId, int ingredientId)
        {
            await _recipeIngredientService.RemoveIngredientFromRecipeAsync(recipeId, ingredientId);
            return RedirectToAction("Edit", "Recipe", new { id = recipeId });
        }
    }
}
