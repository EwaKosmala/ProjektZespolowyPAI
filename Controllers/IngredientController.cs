using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.IngredientVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class IngredientController : Controller
    {
        private readonly IIngredientService _ingredientService;
        protected readonly IMapper _mapper;
        public IngredientController(IIngredientService ingredientService, IMapper mapper)
        {
            _ingredientService = ingredientService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ingredients = await _ingredientService.GetAllAsync();
            return View(ingredients);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateIngredientVM model)
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null)
                return NotFound();

            // Mapowanie do VM (jeśli nie masz w GetByIdAsync mapowania)
            var model = _mapper.Map<IngredientListVM>(ingredient);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _ingredientService.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

    }
}
