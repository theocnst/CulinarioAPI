using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Services.UserServices;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserProfileController : ControllerBase
{
    private readonly IUserProfileService _userProfileService;
    private readonly ILogger<UserProfileController> _logger;

    public UserProfileController(IUserProfileService userProfileService, ILogger<UserProfileController> logger)
    {
        _userProfileService = userProfileService;
        _logger = logger;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserProfile(int id)
    {
        _logger.LogInformation("GetUserProfile called with id: {Id}", id);

        try
        {
            var profile = await _userProfileService.GetUserProfileAsync(id);

            if (profile == null)
            {
                _logger.LogWarning("UserProfile not found for id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("UserProfile found for id: {Id}", id);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting UserProfile for id: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserProfile(int id, [FromBody] UserProfileUpdateDto profileDto)
    {
        _logger.LogInformation("UpdateUserProfile called with id: {Id}", id);

        try
        {
            if (!await _userProfileService.UpdateUserProfileAsync(id, profileDto))
            {
                _logger.LogWarning("Update failed, UserProfile not found for id: {Id}", id);
                return NotFound();
            }

            _logger.LogInformation("UserProfile updated for id: {Id}", id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating UserProfile for id: {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
}
