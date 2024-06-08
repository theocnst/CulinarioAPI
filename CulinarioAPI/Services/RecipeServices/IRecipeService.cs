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
        Task<RecipeDto> AddRecipeAsync(RecipeCreateDto recipeCreateDto); // Changed to return RecipeDto
        Task UpdateRecipeAsync(int id, RecipeCreateDto recipeCreateDto);
        Task DeleteRecipeAsync(int id);
    }
}
