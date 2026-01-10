using lab1_gr1.ViewModels.RecipeVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu obsługującego przepisy użytkowników.
    /// </summary>
    public interface IRecipeService
    {
        /// <summary>
        /// Pobiera szczegóły przepisu po jego identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <returns>Obiekt <see cref="Recipe"/> jeśli znaleziono, w przeciwnym razie null</returns>
        Task<Recipe?> GetByIdAsync(int id);

        /// <summary>
        /// Pobiera wszystkie przepisy przypisane do danego użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Kolekcja widoków listy przepisów (<see cref="RecipeListVM"/>)</returns>
        Task<IEnumerable<RecipeListVM>> GetAllByUserIdAsync(int userId);

        /// <summary>
        /// Tworzy nowy przepis przypisany do użytkownika.
        /// </summary>
        /// <param name="model">Model danych nowego przepisu (<see cref="CreateRecipeVM"/>)</param>
        /// <param name="userId">Identyfikator użytkownika tworzącego przepis</param>
        /// <returns>Identyfikator nowo utworzonego przepisu</returns>
        Task<int> CreateAsync(CreateRecipeVM model, int userId);

        /// <summary>
        /// Aktualizuje istniejący przepis.
        /// </summary>
        /// <param name="id">Identyfikator przepisu do aktualizacji</param>
        /// <param name="model">Model danych do aktualizacji (<see cref="CreateRecipeVM"/>)</param>
        /// <returns>True jeśli aktualizacja powiodła się, w przeciwnym razie false</returns>
        Task<bool> UpdateAsync(int id, CreateRecipeVM model);

        /// <summary>
        /// Usuwa przepis po jego identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator przepisu do usunięcia</param>
        /// <returns>True jeśli usunięto przepis, w przeciwnym razie false</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Pobiera dane przepisu potrzebne do edycji.
        /// </summary>
        /// <param name="id">Identyfikator przepisu</param>
        /// <returns>Model do edycji (<see cref="CreateRecipeVM"/>) lub null, jeśli przepis nie istnieje</returns>
        Task<CreateRecipeVM?> GetForEditAsync(int id);

        /// <summary>
        /// Pobiera wszystkie przepisy w systemie.
        /// </summary>
        /// <returns>Kolekcja widoków listy przepisów (<see cref="RecipeListVM"/>)</returns>
        Task<IEnumerable<RecipeListVM>> GetAllAsync();

        /// <summary>
        /// Filtruje przepisy użytkownika na podstawie składników i widoczności.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="ingredientIds">Lista identyfikatorów składników do filtrowania</param>
        /// <param name="showMine">Czy pokazać przepisy użytkownika</param>
        /// <param name="showOthers">Czy pokazać przepisy innych użytkowników</param>
        /// <returns>Kolekcja widoków listy przepisów (<see cref="RecipeListVM"/>)</returns>
        Task<IEnumerable<RecipeListVM>> FilterAsync(int userId, List<int> ingredientIds, bool showMine, bool showOthers);
    }
}
