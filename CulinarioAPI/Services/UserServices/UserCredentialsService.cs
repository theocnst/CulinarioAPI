using AutoMapper;
using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Models;
using CulinarioAPI.Models.UserModels;
using CulinarioAPI.Repositories.UserRepositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CulinarioAPI.Services.UserServices
{
    public class UserCredentialsService : IUserCredentialsService
    {
        private readonly IUserCredentialsRepository _userCredentialsRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserCredentialsService> _logger;

        public UserCredentialsService(
            IUserCredentialsRepository userCredentialsRepository,
            IUserProfileRepository userProfileRepository,
            IMapper mapper,
            IConfiguration configuration,
            ILogger<UserCredentialsService> logger)
        {
            _userCredentialsRepository = userCredentialsRepository;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> RegisterUserAsync(UserRegistrationDto userDto)
        {
            _logger.LogInformation("RegisterUserAsync called.");

            try
            {
                if (await _userCredentialsRepository.UserExistsAsync(userDto.Email))
                {
                    _logger.LogWarning("Registration failed: Email already exists.");
                    return false;
                }

                var user = _mapper.Map<UserCredentials>(userDto);
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                await _userCredentialsRepository.AddUserAsync(user);
                _logger.LogInformation("User added successfully: {Email}", userDto.Email);

                var userProfile = new UserProfile
                {
                    UserId = user.UserId,
                    FirstName = string.Empty,
                    LastName = string.Empty,
                    ProfilePicture = string.Empty,
                    Description = string.Empty,
                    DateOfBirth = DateTime.MinValue
                };

                await _userProfileRepository.AddUserProfileAsync(userProfile);
                _logger.LogInformation("UserProfile added successfully for user: {Email}", userDto.Email);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during registration for email: {Email}", userDto.Email);
                return false;
            }
        }

        public async Task<string> AuthenticateUserAsync(AuthRequestDto authRequest)
        {
            _logger.LogInformation("AuthenticateUserAsync called for email: {Email}", authRequest.Email);

            try
            {
                var user = await _userCredentialsRepository.GetUserByEmailAsync(authRequest.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(authRequest.Password, user.PasswordHash))
                {
                    _logger.LogWarning("Authentication failed: Invalid credentials for email: {Email}", authRequest.Email);
                    return null;
                }

                var token = GenerateJwtToken(user);
                _logger.LogInformation("Authentication successful for email: {Email}", authRequest.Email);
                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during authentication for email: {Email}", authRequest.Email);
                return null;
            }
        }

        public Task<bool> LogoutUserAsync()
        {
            _logger.LogInformation("LogoutUserAsync called.");
            // Implement logout logic if necessary
            return Task.FromResult(true);
        }

        public Task<bool> IsTokenValidAsync(string token)
        {
            _logger.LogInformation("IsTokenValidAsync called.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validationParameters, out _);
                _logger.LogInformation("Token validation successful.");
                return Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                return Task.FromResult(false);
            }
        }

        private string GenerateJwtToken(UserCredentials user)
        {
            _logger.LogInformation("GenerateJwtToken called for user: {Email}", user.Email);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15), // Set expiration to 15 minutes
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _logger.LogInformation("JWT token generated: {Token}", tokenString);
            return tokenString;
        }
    }
}
