using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.ViewModels.IngredientVM;
using Microsoft.AspNetCore.Mvc;

namespace lab1_gr1.Controllers
{
    /// <summary>
    /// Kontroler odpowiedzialny za zarządzanie składnikami.
    /// Udostępnia operacje CRUD: wyświetlanie, dodawanie,
    /// edycję oraz usuwanie składników.
    /// </summary>
    public class IngredientController : Controller
    {
        /// <summary>
        /// Serwis obsługujący logikę biznesową składników.
        /// </summary>
        private readonly IIngredientService _ingredientService;

        /// <summary>
        /// Mapper używany do konwersji modeli domenowych
        /// na modele widoków (ViewModel).
        /// </summary>
        protected readonly IMapper _mapper;

        /// <summary>
        /// Konstruktor kontrolera składników.
        /// </summary>
        /// <param name="ingredientService">Serwis składników</param>
        /// <param name="mapper">Mapper AutoMapper</param>
        public IngredientController(IIngredientService ingredientService, IMapper mapper)
        {
            _ingredientService = ingredientService;
            _mapper = mapper;
        }

        /// <summary>
        /// Wyświetla listę wszystkich dostępnych składników.
        /// </summary>
        /// <returns>Widok z listą składników</returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ingredients = await _ingredientService.GetAllAsync();
            return View(ingredients);
        }

        /// <summary>
        /// Wyświetla formularz dodawania nowego składnika.
        /// </summary>
        /// <returns>Widok formularza dodawania</returns>
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// Obsługuje wysłanie formularza dodawania nowego składnika.
        /// </summary>
        /// <param name="model">Model danych nowego składnika</param>
        /// <returns>
        /// Przekierowanie do listy składników lub ponowne
        /// wyświetlenie formularza w przypadku błędu walidacji
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Add(CreateIngredientVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _ingredientService.CreateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Wyświetla formularz edycji istniejącego składnika.
        /// </summary>
        /// <param name="id">Identyfikator składnika</param>
        /// <returns>
        /// Widok edycji składnika lub kod 404, jeśli składnik nie istnieje
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null)
                return NotFound();

            var model = new EditIngredientVM
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };

            return View(model);
        }

        /// <summary>
        /// Obsługuje zapis zmian edytowanego składnika.
        /// </summary>
        /// <param name="model">Model edycji składnika</param>
        /// <returns>
        /// Przekierowanie do listy składników lub ponowne
        /// wyświetlenie formularza w przypadku błędu walidacji
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Edit(EditIngredientVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            await _ingredientService.UpdateAsync(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Wyświetla widok potwierdzenia usunięcia składnika.
        /// </summary>
        /// <param name="id">Identyfikator składnika</param>
        /// <returns>
        /// Widok potwierdzenia usunięcia lub kod 404,
        /// jeśli składnik nie istnieje
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var ingredient = await _ingredientService.GetByIdAsync(id);
            if (ingredient == null)
                return NotFound();

            var model = _mapper.Map<IngredientListVM>(ingredient);
            return View(model);
        }

        /// <summary>
        /// Usuwa wskazany składnik po potwierdzeniu operacji.
        /// </summary>
        /// <param name="id">Identyfikator składnika</param>
        /// <returns>
        /// Przekierowanie do listy składników lub kod 404,
        /// jeśli składnik nie istnieje
        /// </returns>
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
