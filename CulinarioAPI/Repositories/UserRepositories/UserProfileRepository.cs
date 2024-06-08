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

        public async Task<UserProfile> GetUserProfileByIdAsync(int id)
        {
            _logger.LogInformation("GetUserProfileByIdAsync called with id: {Id}", id);

            try
            {
                var profile = await _context.UserProfiles
                    .Include(p => p.Ratings)
                    .Include(p => p.Friendships)
                    .Include(p => p.LikedRecipes)
                    .SingleOrDefaultAsync(p => p.UserId == id);

                if (profile == null)
                {
                    _logger.LogWarning("UserProfile not found with id: {Id}", id);
                }
                else
                {
                    _logger.LogInformation("UserProfile found with id: {Id}", id);
                }
                return profile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting UserProfile by id: {Id}", id);
                throw;
            }
        }

        public async Task AddUserProfileAsync(UserProfile profile)
        {
            _logger.LogInformation("AddUserProfileAsync called for UserId: {UserId}", profile.UserId);

            try
            {
                _context.UserProfiles.Add(profile);
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserProfile added successfully for UserId: {UserId}", profile.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding UserProfile for UserId: {UserId}", profile.UserId);
                throw;
            }
        }

        public async Task UpdateUserProfileAsync(UserProfile profile)
        {
            _logger.LogInformation("UpdateUserProfileAsync called for UserId: {UserId}", profile.UserId);

            try
            {
                _context.Entry(profile).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                _logger.LogInformation("UserProfile updated successfully for UserId: {UserId}", profile.UserId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating UserProfile for UserId: {UserId}", profile.UserId);
                throw;
            }
        }

        public async Task<bool> UserProfileExistsAsync(int id)
        {
            _logger.LogInformation("UserProfileExistsAsync called with id: {Id}", id);

            try
            {
                var exists = await _context.UserProfiles.AnyAsync(p => p.UserId == id);
                _logger.LogInformation("UserProfile exists: {Exists} for id: {Id}", exists, id);
                return exists;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if UserProfile exists: {Id}", id);
                throw;
            }
        }
    }
}