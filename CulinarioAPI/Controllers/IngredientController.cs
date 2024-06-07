using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinarioAPI.Data;
using CulinarioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class IngredientController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public IngredientController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{recipeId}")]
    public async Task<IActionResult> GetIngredients(int recipeId)
    {
        var ingredients = await _context.Ingredients.Where(i => i.RecipeId == recipeId).ToListAsync();
        return Ok(ingredients);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddIngredient([FromBody] Ingredient ingredient)
    {
        if (ModelState.IsValid)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return Ok(ingredient);
        }

        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, [FromBody] Ingredient ingredient)
    {
        if (id != ingredient.IngredientId)
        {
            return BadRequest();
        }

        _context.Entry(ingredient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Ingredients.Any(i => i.IngredientId == id))
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
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null)
        {
            return NotFound();
        }

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
