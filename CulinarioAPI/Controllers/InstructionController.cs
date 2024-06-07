using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinarioAPI.Data;
using CulinarioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class InstructionController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public InstructionController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("{recipeId}")]
    public async Task<IActionResult> GetInstructions(int recipeId)
    {
        var instructions = await _context.Instructions.Where(i => i.RecipeId == recipeId).ToListAsync();
        return Ok(instructions);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddInstruction([FromBody] Instruction instruction)
    {
        if (ModelState.IsValid)
        {
            _context.Instructions.Add(instruction);
            await _context.SaveChangesAsync();
            return Ok(instruction);
        }

        return BadRequest(ModelState);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInstruction(int id, [FromBody] Instruction instruction)
    {
        if (id != instruction.InstructionId)
        {
            return BadRequest();
        }

        _context.Entry(instruction).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Instructions.Any(i => i.InstructionId == id))
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
    public async Task<IActionResult> DeleteInstruction(int id)
    {
        var instruction = await _context.Instructions.FindAsync(id);
        if (instruction == null)
        {
            return NotFound();
        }

        _context.Instructions.Remove(instruction);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
