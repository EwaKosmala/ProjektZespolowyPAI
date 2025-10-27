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
    public async Task<IEnumerable<RecipeListVM>> GetAllAsync()
    {
        var recipes = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.User)
            .ToListAsync();

        return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
    }


    public async Task<int> CreateAsync(CreateRecipeVM model, int userId)
    {
        var recipe = _mapper.Map<Recipe>(model);
        recipe.UserId = userId;
        recipe.CreatedAt = DateTime.Now;

        // 🔹 Składniki
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

        // 🔹 Dni tygodnia (z poprawką)
        if (model.SelectedDays != null && model.SelectedDays.Any())
        {
            var validSchedules = model.SelectedDays.Select(day => new RecipeSchedule
            {
                DayOfWeek = day,
                UserId = recipe.UserId,
                RecipeId = recipe.Id
            }).ToList();

            _dbContext.RecipeSchedules.AddRange(validSchedules);
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

        var model = _mapper.Map<CreateRecipeVM>(recipe);

        // 🔹 Tutaj wypełniamy zaznaczone dni tygodnia
        model.SelectedDays = recipe.RecipeSchedules
            .Select(s => s.DayOfWeek)
            .ToList();

        return model;
    }



    public async Task<bool> UpdateAsync(int id, CreateRecipeVM model)
    {
        var recipe = await _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
            .Include(r => r.RecipeSchedules)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (recipe == null) return false;

        // Aktualizacja podstawowych pól
        recipe.Name = model.Name;
        recipe.Description = model.Description;
        recipe.Instructions = model.Instructions;

        // Składniki
        if (model.Ingredients != null)
        {
            _dbContext.RecipeIngredients.RemoveRange(recipe.RecipeIngredients);

            foreach (var ing in model.Ingredients)
            {
                if (string.IsNullOrWhiteSpace(ing.IngredientName)) continue;

                var existingIngredient = await _dbContext.Ingredients
                    .FirstOrDefaultAsync(i => i.Name.ToLower() == ing.IngredientName.ToLower());

                if (existingIngredient == null)
                {
                    existingIngredient = new Ingredient { Name = ing.IngredientName };
                    _dbContext.Ingredients.Add(existingIngredient);
                    await _dbContext.SaveChangesAsync(); // tutaj może być osobno lub można optymalizować
                }

                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = existingIngredient.Id,
                    Quantity = ing.Quantity
                });
            }
        }

        // Harmonogram
        _dbContext.RecipeSchedules.RemoveRange(recipe.RecipeSchedules);

        if (model.SelectedDays != null && model.SelectedDays.Any())
        {
            var newSchedules = model.SelectedDays.Select(day => new RecipeSchedule
            {
                RecipeId = recipe.Id,
                UserId = recipe.UserId,
                DayOfWeek = day
            }).ToList();

            _dbContext.RecipeSchedules.AddRange(newSchedules);
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

    public async Task<IEnumerable<RecipeListVM>> FilterAsync(int userId, List<int> ingredientIds, bool showMine, bool showOthers)
    {
        var query = _dbContext.Recipes
            .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
            .Include(r => r.User)
            .AsQueryable();

        if (ingredientIds.Any())
        {
            query = query.Where(r => r.RecipeIngredients
                .Any(ri => ingredientIds.Contains(ri.IngredientId)));
        }

        if (!showMine)
            query = query.Where(r => r.UserId != userId);

        if (!showOthers)
            query = query.Where(r => r.UserId == userId);

        var recipes = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();
        return _mapper.Map<IEnumerable<RecipeListVM>>(recipes);
    }
}
