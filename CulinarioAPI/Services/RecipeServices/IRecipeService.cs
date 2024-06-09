using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CulinarioAPI.Services.RecipeServices
{
    public interface IRecipeService
    {
        Task<IEnumerable<RecipeDto>> GetAllRecipesAsync();
        Task<RecipeDto> GetRecipeByIdAsync(int id);
        Task<RecipeDto> AddRecipeAsync(RecipeCreateDto recipeCreateDto);
        Task UpdateRecipeAsync(int id, RecipeCreateDto recipeCreateDto);
        Task DeleteRecipeAsync(int id);
        Task<IEnumerable<CountryDto>> GetAllCountriesAsync(); // New method
        IEnumerable<RecipeTypeDto> GetRecipeTypes(); // Updated method
    }
}
