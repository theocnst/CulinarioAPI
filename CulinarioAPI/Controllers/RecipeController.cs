using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinarioAPI.Data;
using CulinarioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class RecipeController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public RecipeController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var recipes = await _context.Recipes.Include(r => r.Ingredients).Include(r => r.Instructions).ToListAsync();
        return Ok(recipes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _context.Recipes
            .Include(r => r.Ingredients)
            .Include(r => r.Instructions)
            .Include(r => r.NutritionInfo)
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.RecipeId == id);

        if (recipe == null)
        {
            return NotFound();
        }

        return Ok(recipe);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddRecipe([FromBody] Recipe recipe)
    {
        if (ModelState.IsValid)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return Ok(recipe);
        }

        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] Recipe recipe)
    {
        if (id != recipe.RecipeId)
        {
            return BadRequest();
        }

        _context.Entry(recipe).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Recipes.Any(r => r.RecipeId == id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [Authorize]
    [HttpPost("{id}/rate")]
    public async Task<IActionResult> RateRecipe(int id, [FromBody] Rating rating)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null)
        {
            return NotFound();
        }

        rating.RecipeId = id;
        _context.Ratings.Add(rating);

        await _context.SaveChangesAsync();

        return Ok(recipe.StarRating);
    }
}
