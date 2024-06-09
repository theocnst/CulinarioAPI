using AutoMapper;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Models.RecipeModels;
using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistrationDto, UserCredentials>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfileUpdateDto, UserProfile>();

            CreateMap<Recipe, RecipeDto>();
            CreateMap<RecipeCreateDto, Recipe>();
            CreateMap<Ingredient, IngredientDto>().ReverseMap();
            CreateMap<IngredientCreateDto, Ingredient>();
            CreateMap<Instruction, InstructionDto>().ReverseMap();
            CreateMap<InstructionCreateDto, Instruction>();
            CreateMap<NutritionInfo, NutritionInfoDto>().ReverseMap();
            CreateMap<NutritionInfoCreateDto, NutritionInfo>();

            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}
