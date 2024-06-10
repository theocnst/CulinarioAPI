using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Services.UserServices;

[ApiController]
[Route("api/[controller]")]
public class UserCredentialsController : ControllerBase
{
    private readonly IUserCredentialsService _userCredentialsService;
    private readonly ILogger<UserCredentialsController> _logger;

    public UserCredentialsController(IUserCredentialsService userCredentialsService, ILogger<UserCredentialsController> logger)
    {
        _userCredentialsService = userCredentialsService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDto userDto)
    {
        _logger.LogInformation("Register method called.");

        try
        {
            if (!await _userCredentialsService.IsUsernameUniqueAsync(userDto.Username))
            {
                _logger.LogWarning("Registration failed: Username already exists.");
                return BadRequest("Username already exists.");
            }

            if (!await _userCredentialsService.RegisterUserAsync(userDto))
            {
                _logger.LogWarning("Registration failed: Email already exists.");
                return BadRequest("Email already exists.");
            }

            var token = await _userCredentialsService.AuthenticateUserAsync(new AuthRequestDto { Email = userDto.Email, Password = userDto.Password });
            if (token == null)
            {
                _logger.LogError("Registration failed: Token generation failed.");
                return BadRequest("Registration failed.");
            }

            SetTokenCookie(token);

            _logger.LogInformation("User registered successfully: {Email}", userDto.Email);
            return Ok(new { token, message = "Registration successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during registration for email: {Email}", userDto.Email);
            return StatusCode(500, "Internal server error");
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequestDto request)
    {
        _logger.LogInformation("Login method called for email: {Email}", request.Email);

        try
        {
            var token = await _userCredentialsService.AuthenticateUserAsync(request);
            if (token == null)
            {
                _logger.LogWarning("Login failed: Invalid credentials for email: {Email}", request.Email);
                return Unauthorized("Invalid credentials.");
            }

            SetTokenCookie(token);

            _logger.LogInformation("User logged in successfully: {Email}", request.Email);
            return Ok(new { token, message = "Login successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during login for email: {Email}", request.Email);
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        _logger.LogInformation("Logout method called.");

        try
        {
            var result = await _userCredentialsService.LogoutUserAsync();
            if (!result)
            {
                _logger.LogError("Logout failed.");
                return BadRequest("Logout failed.");
            }

            Response.Cookies.Delete("jwt");

            _logger.LogInformation("User logged out successfully.");
            return Ok(new { message = "Logout successful" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during logout.");
            return StatusCode(500, "Internal server error");
        }
    }

    [Authorize]
    [HttpGet("authenticated")]
    public async Task<IActionResult> Authenticated()
    {
        _logger.LogInformation("Authenticated method called.");

        try
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                _logger.LogWarning("No token provided.");
                return Unauthorized(new { message = "No token provided." });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var isValid = await _userCredentialsService.IsTokenValidAsync(token);
            if (!isValid)
            {
                _logger.LogWarning("Invalid token.");
                return Unauthorized(new { message = "Invalid token." });
            }

            _logger.LogInformation("Token is valid.");
            return Ok(new { message = "Token is valid" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during token validation.");
            return StatusCode(500, "Internal server error");
        }
    }

    private void SetTokenCookie(string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTime.UtcNow.AddMinutes(15), // Ensure the cookie expiration matches token expiration
            Secure = true,
            SameSite = SameSiteMode.None // Updated to None
        };
        Response.Cookies.Append("jwt", token, cookieOptions);
        _logger.LogInformation("Token set in cookie: {Token}", token);
    }
}
