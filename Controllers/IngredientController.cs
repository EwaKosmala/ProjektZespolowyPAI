using lab1_gr1.Models;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Controllers
{
    public class IngredientController : Controller
    {
        private readonly MyDBContext _dbContext;
        public IngredientController(MyDBContext dbContext) => _dbContext = dbContext;

        // GET: /Ingredient/Add
        [HttpGet("/Ingredient/Add")]
        public IActionResult Add()
        {
            return View(); // Views/Ingredients/AddIngredient.cshtml
        }

        // POST: /Ingredient/Add
        [HttpPost("/Ingredient/Add")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Ingredient ingredient)
        {

            // dodanie skladnika do bazy i zpais
            _dbContext.Ingredients.Add(ingredient);
            await _dbContext.SaveChangesAsync();

            // powrot do index
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ingredients = await _dbContext.Ingredients
                .Include(i => i.RecipeIngredients)
                .Include(i => i.ShoppingListItems)
                .ToListAsync();

            return Ok(ingredients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var ingredient = await _dbContext.Ingredients
                .Include(i => i.RecipeIngredients)
                .Include(i => i.ShoppingListItems)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (ingredient == null) return NotFound();
            return Ok(ingredient);
        }

        [HttpGet("/Ingredient")] 
        public async Task<IActionResult> Index()
        {
            var ingredients = await _dbContext.Ingredients
                .Include(i => i.RecipeIngredients)
                .Include(i => i.ShoppingListItems)
                .ToListAsync();
            return View(ingredients);
        }

        // GET: /Ingredient/Delete/{id}
        [HttpGet("/Ingredient/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(id);
            if (ingredient == null)
                return NotFound();

            _dbContext.Ingredients.Remove(ingredient);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

    }

}
