using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.IngredientVM;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za zarządzanie składnikami.
    /// Implementuje logikę biznesową związaną z pobieraniem, tworzeniem,
    /// edycją oraz usuwaniem składników.
    /// </summary>
    public class IngredientService : BaseService, IIngredientService
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="IngredientService"/>.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych aplikacji.</param>
        /// <param name="mapper">Obiekt AutoMapper.</param>
        public IngredientService(MyDBContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        /// <summary>
        /// Pobiera listę wszystkich składników posortowanych alfabetycznie.
        /// </summary>
        /// <returns>
        /// Kolekcja składników w postaci <see cref="IngredientListVM"/>.
        /// </returns>
        public async Task<IEnumerable<IngredientListVM>> GetAllAsync()
        {
            return await _dbContext.Ingredients
                .OrderBy(i => i.Name)
                .Select(i => new IngredientListVM
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();
        }

        /// <summary>
        /// Pobiera listę składników, które są wykorzystywane w przepisach.
        /// </summary>
        /// <returns>
        /// Lista unikalnych składników używanych w przepisach.
        /// </returns>
        public async Task<List<IngredientListVM>> GetUsedIngredientsAsync()
        {
            return await _dbContext.RecipeIngredients
                .Include(ri => ri.Ingredient)
                .Select(ri => ri.Ingredient)
                .Distinct()
                .OrderBy(i => i.Name)
                .Select(i => new IngredientListVM
                {
                    Id = i.Id,
                    Name = i.Name
                })
                .ToListAsync();
        }

        /// <summary>
        /// Pobiera składnik na podstawie jego identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator składnika.</param>
        /// <returns>
        /// Obiekt <see cref="Ingredient"/> lub <c>null</c>, jeśli nie istnieje.
        /// </returns>
        public async Task<Ingredient?> GetByIdAsync(int id)
        {
            return await _dbContext.Ingredients.FindAsync(id);
        }

        /// <summary>
        /// Tworzy nowy składnik w systemie.
        /// </summary>
        /// <param name="model">Model danych nowego składnika.</param>
        /// <returns>
        /// Identyfikator nowo utworzonego składnika.
        /// </returns>
        public async Task<int> CreateAsync(CreateIngredientVM model)
        {
            var ingredient = _mapper.Map<Ingredient>(model);
            _dbContext.Ingredients.Add(ingredient);
            await _dbContext.SaveChangesAsync();
            return ingredient.Id;
        }

        /// <summary>
        /// Aktualizuje istniejący składnik.
        /// </summary>
        /// <param name="model">Model edycji składnika.</param>
        /// <returns>
        /// <c>true</c> jeśli aktualizacja się powiodła, w przeciwnym razie <c>false</c>.
        /// </returns>
        public async Task<bool> UpdateAsync(EditIngredientVM model)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(model.Id);
            if (ingredient == null)
                return false;

            ingredient.Name = model.Name;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Usuwa składnik na podstawie identyfikatora.
        /// </summary>
        /// <param name="id">Identyfikator składnika.</param>
        /// <returns>
        /// <c>true</c> jeśli usunięcie się powiodło, w przeciwnym razie <c>false</c>.
        /// </returns>
        public async Task<bool> DeleteAsync(int id)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(id);
            if (ingredient == null)
                return false;

            _dbContext.Ingredients.Remove(ingredient);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
