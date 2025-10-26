using lab1_gr1.Interfaces;
using ListaZakupow.Model.DataModels;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.Models;
using AutoMapper;
using lab1_gr1.Services;
using Microsoft.Extensions.Configuration.UserSecrets;

public class RecipeService : BaseService, IRecipeService
{
    public RecipeService(MyDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
    {
    }

    public async Task<Recipe?> GetByIdAsync(int id)
    {
        return await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.RecipeSchedules)
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == id);
    }
    public async Task<IEnumerable<RecipeListVM>> GetAllByUserIdAsync(int userId)
    {
        var recipes = await _dbContext.Recipes
            .Where(r => r.UserId == userId)
            .Include(r => r.RecipeIngredients)
            .Include(r => r.RecipeSchedules)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
    }

    public async Task<int> CreateAsync(CreateRecipeVM model, int userId)
    {
        var recipe = _mapper.Map<Recipe>(model);
        recipe.UserId = userId;
        recipe.CreatedAt = DateTime.Now;

        _dbContext.Recipes.Add(recipe);
        await _dbContext.SaveChangesAsync(); 

        recipe.RecipeIngredients = new List<RecipeIngredient>();
        foreach (var ing in model.Ingredients)
        {
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
                RecipeId = recipe.Id,
                IngredientId = existingIngredient.Id,
                Quantity = ing.Quantity
            });
        }

        if (recipe.RecipeIngredients.Any())
        {
            _dbContext.RecipeIngredients.AddRange(recipe.RecipeIngredients);
            await _dbContext.SaveChangesAsync();
        }

        if (model.Schedules != null && model.Schedules.Any())
        {
            recipe.RecipeSchedules = model.Schedules.Select(s => new RecipeSchedule
            {
                DayOfWeek = s.DayOfWeek,
                RecipeId = recipe.Id,
                UserId = userId
            }).ToList();

            _dbContext.RecipeSchedules.AddRange(recipe.RecipeSchedules);
            await _dbContext.SaveChangesAsync();
        }

        return recipe.Id;
    }

    public async Task<CreateRecipeVM?> GetForEditAsync(int id)
    {
        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.RecipeSchedules)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null)
            return null;

        return _mapper.Map<CreateRecipeVM>(recipe);
    }


    public async Task<bool> UpdateAsync(int id, CreateRecipeVM model)
    {
        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
            .Include(r => r.RecipeSchedules)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return false;

        recipe.Name = model.Name;
        recipe.Description = model.Description;
        recipe.Instructions = model.Instructions;

        if (model.Ingredients != null)
        {
            _dbContext.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

            var newRecipeIngredients = new List<RecipeIngredient>();

            foreach (var ing in model.Ingredients)
            {
                var existingIngredient = await _dbContext.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == ing.IngredientName.ToLower());

                if (existingIngredient == null)
                {
                    existingIngredient = new Ingredient { Name = ing.IngredientName };
                    _dbContext.Ingredients.Add(existingIngredient);
                    await _dbContext.SaveChangesAsync();
                }

                newRecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = existingIngredient.Id,
                    Quantity = ing.Quantity
                });
            }

            recipe.RecipeIngredients = newRecipeIngredients;
        }

        if (model.Schedules != null)
        {
            _dbContext.RecipeSchedules.RemoveRange(recipe.RecipeSchedules);
            recipe.RecipeSchedules = model.Schedules.Select(s => new RecipeSchedule
            {
                DayOfWeek = s.DayOfWeek,
                UserId = recipe.UserId,
                RecipeId = recipe.Id
            }).ToList();
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }


    public async Task<bool> DeleteAsync(int id)
    {
        var recipe = await _dbContext.Recipes.FindAsync(id);
        if (recipe == null) return false;

        _dbContext.Recipes.Remove(recipe);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
