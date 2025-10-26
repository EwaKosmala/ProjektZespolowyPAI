using AutoMapper;
using lab1_gr1.ViewModels.RecipeVM;
using lab1_gr1.ViewModels.RecipeIngredientVM;
using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using lab1_gr1.ViewModels.IngredientVM;
using lab1_gr1.ViewModels.ShoppingListItemVM;
using lab1_gr1.ViewModels.ShoppingListVM;
namespace lab1_gr1.Configuration.AMProfile
{
    public class MainProfile:Profile
    {
        public MainProfile()
        {
            CreateMap<CreateRecipeVM, Recipe>();
            CreateMap<CreateRecipeIngredientVM, RecipeIngredient>();
            CreateMap<Recipe, RecipeListVM>();
            CreateMap<LoginVM, User>();
            CreateMap<Ingredient, IngredientListVM>();
            CreateMap<CreateIngredientVM, Ingredient>();
            CreateMap<EditIngredientVM, Ingredient>();


            CreateMap<Recipe, CreateRecipeVM>()
                .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.RecipeIngredients))
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.RecipeSchedules));

            CreateMap<RecipeIngredient, CreateRecipeIngredientVM>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<RecipeSchedule, CreateRecipeScheduleVM>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek));

            CreateMap<Ingredient, IngredientListVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Ingredient, IngredientListVM>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.UsedInRecipesCount, opt => opt.MapFrom(src => src.RecipeIngredients.Count))
                .ForMember(dest => dest.ShoppingListItemsCount, opt => opt.MapFrom(src => src.ShoppingListItems.Count));

            CreateMap<ShoppingList, CreateShoppingListVM>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


            CreateMap<ShoppingListItem, CreateShoppingListItemVM>()
                .ForMember(dest => dest.IngredientId, opt => opt.MapFrom(src => src.IngredientId))
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.Name))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());


        }
    }
}
