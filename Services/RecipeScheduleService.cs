using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.ViewModels.RecipeScheduleVM;

namespace lab1_gr1.Services
{
    public class RecipeScheduleService: BaseService, IRecipeScheduleService
    {
        public RecipeScheduleService(MyDBContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<List<DayRecipesVM>> GetRecipesPerWeekAsync(int userId)
        {
            var schedules = await _dbContext.RecipeSchedules
                .Include(rs => rs.Recipe)
                .Where(rs => rs.UserId == userId)
                .ToListAsync();

            var recipesPerDay = schedules
                .GroupBy(rs => rs.DayOfWeek)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(rs => _mapper.Map<RecipeListVM>(rs.Recipe)).ToList()
                );

            return Enumerable.Range(1, 7)
                .Select(day => new DayRecipesVM
                {
                    Day = day,
                    Recipes = recipesPerDay.ContainsKey(day) ? recipesPerDay[day] : new List<RecipeListVM>()
                }).ToList();
        }
    }
}
