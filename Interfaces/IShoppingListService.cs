using lab1_gr1.ViewModels.ShoppingListVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu do obsługi list zakupów użytkownika.
    /// </summary>
    public interface IShoppingListService
    {
        /// <summary>
        /// Pobiera wszystkie listy zakupów przypisane do użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Lista widoków list zakupów (<see cref="CreateShoppingListVM"/>)</returns>
        Task<List<CreateShoppingListVM>> GetAllByUserIdAsync(int userId);

        /// <summary>
        /// Pobiera szczegóły listy zakupów po jej identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów</param>
        /// <returns>Obiekt <see cref="CreateShoppingListVM"/> jeśli znaleziono, w przeciwnym razie null</returns>
        Task<CreateShoppingListVM?> GetByIdAsync(int id);

        /// <summary>
        /// Tworzy nową listę zakupów przypisaną do użytkownika.
        /// </summary>
        /// <param name="model">Model danych nowej listy zakupów (<see cref="CreateShoppingListVM"/>)</param>
        /// <param name="userId">Identyfikator użytkownika tworzącego listę</param>
        /// <returns>Identyfikator nowo utworzonej listy zakupów</returns>
        Task<int> CreateAsync(CreateShoppingListVM model, int userId);

        /// <summary>
        /// Usuwa listę zakupów po jej identyfikatorze.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów do usunięcia</param>
        /// <returns>True jeśli usunięto listę, w przeciwnym razie false</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Generuje listę zakupów na podstawie przepisów zaplanowanych w wybranych dniach.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="days">Lista dni tygodnia (0–6), dla których generować listę</param>
        /// <returns>Model wygenerowanej listy zakupów (<see cref="CreateShoppingListVM"/>)</returns>
        Task<CreateShoppingListVM> GenerateFromDaysAsync(int userId, List<int> days);

        /// <summary>
        /// Aktualizuje istniejącą listę zakupów.
        /// </summary>
        /// <param name="id">Identyfikator listy zakupów do aktualizacji</param>
        /// <param name="model">Model danych do aktualizacji (<see cref="CreateShoppingListVM"/>)</param>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>True jeśli aktualizacja powiodła się, w przeciwnym razie false</returns>
        Task<bool> UpdateAsync(int id, CreateShoppingListVM model, int userId);

        /// <summary>
        /// Generuje plik PDF z listą zakupów.
        /// </summary>
        /// <param name="shoppingListId">Identyfikator listy zakupów</param>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Tablica bajtów reprezentująca plik PDF</returns>
        Task<byte[]> GeneratePdfAsync(int shoppingListId, int userId);

        /// <summary>
        /// Generuje listę zakupów obejmującą cały tydzień na podstawie harmonogramu użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>Model wygenerowanej listy zakupów (<see cref="CreateShoppingListVM"/>)</returns>
        Task<CreateShoppingListVM> GenerateForWeekAsync(int userId);
    }
}
