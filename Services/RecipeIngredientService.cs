using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.RecipeIngredientVM;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za zarządzanie składnikami przypisanymi do przepisów.
    /// Umożliwia pobieranie, dodawanie oraz usuwanie składników z przepisu.
    /// </summary>
    public class RecipeIngredientService : BaseService, IRecipeIngredientService
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="RecipeIngredientService"/>.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych aplikacji.</param>
        /// <param name="mapper">Obiekt AutoMapper.</param>
        public RecipeIngredientService(MyDBContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        /// <summary>
        /// Pobiera listę składników przypisanych do określonego przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <returns>
        /// Kolekcja składników przepisu w postaci <see cref="CreateRecipeIngredientVM"/>.
        /// </returns>
        public async Task<IEnumerable<CreateRecipeIngredientVM>> GetIngredientsForRecipeAsync(int recipeId)
        {
            var recipeIngredients = await _dbContext.RecipeIngredients
                .Include(ri => ri.Ingredient)
                .Where(ri => ri.RecipeId == recipeId)
                .ToListAsync();

            return recipeIngredients.Select(ri => new CreateRecipeIngredientVM
            {
                RecipeId = ri.RecipeId,
                IngredientId = ri.IngredientId,
                IngredientName = ri.Ingredient?.Name ?? "Nieznany składnik",
                Quantity = ri.Quantity
            });
        }

        /// <summary>
        /// Dodaje składnik do przepisu.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <param name="ingredientId">Identyfikator składnika.</param>
        /// <param name="quantity">Ilość składnika.</param>
        /// <exception cref="Exception">
        /// Rzucany, gdy przepis lub składnik nie istnieje.
        /// </exception>
        public async Task AddIngredientToRecipeAsync(int recipeId, int ingredientId, string quantity)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            var ingredient = await _dbContext.Ingredients.FindAsync(ingredientId);

            if (recipe == null)
                throw new Exception("Nie znaleziono przepisu.");

            if (ingredient == null)
                throw new Exception("Nie znaleziono składnika.");

            var recipeIngredient = new RecipeIngredient
            {
                RecipeId = recipeId,
                IngredientId = ingredientId,
                Quantity = quantity
            };

            _dbContext.RecipeIngredients.Add(recipeIngredient);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Usuwa składnik z przepisu.
        /// Jeśli składnik nie jest już używany w żadnym przepisie,
        /// zostaje również usunięty z bazy danych.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <param name="ingredientId">Identyfikator składnika.</param>
        /// <exception cref="Exception">
        /// Rzucany, gdy składnik nie jest przypisany do przepisu.
        /// </exception>
        public async Task RemoveIngredientFromRecipeAsync(int recipeId, int ingredientId)
        {
            var recipeIngredient = await _dbContext.RecipeIngredients
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeId && ri.IngredientId == ingredientId);

            if (recipeIngredient == null)
                throw new Exception("Nie znaleziono składnika w przepisie.");

            _dbContext.RecipeIngredients.Remove(recipeIngredient);
            await _dbContext.SaveChangesAsync();

            var usedCount = await _dbContext.RecipeIngredients
                .CountAsync(ri => ri.IngredientId == ingredientId);

            if (usedCount == 0)
            {
                var ingredient = await _dbContext.Ingredients.FindAsync(ingredientId);
                if (ingredient != null)
                {
                    _dbContext.Ingredients.Remove(ingredient);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
