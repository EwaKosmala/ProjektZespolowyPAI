using lab1_gr1.ViewModels.RecipeVM;
using lab1_gr1.ViewModels.RecipeScheduleVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu do zarządzania harmonogramem przepisów użytkownika.
    /// </summary>
    public interface IRecipeScheduleService
    {
        /// <summary>
        /// Pobiera przepisy przypisane do każdego dnia tygodnia dla danego użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Widok tygodniowego harmonogramu (<see cref="WeekVM"/>)</returns>
        Task<WeekVM> GetRecipesPerWeekAsync(int userId);

        /// <summary>
        /// Dodaje przepis do harmonogramu w wybranym dniu tygodnia.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="dayOfWeek">Dzień tygodnia (0–6)</param>
        /// <returns>Task reprezentujący operację asynchroniczną</returns>
        Task AddRecipeToDayAsync(int userId, int recipeId, int dayOfWeek);

        /// <summary>
        /// Usuwa przepis z harmonogramu w wybranym dniu tygodnia.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="dayOfWeek">Dzień tygodnia (0–6)</param>
        /// <returns>Task reprezentujący operację asynchroniczną</returns>
        Task RemoveRecipeFromDayAsync(int userId, int recipeId, int dayOfWeek);

        /// <summary>
        /// Pobiera listę wszystkich przepisów dostępnych dla użytkownika,
        /// które mogą zostać dodane do harmonogramu.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Lista widoków przepisów (<see cref="RecipeListVM"/>)</returns>
        Task<List<RecipeListVM>> GetAvailableRecipesForUserAsync(int userId);

        /// <summary>
        /// Dodaje istniejący przepis do harmonogramu użytkownika
        /// w wybranym dniu tygodnia.
        /// </summary>
        /// <param name="recipeId">Identyfikator przepisu</param>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="dayOfWeek">Dzień tygodnia (0–6)</param>
        /// <returns>True jeśli dodanie powiodło się, w przeciwnym razie false</returns>
        Task<bool> AddExistingRecipeToScheduleAsync(int recipeId, int userId, int dayOfWeek);
    }
}
