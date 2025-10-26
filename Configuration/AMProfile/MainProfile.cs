using AutoMapper;
using lab1_gr1.ViewModels.RecipeVM;
using lab1_gr1.ViewModels.RecipeIngredientVM;
using lab1_gr1.ViewModels.UserVM;
using ListaZakupow.Model.DataModels;
using lab1_gr1.ViewModels.IngredientVM;
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

        }
    }
}
