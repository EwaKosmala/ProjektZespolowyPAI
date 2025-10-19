using lab1_gr1.Models;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Controllers
{
    public class RecipeController : Controller
    {
        private readonly MyDBContext _dbContext;

        public RecipeController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: /Recipe
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var recipes = await _dbContext.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .ToListAsync();

            return View(recipes); // Views/Recipe/Index.cshtml
        }

        // GET: /Recipe/Add
        [HttpGet]
        public IActionResult Add()
        {
            return View(); // Views/Recipe/Add.cshtml
        }

        //POST: /Recipe/Add
       [HttpPost]
       [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Recipe recipe)
        {
            // 1. Pobranie nazwy zalogowanego użytkownika
            var username = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Index", "Home"); // nie zalogowany

            // 2. Pobranie obiektu User z bazy
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return RedirectToAction("Index", "Home"); // nie znaleziono użytkownika

            // 3. Przypisanie UserId i daty
            recipe.UserId = user.Id;
            recipe.CreatedAt = DateTime.Now;

            // 4. Dodanie do DbContext i zapis
            await _dbContext.Recipes.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();

            // 5. Opcjonalnie przekierowanie lub widok potwierdzenia
            return RedirectToAction("Index"); // np. lista przepisów
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            return View(recipe); // musi istnieć Views/Recipe/Edit.cshtml
        }

        // POST: /Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe recipe)
        {
            var existingRecipe = await _dbContext.Recipes.FindAsync(id);
            if (existingRecipe == null) return NotFound();

            existingRecipe.Name = recipe.Name;
            existingRecipe.Description = recipe.Description;
            existingRecipe.Instructions = recipe.Instructions;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // GET: /Recipe/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound();

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }
}
