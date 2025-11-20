using lab1_gr1.ViewModels.ShoppingListVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    public interface IShoppingListService
    {
        Task<List<CreateShoppingListVM>> GetAllByUserIdAsync(int userId);
        Task<CreateShoppingListVM?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateShoppingListVM model, int userId);
        Task<bool> DeleteAsync(int id);
        Task<CreateShoppingListVM> GenerateFromDaysAsync(int userId, List<int> days);
        Task<bool> UpdateAsync(int id, CreateShoppingListVM model, int userId);
        Task<byte[]> GeneratePdfAsync(int shoppingListId, int userId);
        Task<CreateShoppingListVM> GenerateForWeekAsync(int userId);
    }
}