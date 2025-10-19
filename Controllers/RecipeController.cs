using lab1_gr1.Models;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : Controller
    {
        private readonly MyDBContext _dbContext;

        public RecipeController(MyDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/recipes/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return NotFound();

            return Ok(recipe);
        }

        // POST: api/recipes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            recipe.CreatedAt = DateTime.Now;
            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe);
        }


        // DELETE: api/recipe/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound(new { message = $"Recipe with Id {id} not found." });
            }

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        //UPDATE: api/recipe/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Recipe updatedRecipe) {

            // Sprawdzamy tu czy id w URL zgadza się z id w ciele zapytania
            if (id != updatedRecipe.Id)
            {
                return BadRequest(new { message = "Id in URL does not match Id in body." });
            }

            // Walidacja modelu czy wszystkie wymagane pola są poprawnie wypełnione
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Pobieramy istniejący przepis z bazy danych wraz z powiązanymi składnikami i harmonogramami
            var exististngRecipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);

            // Jeśli przepis nie istnieje, zwracamy błąd 404
            if (exististngRecipe == null)
            {
                return NotFound(new { message = $"Recipe with Id {id} not found." });
            }

            // Aktualizujemy pola przepisu
            exististngRecipe.Name = updatedRecipe.Name;
            exististngRecipe.Description = updatedRecipe.Description;
            exististngRecipe.Instructions = updatedRecipe.Instructions;

            // Aktualizujemy składniki przepisu, jeśli zostały podane
            if (updatedRecipe.RecipeIngredients != null)
            {
                // Usuwamy stare składniki przypisane do przepisu
                _dbContext.RecipeIngredients.RemoveRange(exististngRecipe.RecipeIngredients);
                // Przypisujemy nowe składniki
                exististngRecipe.RecipeIngredients = updatedRecipe.RecipeIngredients;
            }

            // Aktualizujemy harmonogramy przepisu, jeśli zostały podane
            if (updatedRecipe.RecipeSchedules != null)
            {
                //Usuwamy stare harmonogramy przypisane do przepisu
                _dbContext.RecipeSchedules.RemoveRange(exististngRecipe.RecipeSchedules);
                // Przypisujemy nowe harmonogramy
                exististngRecipe.RecipeSchedules = updatedRecipe.RecipeSchedules;
            }

            // Zapisujemy zmiany w bazie danych
            await _dbContext.SaveChangesAsync();
            // Zwracamy zaktualizowany przepis
            return Ok(exististngRecipe);
        }


    }
}
