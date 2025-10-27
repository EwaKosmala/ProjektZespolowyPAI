using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class ShoppingListController : BaseController
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

        // GET: ShoppingList/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (list.UserId != userId) return Forbid(); // bezpieczeństwo

            // pobranie wszystkich składników do checkboxów
            var ingredients = await _ingredientService.GetUsedIngredientsAsync();

            // dodanie brakujących składników do listy (np. żeby były checkboxy)
            foreach (var ing in ingredients)
            {
                if (!list.Items.Any(i => i.IngredientId == ing.Id))
                {
                    list.Items.Add(new CreateShoppingListItemVM
                    {
                        IngredientId = ing.Id,
                        IngredientName = ing.Name,
                        Quantity = "",
                        IsSelected = false
                    });
                }
                else
                {
                    // zaznacz checkbox jeśli składnik jest już w liście
                    list.Items.First(i => i.IngredientId == ing.Id).IsSelected = true;
                }
            }

            return View(list);
        }

        // POST: ShoppingList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateShoppingListVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            model.Items = model.Items.Where(i => i.IsSelected).ToList();
            if (!model.Items.Any())
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
                return View(model);
            }

            var updated = await _shoppingListService.UpdateAsync(id, model, userId);
            if (!updated) return NotFound();

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
