using CulinarioAPI.Data;
using CulinarioAPI.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace CulinarioAPI.Repositories.UserRepositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserProfileRepository> _logger;

        public UserProfileRepository(ApplicationDbContext context, ILogger<UserProfileRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<UserProfile> GetUserProfileByUsernameAsync(string username)
        {
            _logger.LogInformation("GetUserProfileByUsernameAsync called with username: {Username}", username);

            try
            {
                var profile = await _context.UserProfiles
                    .Include(p => p.Ratings)
                    .Include(p => p.Friendships)
                    .Include(p => p.LikedRecipes)
                    .SingleOrDefaultAsync(p => p.Username == username);

                if (profile == null)
                {
                    _logger.LogWarning("UserProfile not found with username: {Username}", username);
                }
                else
                {
                    _logger.LogInformation("UserProfile found with username: {Username}", username);
                }
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting UserProfile by username: {Username}", username);
                throw;
            }
        }


        public async Task AddUserProfileAsync(UserProfile profile)
        {
            _logger.LogInformation("AddUserProfileAsync called for username: {Username}", profile.Username);

            try
            {
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserProfile added successfully for username: {Username}", profile.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding UserProfile for username: {Username}", profile.Username);
                throw;
            }
        }

        public async Task UpdateUserProfileAsync(UserProfile profile)
        {
            _logger.LogInformation("UpdateUserProfileAsync called for username: {Username}", profile.Username);

            try
            {
                _context.Entry(profile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserProfile updated successfully for username: {Username}", profile.Username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating UserProfile for username: {Username}", profile.Username);
                throw;
            }
        }

        public async Task<bool> UserProfileExistsAsync(string username)
        {
            _logger.LogInformation("UserProfileExistsAsync called with username: {Username}", username);

            try
            {
                var exists = await _context.UserProfiles.AnyAsync(p => p.Username == username);
                _logger.LogInformation("UserProfile exists: {Exists} for username: {Username}", exists, username);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if UserProfile exists: {Username}", username);
                throw;
            }
        }
    }
}