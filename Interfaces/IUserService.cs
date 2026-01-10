using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using System.Linq.Expressions;

namespace lab1_gr1.Interfaces
{
    /// <summary>
    /// Interfejs definiujący metody serwisu obsługującego użytkowników.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Rejestruje nowego użytkownika w systemie.
        /// </summary>
        /// <param name="model">Model danych nowego użytkownika (<see cref="RegisterVM"/>)</param>
        /// <returns>True jeśli rejestracja powiodła się, w przeciwnym razie false</returns>
        Task<bool> RegisterAsync(RegisterVM model);

        /// <summary>
        /// Loguje użytkownika na podstawie podanego loginu i hasła.
        /// </summary>
        /// <param name="model">Model logowania (<see cref="LoginVM"/>)</param>
        /// <returns>Obiekt <see cref="User"/> jeśli logowanie powiodło się, w przeciwnym razie null</returns>
        Task<User?> LoginAsync(LoginVM model);

        /// <summary>
        /// Wylogowuje użytkownika, czyszcząc sesję HTTP.
        /// </summary>
        /// <param name="httpContext">Kontekst HTTP użytkownika</param>
        void Logout(HttpContext httpContext);

        /// <summary>
        /// Usuwa konto użytkownika z systemu.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>True jeśli konto zostało usunięte, w przeciwnym razie false</returns>
        Task<bool> DeleteAccountAsync(int userId);

        /// <summary>
        /// Zmienia hasło użytkownika.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="currentPassword">Aktualne hasło użytkownika</param>
        /// <param name="newPassword">Nowe hasło użytkownika</param>
        /// <returns>True jeśli hasło zostało zmienione, w przeciwnym razie false</returns>
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}
