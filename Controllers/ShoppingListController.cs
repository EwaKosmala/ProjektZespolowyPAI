using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IIngredientService _ingredientService; // do pobrania składników

        public ShoppingListController(IShoppingListService shoppingListService, IIngredientService ingredientService)
        {
            _shoppingListService = shoppingListService;
            _ingredientService = ingredientService;
        }

        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var lists = await _shoppingListService.GetAllByUserIdAsync(userId);
            return View(lists);
        }

        public async Task<IActionResult> Details(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null) return NotFound();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var ingredients = await _ingredientService.GetAllAsync();

            var model = new CreateShoppingListVM
            {
                Items = ingredients.Select(i => new CreateShoppingListItemVM
                {
                    IngredientId = i.Id,
                    IngredientName = i.Name,
                    Quantity = "",
                    IsSelected = false
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShoppingListVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            model.Items = model.Items.Where(i => i.IsSelected).ToList();

            if (!model.Items.Any())
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
                return View(model);
            }

            await _shoppingListService.CreateAsync(model, userId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _shoppingListService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult FromDays()
        {
            return View(new CreateShoppingListFromDaysVM());
        }

        [HttpPost]
        public async Task<IActionResult> FromDays(CreateShoppingListFromDaysVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var shoppingList = await _shoppingListService.GenerateFromDaysAsync(userId, model.SelectedDays);

            return View("Create", shoppingList); 
        }
    }
}
