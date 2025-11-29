using AutoMapper;
using lab1_gr1.Interfaces;
using lab1_gr1.Models;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
using ListaZakupow.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;

namespace lab1_gr1.Services
{
    public class ShoppingListService : BaseService, IShoppingListService
    {
        public ShoppingListService(MyDBContext dbContext, IMapper mapper)
        : base(dbContext, mapper)
        {
        }

        public async Task<List<CreateShoppingListVM>> GetAllByUserIdAsync(int userId)
        {
            var lists = await _dbContext.ShoppingLists
                .Where(sl => sl.UserId == userId)
                .Include(sl => sl.Items)
                    .ThenInclude(i => i.Ingredient)
                .OrderByDescending(sl => sl.CreatedAt)
            .ToListAsync();

            return _mapper.Map<List<CreateShoppingListVM>>(lists);
        }

        public async Task<CreateShoppingListVM?> GetByIdAsync(int id)
        {
            var list = await _dbContext.ShoppingLists
                .Include(sl => sl.Items)
            .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(sl => sl.Id == id);

            return list == null ? null : _mapper.Map<CreateShoppingListVM>(list);
        }

        public async Task<int> CreateAsync(CreateShoppingListVM model, int userId)
        {
            var shoppingList = new ListaZakupow.Model.DataModels.ShoppingList
            {
                UserId = userId,
                CreatedAt = DateTime.Now
            };

            _dbContext.ShoppingLists.Add(shoppingList);
            await _dbContext.SaveChangesAsync();

            var items = model.Items.Select(i => new ListaZakupow.Model.DataModels.ShoppingListItem
            {
                ShoppingListId = shoppingList.Id,
                IngredientId = i.IngredientId,
                Quantity = i.Quantity
            }).ToList();

            _dbContext.ShoppingListItems.AddRange(items);
            await _dbContext.SaveChangesAsync();

            return shoppingList.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var list = await _dbContext.ShoppingLists.FindAsync(id);
            if (list == null) return false;

            _dbContext.ShoppingLists.Remove(list);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CreateShoppingListVM> GenerateFromDaysAsync(int userId, List<int> days)
        {
            var recipes = await _dbContext.RecipeSchedules
                .Where(rs => rs.UserId == userId && days.Contains(rs.DayOfWeek))
                .Include(rs => rs.Recipe)
                    .ThenInclude(r => r.RecipeIngredients)
                        .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            var shoppingItems = new Dictionary<(int, string), CreateShoppingListItemVM>();
            // klucz: (IngredientId, Unit)

            foreach (var schedule in recipes)
            {
                foreach (var ri in schedule.Recipe.RecipeIngredients)
                {
                    var (number, unit) = ParseQuantity(ri.Quantity);

                    var key = (ri.IngredientId, unit);

                    if (shoppingItems.ContainsKey(key))
                    {
                        // sumowanie liczb
                        var existingNumber = ParseQuantity(shoppingItems[key].Quantity).number;
                        shoppingItems[key].Quantity = $"{existingNumber + number} {unit}".Trim();
                    }
                    else
                    {
                        shoppingItems[key] = new CreateShoppingListItemVM
                        {
                            IngredientId = ri.IngredientId,
                            IngredientName = ri.Ingredient.Name,
                            Quantity = ri.Quantity
                        };
                    }
                }
            }

            return new CreateShoppingListVM
            {
                Items = shoppingItems.Values.ToList()
            };
        }

        public async Task<CreateShoppingListVM> GenerateForWeekAsync(int userId)
        {
            var allDays = Enumerable.Range(1, 7).ToList();
            return await GenerateFromDaysAsync(userId, allDays);
        }

        public async Task<bool> UpdateAsync(int id, CreateShoppingListVM model, int userId)
        {
            var list = await _dbContext.ShoppingLists
                .Include(sl => sl.Items)
                .FirstOrDefaultAsync(sl => sl.Id == id && sl.UserId == userId);

            if (list == null) return false;

            // Usuń stare elementy
            _dbContext.ShoppingListItems.RemoveRange(list.Items);

            // Dodaj nowe
            list.Items = model.Items.Select(i => new ShoppingListItem
            {
                ShoppingListId = list.Id,
                IngredientId = i.IngredientId,
                Quantity = i.Quantity
            }).ToList();

            await _dbContext.SaveChangesAsync();
            return true;
        }



        private (double number, string unit) ParseQuantity(string quantity)
        {
            if (string.IsNullOrWhiteSpace(quantity))
                return (0, "");

            quantity = quantity.Trim();
            int i = 0;
            // przechodzimy po znakach, aż znajdziemy pierwszy nie-cyfrowy znak lub kropkę
            while (i < quantity.Length && (char.IsDigit(quantity[i]) || quantity[i] == '.' || quantity[i] == ','))
                i++;

            var numberPart = quantity.Substring(0, i).Replace(',', '.'); // np. "30" lub "0.5"
            var unitPart = quantity.Substring(i).Trim(); // np. "gr", "ml"

            double number = 0;
            double.TryParse(numberPart, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);

            return (number, unitPart);
        }

        public async Task<byte[]> GeneratePdfAsync(int shoppingListId, int userId)
        {
            var list = await _dbContext.ShoppingLists
                .Include(sl => sl.Items)
                    .ThenInclude(i => i.Ingredient)
                .FirstOrDefaultAsync(sl => sl.Id == shoppingListId && sl.UserId == userId);

            if (list == null)
                throw new Exception("Lista zakupów nie została znaleziona.");

            // Tworzenie dokumentu PDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);

                    page.Header()
                        .Text($"🛒 Lista zakupów z dnia {list.CreatedAt:dd.MM.yyyy}\n")
                        .FontSize(20)
                        .Bold()
                        .AlignCenter();

                    page.Content()
                        .Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Text("Składnik").Bold();
                                header.Cell().Text("Ilość").Bold();
                            });

                            foreach (var item in list.Items)
                            {
                                table.Cell().Text(item.Ingredient?.Name ?? "—");
                                table.Cell().Text(item.Quantity);
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text($"Wygenerowano automatycznie — {DateTime.Now:dd.MM.yyyy HH:mm}");
                });
            });

            return document.GeneratePdf();
        }
    }
}