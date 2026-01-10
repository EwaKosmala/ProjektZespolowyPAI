using lab1_gr1.Interfaces;
using ListaZakupow.Model.DataModels;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.Models;
using AutoMapper;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za zarządzanie przepisami kulinarnymi.
    /// Obsługuje tworzenie, edycję, usuwanie, pobieranie oraz filtrowanie przepisów.
    /// </summary>
    public class RecipeService : BaseService, IRecipeService
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="RecipeService"/>.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych aplikacji.</param>
        /// <param name="mapper">Obiekt AutoMapper.</param>
        public RecipeService(MyDBContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        /// <summary>
        /// Pobiera przepis wraz z powiązanymi składnikami, harmonogramem oraz użytkownikiem.
        /// </summary>
        /// <param name="id">Identyfikator przepisu.</param>
        /// <returns>
        /// Obiekt <see cref="Recipe"/> lub <c>null</c>, jeśli przepis nie istnieje.
        /// </returns>
        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeSchedules)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <summary>
        /// Pobiera wszystkie przepisy należące do danego użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>Lista przepisów w postaci <see cref="RecipeListVM"/>.</returns>
        public async Task<IEnumerable<RecipeListVM>> GetAllByUserIdAsync(int userId)
        {
            var recipes = await _dbContext.Recipes
                .Where(r => r.UserId == userId)
                .Include(r => r.User)
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
        }

        /// <summary>
        /// Pobiera wszystkie przepisy dostępne w systemie.
        /// </summary>
        /// <returns>Lista wszystkich przepisów.</returns>
        public async Task<IEnumerable<RecipeListVM>> GetAllAsync()
        {
            var recipes = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.User)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
        }

        /// <summary>
        /// Tworzy nowy przepis wraz ze składnikami oraz opcjonalnym harmonogramem.
        /// </summary>
        /// <param name="model">Model tworzenia przepisu.</param>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>Identyfikator utworzonego przepisu.</returns>
        public async Task<int> CreateAsync(CreateRecipeVM model, int userId)
        {
            var recipe = _mapper.Map<Recipe>(model);
            recipe.UserId = userId;
            recipe.CreatedAt = DateTime.Now;

            recipe.RecipeIngredients = new List<RecipeIngredient>();

            foreach (var ing in model.Ingredients)
            {
                if (string.IsNullOrWhiteSpace(ing.IngredientName))
                    continue;

                var existingIngredient = await _dbContext.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == ing.IngredientName.ToLower());

                if (existingIngredient == null)
                {
                    existingIngredient = new Ingredient { Name = ing.IngredientName };
                    _dbContext.Ingredients.Add(existingIngredient);
                    await _dbContext.SaveChangesAsync();
                }

                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    IngredientId = existingIngredient.Id,
                    Quantity = ing.Quantity
                });
            }

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            if (model.SelectedDays != null && model.SelectedDays.Any())
            {
                var schedules = model.SelectedDays.Select(day => new RecipeSchedule
                {
                    DayOfWeek = day,
                    UserId = recipe.UserId,
                    RecipeId = recipe.Id
                }).ToList();

                _dbContext.RecipeSchedules.AddRange(schedules);
                await _dbContext.SaveChangesAsync();
            }

            return recipe.Id;
        }

        /// <summary>
        /// Pobiera przepis w formie modelu edycji.
        /// </summary>
        /// <param name="id">Identyfikator przepisu.</param>
        /// <returns>
        /// Model <see cref="CreateRecipeVM"/> lub <c>null</c>, jeśli przepis nie istnieje.
        /// </returns>
        public async Task<CreateRecipeVM?> GetForEditAsync(int id)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return null;

            var model = _mapper.Map<CreateRecipeVM>(recipe);

            model.SelectedDays = recipe.RecipeSchedules
                .Select(s => s.DayOfWeek)
                .ToList();

            return model;
        }

        /// <summary>
        /// Aktualizuje istniejący przepis wraz ze składnikami i harmonogramem.
        /// </summary>
        /// <param name="id">Identyfikator przepisu.</param>
        /// <param name="model">Model edycji przepisu.</param>
        /// <returns>
        /// <c>true</c> jeśli aktualizacja się powiodła, w przeciwnym razie <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateAsync(int id, CreateRecipeVM model)
        {
            var recipe = await _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                .Include(r => r.RecipeSchedules)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
                return false;

            recipe.Name = model.Name;
            recipe.Description = model.Description;
            recipe.Instructions = model.Instructions;

            _dbContext.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

            if (model.Ingredients != null)
            {
                foreach (var ing in model.Ingredients)
                {
                    if (string.IsNullOrWhiteSpace(ing.IngredientName))
                        continue;

                    var ingredient = await _dbContext.Ingredients
                        .FirstOrDefaultAsync(i => i.Name.ToLower() == ing.IngredientName.ToLower())
                        ?? new Ingredient { Name = ing.IngredientName };

                    recipe.RecipeIngredients.Add(new RecipeIngredient
                    {
                        RecipeId = recipe.Id,
                        IngredientId = ingredient.Id,
                        Quantity = ing.Quantity
                    });
                }
            }

            _dbContext.RecipeSchedules.RemoveRange(recipe.RecipeSchedules);

            if (model.SelectedDays != null)
            {
                var schedules = model.SelectedDays.Select(day => new RecipeSchedule
                {
                    RecipeId = recipe.Id,
                    UserId = recipe.UserId,
                    DayOfWeek = day
                }).ToList();

                _dbContext.RecipeSchedules.AddRange(schedules);
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Usuwa przepis z systemu.
        /// </summary>
        /// <param name="id">Identyfikator przepisu.</param>
        /// <returns>
        /// <c>true</c> jeśli przepis został usunięty, w przeciwnym razie <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe == null)
                return false;

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Filtruje przepisy według składników oraz autora.
        /// </summary>
        /// <param name="userId">Identyfikator aktualnego użytkownika.</param>
        /// <param name="ingredientIds">Lista identyfikatorów składników.</param>
        /// <param name="showMine">Czy pokazywać przepisy użytkownika.</param>
        /// <param name="showOthers">Czy pokazywać przepisy innych użytkowników.</param>
        /// <returns>Lista przefiltrowanych przepisów.</returns>
        public async Task<IEnumerable<RecipeListVM>> FilterAsync(
            int userId,
            List<int> ingredientIds,
            bool showMine,
            bool showOthers)
        {
            var query = _dbContext.Recipes
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .Include(r => r.User)
                .AsQueryable();

            if (ingredientIds.Any())
            {
                query = query.Where(r =>
                    r.RecipeIngredients.Any(ri => ingredientIds.Contains(ri.IngredientId)));
            }

            if (!showMine)
                query = query.Where(r => r.UserId != userId);

            if (!showOthers)
                query = query.Where(r => r.UserId == userId);

            var recipes = await query
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
        }
    }
}
