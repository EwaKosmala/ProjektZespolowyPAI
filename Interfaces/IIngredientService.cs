using lab1_gr1.ViewModels.IngredientVM;
using ListaZakupow.Model.DataModels;

namespace lab1_gr1.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientListVM>> GetAllAsync();
        Task<Ingredient?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateIngredientVM model);
        Task<bool> UpdateAsync(EditIngredientVM model);
        Task<bool> DeleteAsync(int id);
        Task<List<IngredientListVM>> GetUsedIngredientsAsync();


    }
}