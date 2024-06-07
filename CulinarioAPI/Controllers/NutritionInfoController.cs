using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinarioAPI.Data;
using CulinarioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class NutritionInfoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public NutritionInfoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{recipeId}")]
    public async Task<IActionResult> GetNutritionInfo(int recipeId)
    {
        var nutritionInfo = await _context.NutritionInfos.SingleOrDefaultAsync(n => n.RecipeId == recipeId);
        if (nutritionInfo == null)
        {
            return NotFound();
        }

        return Ok(nutritionInfo);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddNutritionInfo([FromBody] NutritionInfo nutritionInfo)
    {
        if (ModelState.IsValid)
        {
            _context.NutritionInfos.Add(nutritionInfo);
            await _context.SaveChangesAsync();
            return Ok(nutritionInfo);
        }

        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNutritionInfo(int id, [FromBody] NutritionInfo nutritionInfo)
    {
        if (id != nutritionInfo.NutritionInfoId)
        {
            return BadRequest();
        }

        _context.Entry(nutritionInfo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.NutritionInfos.Any(n => n.NutritionInfoId == id))
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
    public async Task<IActionResult> DeleteNutritionInfo(int id)
    {
        var nutritionInfo = await _context.NutritionInfos.FindAsync(id);
        if (nutritionInfo == null)
        {
            return NotFound();
        }

        _context.NutritionInfos.Remove(nutritionInfo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
