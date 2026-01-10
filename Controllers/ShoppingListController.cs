using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie listami zakupów użytkownika.
    /// Umożliwia tworzenie, edycję, usuwanie, generowanie oraz eksport list zakupów.
    /// </summary>
    public class ShoppingListController : BaseController
    {
        /// <summary>
        /// Serwis obsługujący logikę biznesową list zakupów.
        /// </summary>
        private readonly IShoppingListService _shoppingListService;

        /// <summary>
        /// Serwis obsługujący logikę biznesową składników.
        /// </summary>
        private readonly IIngredientService _ingredientService;

        /// <summary>
        /// Konstruktor kontrolera list zakupów.
        /// </summary>
        /// <param name="shoppingListService">Serwis list zakupów</param>
        /// <param name="ingredientService">Serwis składników</param>
        public ShoppingListController(
            IShoppingListService shoppingListService,
            IIngredientService ingredientService)
        {
            _shoppingListService = shoppingListService;
            _ingredientService = ingredientService;
        }

        /// <summary>
        /// Wyświetla wszystkie listy zakupów zalogowanego użytkownika.
        /// </summary>
        /// <returns>Widok z listami zakupów</returns>
        public async Task<IActionResult> Index()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var lists = await _shoppingListService.GetAllByUserIdAsync(userId);
            return View(lists);
        }

        /// <summary>
        /// Przekierowuje do edycji wybranej listy zakupów.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <returns>Przekierowanie do edycji listy</returns>
        public async Task<IActionResult> Details(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null)
                return NotFound();

            return RedirectToAction("Edit", new { id = list.Id });
        }

        /// <summary>
        /// Wyświetla formularz tworzenia nowej listy zakupów.
        /// </summary>
        /// <returns>Widok tworzenia listy zakupów</returns>
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

        /// <summary>
        /// Obsługuje tworzenie nowej listy zakupów.
        /// </summary>
        /// <param name="model">Model tworzenia listy zakupów</param>
        /// <returns>
        /// Przekierowanie do list zakupów lub ponowne wyświetlenie formularza
        /// w przypadku błędów walidacji
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateShoppingListVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (!model.Items.Any(i => i.IsSelected))
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
            }

            if (model.Items.Any(i => i.IsSelected && string.IsNullOrWhiteSpace(i.Quantity)))
            {
                ModelState.AddModelError("", "Musisz podać ilość dla wszystkich zaznaczonych składników.");
            }

            for (int i = 0; i < model.Items.Count; i++)
            {
                if (!model.Items[i].IsSelected)
                {
                    ModelState.Remove($"Items[{i}].Quantity");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ShowBackButton = true;
                ViewBag.ReturnUrl = Url.Action("Create", "ShoppingList");
                return View(model);
            }

            model.Items = model.Items.Where(i => i.IsSelected).ToList();
            await _shoppingListService.CreateAsync(model, userId);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Usuwa wskazaną listę zakupów.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <returns>Przekierowanie do list zakupów</returns>
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _shoppingListService.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Wyświetla formularz edycji istniejącej listy zakupów.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <returns>Widok edycji listy</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var list = await _shoppingListService.GetByIdAsync(id);
            if (list == null)
                return NotFound();

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            if (list.UserId != userId)
                return Forbid();

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

        /// <summary>
        /// Obsługuje zapis zmian edytowanej listy zakupów.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <param name="model">Model edycji listy zakupów</param>
        /// <returns>Przekierowanie do list zakupów</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateShoppingListVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (!model.Items.Any(i => i.IsSelected))
            {
                ModelState.AddModelError("", "Musisz zaznaczyć przynajmniej jeden składnik.");
                return View(model);
            }

            if (model.Items.Any(i => i.IsSelected && string.IsNullOrWhiteSpace(i.Quantity)))
            {
                ModelState.AddModelError("", "Musisz podać ilość dla wszystkich zaznaczonych składników.");
            }

            for (int i = 0; i < model.Items.Count; i++)
            {
                if (!model.Items[i].IsSelected)
                {
                    ModelState.Remove($"Items[{i}].Quantity");
                }
            }

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            model.Items = model.Items.Where(i => i.IsSelected).ToList();

            var updated = await _shoppingListService.UpdateAsync(id, model, userId);
            if (!updated)
                return NotFound();

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Wyświetla formularz generowania listy zakupów
        /// na podstawie wybranych dni tygodnia.
        /// </summary>
        /// <returns>Widok generowania listy</returns>
        [HttpGet]
        public IActionResult FromDays()
        {
            return View(new CreateShoppingListFromDaysVM());
        }

        /// <summary>
        /// Generuje listę zakupów na podstawie przepisów
        /// zaplanowanych na wybrane dni tygodnia.
        /// </summary>
        /// <param name="model">Model wyboru dni</param>
        /// <returns>Widok tworzenia listy zakupów</returns>
        [HttpPost]
        public async Task<IActionResult> FromDays(CreateShoppingListFromDaysVM model)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (model.SelectedDays == null || !model.SelectedDays.Any())
            {
                ViewBag.Error = "Musisz wybrać przynajmniej jeden dzień.";
                return View(model);
            }

            var shoppingList =
                await _shoppingListService.GenerateFromDaysAsync(userId, model.SelectedDays);

            return View("Create", shoppingList);
        }

        /// <summary>
        /// Generuje listę zakupów na podstawie
        /// całego tygodniowego harmonogramu użytkownika.
        /// </summary>
        /// <returns>Widok tworzenia listy zakupów</returns>
        [HttpGet]
        public async Task<IActionResult> Generate()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var days = new List<int> { 0, 1, 2, 3, 4, 5, 6 };

            var shoppingList =
                await _shoppingListService.GenerateFromDaysAsync(userId, days);

            if (!shoppingList.Items.Any())
            {
                TempData["Error"] = "Brak przepisów w planie na ten tydzień.";
                return RedirectToAction("Week", "RecipeSchedule");
            }

            return View("Create", shoppingList);
        }

        /// <summary>
        /// Generuje plik PDF z listą zakupów
        /// i udostępnia go do pobrania.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <returns>Plik PDF z listą zakupów</returns>
        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            int userId = GetUserId();

            try
            {
                var pdfBytes =
                    await _shoppingListService.GeneratePdfAsync(id, userId);

                return File(
                    pdfBytes,
                    "application/pdf",
                    $"lista_zakupow_{DateTime.Now:yyyyMMdd}.pdf"
                );
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }
        }
    }
}
