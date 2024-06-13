using CulinarioAPI.Dtos;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;

namespace CulinarioAPI.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto> GetRecipeByIdAsync(int id);
        Task<RecipeDto> AddRecipeAsync(RecipeCreateDto recipeCreateDto);
        Task UpdateRecipeAsync(int id, RecipeCreateDto recipeCreateDto);
        Task DeleteRecipeAsync(int id);
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync();
        IEnumerable<RecipeTypeDto> GetRecipeTypes();
        Task<RecipeDto> RateRecipeAsync(RatingDto ratingDto);
    }
}
