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

            model.Items = model.Items.Where(i => i.IsSelected).ToList();

            if (!model.Items.Any())
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
            }

            if (model.Items.Any(i => string.IsNullOrWhiteSpace(i.Quantity)))
            {
                ModelState.AddModelError("", "Musisz podać ilość dla wszystkich zaznaczonych składników.");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.ShowBackButton = true;
                ViewBag.ReturnUrl = Url.Action("Create", "ShoppingList");

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
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null) return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (list.UserId != userId) return Forbid();

            var ingredients = await _ingredientService.GetAllAsync();

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
                    list.Items.First(i => i.IngredientId == ing.Id).IsSelected = true;
                }
            }

            return View("Create", list);
        }

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
            if (model.Items.Any(i => string.IsNullOrWhiteSpace(i.Quantity)))
            {
                ModelState.AddModelError("", "Musisz podać ilość dla wszystkich zaznaczonych składników.");
            }
            if (!ModelState.IsValid)
            {
                return View("Create", model);
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