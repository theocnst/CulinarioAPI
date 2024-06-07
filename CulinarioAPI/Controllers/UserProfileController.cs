using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CulinarioAPI.Data;
using CulinarioAPI.Models;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserProfileController> _logger;

    public UserProfileController(ApplicationDbContext context, ILogger<UserProfileController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(int id)
    {
        var profile = await _context.UserProfiles
            .Include(p => p.Ratings)
            .Include(p => p.Friends)
            .Include(p => p.LikedRecipes)
            .SingleOrDefaultAsync(p => p.UserId == id);

        if (profile == null)
        {
            return NotFound();
        }

        return Ok(profile);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserProfile profile)
    {
        if (id != profile.UserProfileId)
        {
            return BadRequest();
        }

        _context.Entry(profile).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.UserProfiles.Any(p => p.UserProfileId == id))
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
}
