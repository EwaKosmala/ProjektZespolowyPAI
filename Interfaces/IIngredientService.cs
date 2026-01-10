using lab1_gr1.ViewModels.IngredientVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu do obsługi składników.
    /// </summary>
    public interface IIngredientService
    {
        /// <summary>
        /// Pobiera wszystkie składniki w formie widoku listy.
        /// </summary>
        /// <returns>Lista widoków składników (<see cref="IngredientListVM"/>)</returns>
        Task<IEnumerable<IngredientListVM>> GetAllAsync();

        /// <summary>
        /// Pobiera szczegóły składnika po jego identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator składnika</param>
        /// <returns>
        /// Obiekt <see cref="Ingredient"/> jeśli znaleziono, w przeciwnym razie null
        /// </returns>
        Task<Ingredient?> GetByIdAsync(int id);

        /// <summary>
        /// Tworzy nowy składnik w bazie danych.
        /// </summary>
        /// <param name="model">Model danych nowego składnika (<see cref="CreateIngredientVM"/>)</param>
        /// <returns>Identyfikator nowo utworzonego składnika</returns>
        Task<int> CreateAsync(CreateIngredientVM model);

        /// <summary>
        /// Aktualizuje istniejący składnik.
        /// </summary>
        /// <param name="model">Model danych do aktualizacji (<see cref="EditIngredientVM"/>)</param>
        /// <returns>True jeśli aktualizacja powiodła się, w przeciwnym razie false</returns>
        Task<bool> UpdateAsync(EditIngredientVM model);

        /// <summary>
        /// Usuwa składnik po jego identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator składnika do usunięcia</param>
        /// <returns>True jeśli usunięto składnik, w przeciwnym razie false</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Pobiera listę składników, które zostały użyte w przepisach.
        /// </summary>
        /// <returns>Lista widoków składników (<see cref="IngredientListVM"/>) używanych w przepisach</returns>
        Task<List<IngredientListVM>> GetUsedIngredientsAsync();
    }
}
