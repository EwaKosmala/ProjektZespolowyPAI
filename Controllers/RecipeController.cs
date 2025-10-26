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

        // GET: api/recipes/5
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return NotFound();

            return Ok(recipe); //change to redirect view
        }

        // POST: api/recipes
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Recipe recipe)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // redirect error view

            recipe.CreatedAt = DateTime.Now;
            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = recipe.Id }, recipe); //change to redirect view
        }

        // GET: /Recipe/Edit/{id}
        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null) return NotFound();

            // Load all ingredients and mark selected
            var ingredients = await _dbContext.Ingredients.ToListAsync();
            ViewBag.Ingredients = ingredients;
            ViewBag.SelectedIngredients = recipe.RecipeIngredients.Select(ri => ri.IngredientId).ToArray();

            return View(recipe);
        }

        // POST: /Recipe/Edit/{id}
        [HttpPost("Edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Recipe updatedRecipe, int[] selectedIngredients)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null) return NotFound();

            recipe.Name = updatedRecipe.Name;
            recipe.Description = updatedRecipe.Description;
            recipe.Instructions = updatedRecipe.Instructions;

            // Update ingredients
            recipe.RecipeIngredients.Clear();
            if (selectedIngredients != null)
            {
                foreach (var ingredientId in selectedIngredients)
                {
                    recipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = recipe.Id,
                        IngredientId = ingredientId,
                        Quantity = "" // optional
                    });
                }
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
         

        // DELETE: api/recipe/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
            {
                return NotFound(new { message = $"Recipe with Id {id} not found." });
            }

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return NoContent(); //change to redirect view
        }

        //UPDATE: api/recipe/5
        [HttpPut]
        public async Task<IActionResult> Update(int id, [FromBody] Recipe updatedRecipe) {

            if (id != updatedRecipe.Id)
            {
                return BadRequest(new { message = "Id in URL does not match Id in body." }); // redirect error view
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // redirect error view
            }

            var exististngRecipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);
            
            if (exististngRecipe == null)
            {
                return NotFound(new { message = $"Recipe with Id {id} not found." });
            }

            exististngRecipe.Name = updatedRecipe.Name;
            exististngRecipe.Description = updatedRecipe.Description;
            exististngRecipe.Instructions = updatedRecipe.Instructions;

            if (updatedRecipe.RecipeIngredients != null)
            {
                _dbContext.RecipeIngredients.RemoveRange(exististngRecipe.RecipeIngredients);
                exististngRecipe.RecipeIngredients = updatedRecipe.RecipeIngredients;
            }

            if (updatedRecipe.RecipeSchedules != null)
            {
                _dbContext.RecipeSchedules.RemoveRange(exististngRecipe.RecipeSchedules);
                exististngRecipe.RecipeSchedules = updatedRecipe.RecipeSchedules;
            }

            await _dbContext.SaveChangesAsync();
            return Ok(exististngRecipe); // change to redirect view
        }


    }
}
