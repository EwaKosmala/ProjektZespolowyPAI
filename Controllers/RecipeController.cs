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

      
    }
}
