using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using System.Linq.Expressions;

namespace lab1_gr1.Interfaces
{
    public interface IUserService
    {
        Task<bool> RegisterAsync(RegisterVM model);
        Task<User?> LoginAsync(LoginVM model);
        void Logout(HttpContext httpContext);
        Task<bool> DeleteAccountAsync(int userId);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);

    }
}
