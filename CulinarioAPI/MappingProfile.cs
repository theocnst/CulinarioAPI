using AutoMapper;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Models.RecipeModels;
using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<UserRegistrationDto, UserCredentials>();
            CreateMap<UserProfile, UserProfileDto>();
            CreateMap<UserProfileDto, UserProfile>();
            CreateMap<UserProfileUpdateDto, UserProfile>();

            // Recipe Mappings
            CreateMap<Recipe, RecipeDto>()
                .ForMember(dest => dest.RecipeType, opt => opt.MapFrom(src => new RecipeTypeDto { Name = src.RecipeType.ToString() }))
                .ForMember(dest => dest.AdminUsername, opt => opt.MapFrom(src => src.Admin.Username))
                .ForMember(dest => dest.NutritionInfo, opt => opt.MapFrom(src => src.NutritionInfo));

            CreateMap<RecipeCreateDto, Recipe>()
                .ForMember(dest => dest.RecipeType, opt => opt.MapFrom(src => Enum.Parse<RecipeType>(src.RecipeType.Name)));
            
            CreateMap<RecipeTypeDto, RecipeType>().ConvertUsing(src => (RecipeType)Enum.Parse(typeof(RecipeType), src.Name, true));
           
            // Ingredient Mappings
            CreateMap<Ingredient, IngredientDto>().ReverseMap();
            CreateMap<IngredientCreateDto, Ingredient>();

            // Instruction Mappings
            CreateMap<Instruction, InstructionDto>().ReverseMap();
            CreateMap<InstructionCreateDto, Instruction>();

            // NutritionInfo Mappings
            CreateMap<NutritionInfo, NutritionInfoDto>().ReverseMap();
            CreateMap<NutritionInfoCreateDto, NutritionInfo>();

            // Country Mappings
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}
