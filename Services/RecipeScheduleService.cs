using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.RecipeVM;
using Microsoft.EntityFrameworkCore;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za zarządzanie harmonogramem przepisów użytkownika
    /// (planowanie posiłków na dni tygodnia).
    /// </summary>
    public class RecipeScheduleService : BaseService, IRecipeScheduleService
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy <see cref="RecipeScheduleService"/>.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych aplikacji.</param>
        /// <param name="mapper">Obiekt AutoMapper.</param>
        public RecipeScheduleService(MyDBContext dbContext, IMapper mapper)
            : base(dbContext, mapper)
        {
        }

        /// <summary>
        /// Pobiera harmonogram przepisów użytkownika dla całego tygodnia.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>
        /// Obiekt <see cref="WeekVM"/> zawierający listę przepisów przypisanych do dni tygodnia.
        /// </returns>
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
                    _ => string.Empty
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

            return vm;
        }

        /// <summary>
        /// Dodaje przepis do harmonogramu użytkownika na określony dzień tygodnia.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <param name="dayOfWeek">Dzień tygodnia (1 = poniedziałek, 7 = niedziela).</param>
        /// <exception cref="Exception">
        /// Rzucany, gdy przepis nie istnieje.
        /// </exception>
        public async Task AddRecipeToDayAsync(int userId, int recipeId, int dayOfWeek)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                throw new Exception("Przepis nie istnieje.");

            var exists = await _dbContext.RecipeSchedules.AnyAsync(rs =>
                rs.UserId == userId &&
                rs.RecipeId == recipeId &&
                rs.DayOfWeek == dayOfWeek);

            if (exists)
                return;

            var rs = new RecipeSchedule
            {
                UserId = userId,
                RecipeId = recipeId,
                DayOfWeek = dayOfWeek
            };

            _dbContext.RecipeSchedules.Add(rs);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Usuwa przepis z harmonogramu użytkownika dla danego dnia.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <param name="dayOfWeek">Dzień tygodnia.</param>
        public async Task RemoveRecipeFromDayAsync(int userId, int recipeId, int dayOfWeek)
        {
            var rs = await _dbContext.RecipeSchedules
                .FirstOrDefaultAsync(r =>
                    r.UserId == userId &&
                    r.RecipeId == recipeId &&
                    r.DayOfWeek == dayOfWeek);

            if (rs == null)
                return;

            _dbContext.RecipeSchedules.Remove(rs);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Pobiera listę przepisów dostępnych dla użytkownika,
        /// które mogą zostać dodane do harmonogramu.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <returns>Lista przepisów w postaci <see cref="RecipeListVM"/>.</returns>
        public async Task<List<RecipeListVM>> GetAvailableRecipesForUserAsync(int userId)
        {
            var recipes = await _dbContext.Recipes
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

            return _mapper.Map<List<RecipeListVM>>(recipes);
        }

        /// <summary>
        /// Dodaje istniejący przepis do harmonogramu użytkownika,
        /// jeśli nie został wcześniej zaplanowany na dany dzień.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu.</param>
        /// <param name="userId">Identyfikator użytkownika.</param>
        /// <param name="dayOfWeek">Dzień tygodnia.</param>
        /// <returns>
        /// <c>true</c> jeśli przepis został dodany,
        /// <c>false</c> jeśli już istniał w harmonogramie lub nie istnieje.
        /// </returns>
        public async Task<bool> AddExistingRecipeToScheduleAsync(int recipeId, int userId, int dayOfWeek)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);
            if (recipe == null)
                return false;

            var existing = await _dbContext.RecipeSchedules
                .FirstOrDefaultAsync(rs =>
                    rs.RecipeId == recipeId &&
                    rs.UserId == userId &&
                    rs.DayOfWeek == dayOfWeek);

            if (existing != null)
                return false;

            var schedule = new RecipeSchedule
            {
                RecipeId = recipeId,
                UserId = userId,
                DayOfWeek = dayOfWeek
            };

            _dbContext.RecipeSchedules.Add(schedule);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
