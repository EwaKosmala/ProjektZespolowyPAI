using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lab1_gr1.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly PasswordHasher<User> _passwordHasher;

        public UserService(MyDBContext dbContext, IMapper mapper, PasswordHasher<User> passwordHasher) : base(dbContext, mapper)
        {
            _passwordHasher = passwordHasher;
        }
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

        public async Task<User?> LoginAsync(LoginVM model)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username);

            if (user == null)
                return null;

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
            return result == PasswordVerificationResult.Success ? user : null;
        }

        public void Logout(HttpContext httpContext)
        {
            httpContext.Session.Clear();
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null) return false;

            var verify = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, currentPassword);
            if (verify != PasswordVerificationResult.Success)
                return false; // stare hasło nieprawidłowe

            user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAccountAsync(int userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.ShoppingLists)
                .Include(u => u.RecipeSchedules)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return false;

            // Usuń powiązane dane (listy zakupów, harmonogramy itd.)
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
