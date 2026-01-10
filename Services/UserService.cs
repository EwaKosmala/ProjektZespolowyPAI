using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    /// <summary>
    /// Serwis odpowiedzialny za obsługę użytkowników systemu,
    /// w tym rejestrację, logowanie, zmianę hasła oraz usuwanie konta.
    /// </summary>
    public class UserService : BaseService, IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        /// <summary>
        /// Inicjalizuje nową instancję serwisu użytkowników.
        /// </summary>
        /// <param name="dbContext">Kontekst bazy danych</param>
        /// <param name="mapper">Mapper AutoMapper</param>
        /// <param name="passwordHasher">Hasher haseł użytkowników</param>
        public UserService(
            MyDBContext dbContext,
            IMapper mapper,
            PasswordHasher<User> passwordHasher)
            : base(dbContext, mapper)
        {
            _passwordHasher = passwordHasher;
        }

        /// <summary>
        /// Rejestruje nowego użytkownika w systemie.
        /// </summary>
        /// <param name="model">Model danych rejestracyjnych</param>
        /// <returns>
        /// True, jeśli rejestracja zakończyła się sukcesem;
        /// false, jeśli użytkownik o podanej nazwie już istnieje
        /// </returns>
        public async Task<bool> RegisterAsync(RegisterVM model)
        {
            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (existingUser != null)
                return false;

            var newUser = new User
            {
                Username = model.Username,
                PasswordHash = _passwordHasher.HashPassword(null, model.Password),
                RegistrationDate = DateTime.Now
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Próbuje zalogować użytkownika na podstawie danych logowania.
        /// </summary>
        /// <param name="model">Model danych logowania</param>
        /// <returns>
        /// Obiekt użytkownika w przypadku poprawnych danych;
        /// null, jeśli logowanie się nie powiodło
        /// </returns>
        public async Task<User?> LoginAsync(LoginVM model)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                model.Password);

            return result == PasswordVerificationResult.Success
                ? user
                : null;
        }

        /// <summary>
        /// Wylogowuje aktualnie zalogowanego użytkownika,
        /// czyszcząc dane sesji.
        /// </summary>
        /// <param name="httpContext">Aktualny kontekst HTTP</param>
        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

        /// <summary>
        /// Zmienia hasło użytkownika po wcześniejszej weryfikacji
        /// aktualnego hasła.
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <param name="currentPassword">Aktualne hasło</param>
        /// <param name="newPassword">Nowe hasło</param>
        /// <returns>
        /// True, jeśli hasło zostało zmienione poprawnie;
        /// false, jeśli użytkownik nie istnieje lub stare hasło jest nieprawidłowe
        /// </returns>
        public async Task<bool> ChangePasswordAsync(
            int userId,
            string currentPassword,
            string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return false;

            var verify = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                currentPassword);

            if (verify != PasswordVerificationResult.Success)
                return false;

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Usuwa konto użytkownika wraz z powiązanymi danymi
        /// (listami zakupów oraz harmonogramami przepisów).
        /// </summary>
        /// <param name="userId">Identyfikator użytkownika</param>
        /// <returns>
        /// True, jeśli konto zostało usunięte;
        /// false, jeśli użytkownik nie istnieje
        /// </returns>
        public async Task<bool> DeleteAccountAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.ShoppingLists)
                .Include(u => u.RecipeSchedules)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            if (user.ShoppingLists != null)
                _dbContext.ShoppingLists.RemoveRange(user.ShoppingLists);

            if (user.RecipeSchedules != null)
                _dbContext.RecipeSchedules.RemoveRange(user.RecipeSchedules);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
