using CulinarioAPI.Data;
using CulinarioAPI.Models.RecipeModels;
using Microsoft.EntityFrameworkCore;

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
                .ToListAsync();
        }

        public async Task<Recipe> GetRecipeByIdAsync(int id)
        {
            _logger.LogInformation("GetRecipeByIdAsync called with id: {Id}", id);
            return await _context.Recipes
                .Include(r => r.Ingredients)
                .Include(r => r.Instructions)
                .Include(r => r.NutritionInfo)
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
    }
}
