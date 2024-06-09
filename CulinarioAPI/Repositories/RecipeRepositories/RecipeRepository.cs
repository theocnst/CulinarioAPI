using CulinarioAPI.Data;
using CulinarioAPI.Models.RecipeModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CulinarioAPI.Repositories.RecipeRepositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecipeRepository> _logger;

        public RecipeRepository(ApplicationDbContext context, ILogger<RecipeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Recipe>> GetAllRecipesAsync()
        {
            _logger.LogInformation("GetAllRecipesAsync called");
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.NutritionInfo)
                .Include(r => r.Country)
                .Include(r => r.Admin)
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            _logger.LogInformation("GetRecipeByIdAsync called with id: {Id}", id);
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.NutritionInfo)
                .Include(r => r.Country)
                .Include(r => r.Admin)
                .SingleOrDefaultAsync(r => r.RecipeId == id);
        }

        public async Task AddRecipeAsync(Recipe recipe)
        {
            _logger.LogInformation("AddRecipeAsync called");
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRecipeAsync(Recipe recipe)
        {
            _logger.LogInformation("UpdateRecipeAsync called");
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipeAsync(int id)
        {
            _logger.LogInformation("DeleteRecipeAsync called with id: {Id}", id);
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Country>> GetAllCountriesAsync()
        {
            _logger.LogInformation("GetAllCountriesAsync called");
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountryByNameAsync(string countryName)
        {
            _logger.LogInformation("GetCountryByNameAsync called with countryName: {CountryName}", countryName);
            return await _context.Countries.SingleOrDefaultAsync(c => c.CountryName == countryName);
        }

        public async Task AddCountryAsync(Country country)
        {
            _logger.LogInformation("AddCountryAsync called with countryName: {CountryName}", country.CountryName);
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
