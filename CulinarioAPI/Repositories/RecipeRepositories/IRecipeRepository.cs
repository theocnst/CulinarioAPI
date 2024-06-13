using CulinarioAPI.Models.RecipeModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CulinarioAPI.Repositories.RecipeRepositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetAllRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int id);
        Task AddRecipeAsync(Recipe recipe);
        Task UpdateRecipeAsync(Recipe recipe);
        Task DeleteRecipeAsync(int id);
        Task<IEnumerable<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByNameAsync(string countryName);
        Task AddCountryAsync(Country country);
        Task SaveChangesAsync();
        Task<Rating> GetRatingByUserAndRecipeAsync(string username, int recipeId);
        Task AddRatingAsync(Rating rating);
        Task UpdateRatingAsync(Rating rating);
    }
}
