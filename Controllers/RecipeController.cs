using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.Services;
using lab1_gr1.ViewModels.RecipeVM;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Controllers
{

    public class RecipeController : BaseController
    {
        private readonly IRecipeService _recipeService;
        private readonly IIngredientService _ingredientService;


        public RecipeController(IRecipeService recipeService, IIngredientService ingredientService)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();

            var recipes = await _recipeService.GetAllByUserIdAsync(userId);
            return View(recipes); 
        }

        // GET: api/recipes/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
                return NotFound("Przepis nie został znaleziony.");

            return View(recipe);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRecipeVM());
        }

        // POST: api/recipes
        [HttpPost]
        public async Task<IActionResult> Create(CreateRecipeVM model)
        {
            Console.WriteLine($"Wybrane dni: {string.Join(", ", model.SelectedDays)}");

            if (!ModelState.IsValid)
                return View(model);

            int userId = GetUserId();

            var recipeId = await _recipeService.CreateAsync(model, userId);
            return RedirectToAction("Details", new { id = recipeId });
        }


        // DELETE: api/recipe/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _recipeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }

        //UPDATE: api/recipe/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _recipeService.GetForEditAsync(id);
            if (model == null || model.UserId != GetUserId())
                return RedirectToAction("Index", "Recipe");
            ViewBag.RecipeId = id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateRecipeVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null || recipe.UserId != GetUserId())
                return RedirectToAction("Index", "Recipe");

            var updated = await _recipeService.UpdateAsync(model.Id, model);
            if (!updated)
                return NotFound();

            return RedirectToAction("Details", new { id });
        }
        [HttpGet]
        public IActionResult Error(string message)
        {
            return View(model: message);
        }

        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            int userId = GetUserId();

            var ingredients = await _ingredientService.GetUsedIngredientsAsync();
            var allRecipes = await _recipeService.GetAllAsync(); // wszystkie, nie tylko użytkownika

            var vm = new RecipeListFilterVM
            {
                AvailableIngredients = ingredients.ToList(),
                Recipes = allRecipes.ToList(),
                ShowMyRecipes = true,
                ShowOthersRecipes = true
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Browse(RecipeListFilterVM model)
        {
            int userId = GetUserId();

            var filtered = await _recipeService.FilterAsync(
                userId,
                model.SelectedIngredientIds,
                model.ShowMyRecipes,
                model.ShowOthersRecipes
            );

            model.AvailableIngredients = (await _ingredientService.GetUsedIngredientsAsync()).ToList();
            model.Recipes = filtered.ToList();

            return View(model);
        }




    }
}
