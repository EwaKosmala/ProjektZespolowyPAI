using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.IngredientVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ingredients = await _ingredientService.GetAllAsync();
            return View(ingredients);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateIngredientVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _ingredientService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null) return NotFound();

            var model = new EditIngredientVM { Id = ingredient.Id, Name = ingredient.Name };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditIngredientVM model)
        {
            if (!ModelState.IsValid) return View(model);

            await _ingredientService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _ingredientService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
