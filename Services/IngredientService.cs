using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.IngredientVM;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    public class IngredientService : BaseService, IIngredientService
    {
        public IngredientService(MyDBContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

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
        public async Task<List<IngredientListVM>> GetUsedIngredientsAsync()
        {
            return await _dbContext.RecipeIngredients
                .Include(ri => ri.Ingredient)
                .Select(ri => ri.Ingredient)
                .Distinct()
                .OrderBy(i => i.Name)
                .Select(i => new IngredientListVM { Id = i.Id, Name = i.Name })
                .ToListAsync();
        }

        public async Task<Ingredient?> GetByIdAsync(int id)
        {
            return await _dbContext.Ingredients.FindAsync(id);
        }

        public async Task<int> CreateAsync(CreateIngredientVM model)
        {
            var ingredient = _mapper.Map<Ingredient>(model);
            _dbContext.Ingredients.Add(ingredient);
            await _dbContext.SaveChangesAsync();
            return ingredient.Id;
        }

        public async Task<bool> UpdateAsync(EditIngredientVM model)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(model.Id);
            if (ingredient == null) return false;

            ingredient.Name = model.Name;
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ingredient = await _dbContext.Ingredients.FindAsync(id);
            if (ingredient == null) return false;

            _dbContext.Ingredients.Remove(ingredient);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}