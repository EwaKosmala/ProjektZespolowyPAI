using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.RecipeIngredientVM;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    public class RecipeIngredientService : BaseService, IRecipeIngredientService
    {
        public RecipeIngredientService(MyDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
        {
        }
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