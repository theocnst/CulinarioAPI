//using Azure.Core;
//using Azure;
//using CulinarioAPI.Data;
//using CulinarioAPI.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.EntityFrameworkCore;

//[ApiController]
//[Route("api/[controller]")]
//public class UsersController : ControllerBase
//{
//    private readonly ApplicationDbContext _context;
//    private readonly IConfiguration _configuration;
//    private readonly ILogger<UsersController> _logger;

//    public UsersController(ApplicationDbContext context, IConfiguration configuration, ILogger<UsersController> logger)
//    {
//        _context = context;
//        _configuration = configuration;
//        _logger = logger;
//    }

//    [HttpPost("register")]
//    public async Task<IActionResult> Register([FromBody] User user)
//    {
//        _logger.LogInformation("Register method called.");

//        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
//        {
//            _logger.LogWarning("Registration failed: Email already exists.");
//            return BadRequest("Email already exists.");
//        }

//        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
//        _context.Users.Add(user);
//        await _context.SaveChangesAsync();

//        var token = GenerateJwtToken(user);
//        SetTokenCookie(token);

//        _logger.LogInformation("User registered successfully: {Email}", user.Email);
//        return Ok(new { token, message = "Registration successful" });
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login([FromBody] AuthRequest request)
//    {
//        _logger.LogInformation("Login method called.");

//        var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);
//        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
//        {
//            _logger.LogWarning("Login failed: Invalid credentials.");
//            return Unauthorized("Invalid credentials.");
//        }

//        var token = GenerateJwtToken(user);
//        SetTokenCookie(token);

//        _logger.LogInformation("User logged in successfully: {Email}", request.Email);
//        return Ok(new { token, message = "Login successful" });
//    }

//    [Authorize]
//    [HttpPost("logout")]
//    public IActionResult Logout()
//    {
//        _logger.LogInformation("Logout method called.");

//        Response.Cookies.Delete("jwt");

//        _logger.LogInformation("User logged out successfully.");
//        return Ok(new { message = "Logout successful" });
//    }

//    [Authorize]
//    [HttpGet("authenticated")]
//    public IActionResult Authenticated()
//    {
//        _logger.LogInformation("Authenticated method called.");

//        var authHeader = Request.Headers["Authorization"].ToString();
//        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
//        {
//            _logger.LogWarning("No token provided.");
//            return Unauthorized(new { message = "No token provided." });
//        }

//        var token = authHeader.Substring("Bearer ".Length).Trim();
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

//        try
//        {
//            var validationParameters = new TokenValidationParameters
//            {
//                ValidateIssuerSigningKey = true,
//                IssuerSigningKey = new SymmetricSecurityKey(key),
//                ValidateIssuer = true,
//                ValidateAudience = true,
//                ValidIssuer = _configuration["Jwt:Issuer"],
//                ValidAudience = _configuration["Jwt:Audience"],
//                ClockSkew = TimeSpan.Zero
//            };

//            SecurityToken validatedToken;
//            var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

//            var jwtToken = (JwtSecurityToken)validatedToken;
//            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

//            _logger.LogInformation("Token is valid for user ID: {UserId}", userId);
//            return Ok(new { message = "Token is valid", userId });
//        }
//        catch (SecurityTokenExpiredException ex)
//        {
//            _logger.LogError(ex, "Token has expired.");
//            return Unauthorized(new { message = "Token has expired." });
//        }
//        catch (SecurityTokenInvalidSignatureException ex)
//        {
//            _logger.LogError(ex, "Invalid token signature.");
//            return Unauthorized(new { message = "Invalid token signature." });
//        }
//        catch (SecurityTokenException ex)
//        {
//            _logger.LogError(ex, "Token validation failed.");
//            return Unauthorized(new { message = "Token validation failed." });
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex, "Unexpected error during token validation.");
//            return StatusCode(500, new { message = "Unexpected error during token validation." });
//        }
//    }


//    private void SetTokenCookie(string token)
//    {
//        var cookieOptions = new CookieOptions
//        {
//            HttpOnly = true,
//            Expires = DateTime.UtcNow.AddMinutes(1),
//            Secure = true,
//            SameSite = SameSiteMode.None // Updated to None
//        };
//        Response.Cookies.Append("jwt", token, cookieOptions);
//        _logger.LogInformation("Token set in cookie: {Token}", token);
//    }

//    private string GenerateJwtToken(User user)
//    {
//        var claims = new[]
//        {
//            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
//            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
//        };

//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var token = new JwtSecurityToken(
//            issuer: _configuration["Jwt:Issuer"],
//            audience: _configuration["Jwt:Audience"],
//            claims: claims,
//            expires: DateTime.UtcNow.AddMinutes(1),
//            signingCredentials: creds
//        );

//        _logger.LogInformation("JWT token generated for user: {Email}", user.Email);
//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}
