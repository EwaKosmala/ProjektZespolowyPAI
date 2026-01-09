using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.Services;
using lab1_gr1.ViewModels.RecipeVM;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie przepisami użytkowników.
    /// Umożliwia tworzenie, edycję, usuwanie, przeglądanie oraz
    /// filtrowanie przepisów.
    /// </summary>
    public class RecipeController : BaseController
    {
        /// <summary>
        /// Serwis obsługujący logikę biznesową przepisów.
        /// </summary>
        private readonly IRecipeService _recipeService;

        /// <summary>
        /// Serwis obsługujący logikę biznesową składników.
        /// </summary>
        private readonly IIngredientService _ingredientService;

        /// <summary>
        /// Konstruktor kontrolera przepisów.
        /// </summary>
        /// <param name="recipeService">Serwis przepisów</param>
        /// <param name="ingredientService">Serwis składników</param>
        public RecipeController(IRecipeService recipeService, IIngredientService ingredientService)
        {
            _recipeService = recipeService;
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Wyświetla listę przepisów zalogowanego użytkownika.
        /// </summary>
        /// <returns>Widok z listą przepisów</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int userId = GetUserId();

            var recipes = await _recipeService.GetAllByUserIdAsync(userId);
            return View(recipes);
        }

        /// <summary>
        /// Wyświetla szczegóły wybranego przepisu.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <returns>
        /// Widok szczegółów przepisu lub kod 404,
        /// jeśli przepis nie został znaleziony
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _recipeService.GetByIdAsync(id);
            if (recipe == null)
                return NotFound("Przepis nie został znaleziony.");

            return View(recipe);
        }

        /// <summary>
        /// Wyświetla formularz tworzenia nowego przepisu.
        /// </summary>
        /// <returns>Widok tworzenia przepisu</returns>
        [HttpGet]
        public IActionResult Create()
        {
            return View(new CreateRecipeVM());
        }

        /// <summary>
        /// Obsługuje wysłanie formularza tworzenia nowego przepisu.
        /// Przepis zostaje przypisany do aktualnie zalogowanego użytkownika.
        /// </summary>
        /// <param name="model">Model danych nowego przepisu</param>
        /// <returns>
        /// Przekierowanie do widoku szczegółów nowo utworzonego przepisu
        /// lub ponowne wyświetlenie formularza w przypadku błędów walidacji
        /// </returns>
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

        /// <summary>
        /// Usuwa wskazany przepis użytkownika.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <returns>
        /// Przekierowanie do listy przepisów lub kod 404,
        /// jeśli przepis nie istnieje
        /// </returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _recipeService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Wyświetla formularz edycji istniejącego przepisu.
        /// Dostęp tylko dla właściciela przepisu.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <returns>Widok edycji przepisu lub przekierowanie</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _recipeService.GetForEditAsync(id);
            if (model == null || model.UserId != GetUserId())
                return RedirectToAction("Index", "Recipe");

            ViewBag.RecipeId = id;
            return View(model);
        }

        /// <summary>
        /// Obsługuje zapis zmian edytowanego przepisu.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <param name="model">Model edycji przepisu</param>
        /// <returns>
        /// Przekierowanie do szczegółów przepisu lub kod 404
        /// </returns>
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

        /// <summary>
        /// Wyświetla widok błędu z przekazaną wiadomością.
        /// </summary>
        /// <param name="message">Treść komunikatu błędu</param>
        /// <returns>Widok błędu</returns>
        [HttpGet]
        public IActionResult Error(string message)
        {
            return View(model: message);
        }

        /// <summary>
        /// Wyświetla widok przeglądania i filtrowania przepisów.
        /// </summary>
        /// <returns>Widok listy przepisów z filtrami</returns>
        [HttpGet]
        public async Task<IActionResult> Browse()
        {
            int userId = GetUserId();

            var ingredients = await _ingredientService.GetUsedIngredientsAsync();
            var allRecipes = await _recipeService.GetAllAsync();

            var vm = new RecipeListFilterVM
            {
                AvailableIngredients = ingredients.ToList(),
                Recipes = allRecipes.ToList(),
                ShowMyRecipes = true,
                ShowOthersRecipes = true
            };

            return View(vm);
        }

        /// <summary>
        /// Obsługuje filtrowanie przepisów na podstawie
        /// wybranych składników oraz preferencji użytkownika.
        /// </summary>
        /// <param name="model">Model filtrów przepisów</param>
        /// <returns>Widok przepisów po zastosowaniu filtrów</returns>
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

            model.AvailableIngredients =
                (await _ingredientService.GetUsedIngredientsAsync()).ToList();

            model.Recipes = filtered.ToList();

            return View(model);
        }
    }
}
