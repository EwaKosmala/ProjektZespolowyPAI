using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    public class ShoppingListController : BaseController
    {
        private readonly IShoppingListService _shoppingListService;
        private readonly IIngredientService _ingredientService;

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

            return RedirectToAction("Edit", new { id = list.Id });
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


            var selectedItems = model.Items.Where(i => i.IsSelected).ToList();

            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
            }
            else
            {

                foreach (var item in selectedItems)
                {
                    if (string.IsNullOrWhiteSpace(item.Quantity))
                    {
                        ModelState.AddModelError("", $"Podaj ilość dla składnika: {item.IngredientName}");
                    }
                }
            }

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            model.Items = selectedItems;

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
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (list.UserId != userId) return Forbid();

            var allIngredients = await _ingredientService.GetAllAsync(); 

            var mergedItems = new List<CreateShoppingListItemVM>();

            foreach (var ing in allIngredients)
            {
                var existingItem = list.Items.FirstOrDefault(i => i.IngredientId == ing.Id);
                if (existingItem != null)
                {

                    existingItem.IsSelected = true;
                    mergedItems.Add(existingItem);
                }
                else
                {

                    mergedItems.Add(new CreateShoppingListItemVM
                    {
                        IngredientId = ing.Id,
                        IngredientName = ing.Name,
                        Quantity = "",
                        IsSelected = false
                    });
                }
            }

            list.Items = mergedItems;


            return View("Create", list);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateShoppingListVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var selectedItems = model.Items.Where(i => i.IsSelected).ToList();

            if (!selectedItems.Any())
            {
                ModelState.AddModelError("", "Lista nie może być pusta. Zaznacz składniki.");
                return View("Create", model);
            }

            model.Items = selectedItems; // Przekazujemy tylko wybrane do serwisu

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
            if (model.SelectedDays == null || !model.SelectedDays.Any())
            {
                ViewBag.Error = "Musisz wybrać przynajmniej jeden dzień.";
                return View(model);
            }

            var shoppingList = await _shoppingListService.GenerateFromDaysAsync(userId, model.SelectedDays);

            return View("Create", shoppingList);
        }

     
        [HttpGet]
        public async Task<IActionResult> Generate()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var days = new List<int> { 0, 1, 2, 3, 4, 5, 6 };
            var shoppingList = await _shoppingListService.GenerateFromDaysAsync(userId, days);

            if (!shoppingList.Items.Any())
            {
                TempData["Error"] = "Brak przepisów w planie na ten tydzień.";
                return RedirectToAction("Week", "RecipeSchedule"); // Wróć do planera
            }

            return View("Create", shoppingList);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            int userId = GetUserId();
            try
            {
                var pdfBytes = await _shoppingListService.GeneratePdfAsync(id, userId);
                return File(pdfBytes, "application/pdf", $"lista_zakupow_{DateTime.Now:yyyyMMdd}.pdf");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}