using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class ShoppingListController : Controller
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IIngredientService _ingredientService; // do pobrania składników z serwisu

        public ShoppingListController(IShoppingListService shoppingListService, IIngredientService ingredientService)
        {
            _shoppingListService = shoppingListService;
            _ingredientService = ingredientService;
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