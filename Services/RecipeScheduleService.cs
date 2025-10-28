using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Services
{
    public class RecipeScheduleService: BaseService, IRecipeScheduleService
    {
        public RecipeScheduleService(MyDBContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public async Task<WeekVM> GetRecipesPerWeekAsync(int userId)
        {
            var schedules = await _dbContext.RecipeSchedules
                .Where(rs => rs.UserId == userId)
                .Include(rs => rs.Recipe)
                .ToListAsync();

            var vm = new WeekVM();
            for (int d = 1; d <= 7; d++)
            {
                var dayName = d switch
                {
                    1 => "Poniedziałek",
                    2 => "Wtorek",
                    3 => "Środa",
                    4 => "Czwartek",
                    5 => "Piątek",
                    6 => "Sobota",
                    7 => "Niedziela",
                    _ => ""
                };

                var list = schedules
                    .Where(s => s.DayOfWeek == d)
                    .Select(s => _mapper.Map<RecipeListVM>(s.Recipe))
                    .ToList();

                vm.Days.Add(new DayRecipesVM
                {
                    Day = d,
                    DayName = dayName,
                    Recipes = list
                });
            }

            return vm; // <--- zwracamy cały WeekVM
        }

        public async Task AddRecipeToDayAsync(int userId, int recipeId, int dayOfWeek)
        {
            // sprawdź czy przepis istnieje i należy do użytkownika (lub jest publiczny — zależnie)
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null) throw new Exception("Przepis nie istnieje.");

            // unikaj duplikatu
            var exists = await _dbContext.RecipeSchedules.AnyAsync(rs =>
                rs.UserId == userId && rs.RecipeId == recipeId && rs.DayOfWeek == dayOfWeek);
            if (exists) return;

            var rs = new RecipeSchedule { UserId = userId, RecipeId = recipeId, DayOfWeek = dayOfWeek };
            _dbContext.RecipeSchedules.Add(rs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveRecipeFromDayAsync(int userId, int recipeId, int dayOfWeek)
        {
            var rs = await _dbContext.RecipeSchedules
                .FirstOrDefaultAsync(r => r.UserId == userId && r.RecipeId == recipeId && r.DayOfWeek == dayOfWeek);
            if (rs == null) return;
            _dbContext.RecipeSchedules.Remove(rs);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<RecipeListVM>> GetAvailableRecipesForUserAsync(int userId)
        {
            var recipes = await _dbContext.Recipes
                .Where(r => r.UserId == userId) // lub include others if chcesz pokazywać publiczne
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<RecipeListVM>>(recipes);
        }

        public async Task<bool> AddExistingRecipeToScheduleAsync(int recipeId, int userId, int dayOfWeek)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return false;

            var existing = await _dbContext.RecipeSchedules
                .FirstOrDefaultAsync(rs => rs.RecipeId == recipeId && rs.UserId == userId && rs.DayOfWeek == dayOfWeek);

            if (existing != null)
                return false; // już zaplanowany na ten dzień

            var schedule = new RecipeSchedule
            {
                RecipeId = recipeId,
                UserId = userId,
                DayOfWeek = dayOfWeek,
            };

            _dbContext.RecipeSchedules.Add(schedule);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        
        

    }
}
