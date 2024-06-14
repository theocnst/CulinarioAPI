using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Authorize]
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

    [HttpGet("{username}")]
    public async Task<IActionResult> GetUserProfile(string username)
    {
        _logger.LogInformation("GetUserProfile called with username: {Username}", username);

        try
        {
            var profile = await _userProfileService.GetUserProfileAsync(username);

            if (profile == null)
            {
                _logger.LogWarning("UserProfile not found for username: {Username}", username);
                return NotFound();
            }

            _logger.LogInformation("UserProfile found for username: {Username}", username);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting UserProfile for username: {Username}", username);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{username}/details")]
    public async Task<IActionResult> GetUserDetails(string username)
    {
        _logger.LogInformation("GetUserDetails called with username: {Username}", username);

        try
        {
            var details = await _userProfileService.GetUserDetailsAsync(username);

            if (details == null)
            {
                _logger.LogWarning("User details not found for username: {Username}", username);
                return NotFound();
            }

            _logger.LogInformation("User details found for username: {Username}", username);
            return Ok(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while getting user details for username: {Username}", username);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{username}")]
    public async Task<IActionResult> UpdateUserProfile(string username, [FromBody] UserProfileUpdateDto profileDto)
    {
        _logger.LogInformation("UpdateUserProfile called with username: {Username}", username);

        try
        {
            if (!await _userProfileService.UpdateUserProfileAsync(username, profileDto))
            {
                _logger.LogWarning("Update failed, UserProfile not found for username: {Username}", username);
                return NotFound();
            }

            _logger.LogInformation("UserProfile updated for username: {Username}", username);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating UserProfile for username: {Username}", username);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("addFriend")]
    public async Task<IActionResult> AddFriend([FromBody] FriendshipDto friendshipDto)
    {
        var success = await _userProfileService.AddFriendAsync(friendshipDto);
        if (success)
        {
            return Ok();
        }

        return BadRequest("Failed to add friend.");
    }

    [HttpPost("removeFriend")]
    public async Task<IActionResult> RemoveFriend([FromBody] FriendshipDto friendshipDto)
    {
        var success = await _userProfileService.RemoveFriendAsync(friendshipDto);
        if (success)
        {
            return Ok();
        }

        return BadRequest("Failed to remove friend.");
    }
}
