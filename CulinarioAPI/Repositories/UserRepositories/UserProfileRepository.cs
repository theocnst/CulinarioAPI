using CulinarioAPI.Data;
using CulinarioAPI.Dtos.UserDtos;
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
        public async Task<UserDetailsDto> GetUserDetailsByUsernameAsync(string username)
        {
            _logger.LogInformation("GetUserDetailsByUsernameAsync called with username: {Username}", username);

            var userProfile = await _context.UserProfiles
                .Include(p => p.Friendships)
                    .ThenInclude(f => f.FriendUserProfile)
                .Include(p => p.LikedRecipes)
                    .ThenInclude(lr => lr.Recipe)
                .AsSplitQuery()
                .SingleOrDefaultAsync(p => p.Username == username);

            if (userProfile == null)
            {
                _logger.LogWarning("UserProfile not found for username: {Username}", username);
                return null;
            }

            var userDetails = new UserDetailsDto
            {
                Username = userProfile.Username,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                ProfilePicture = userProfile.ProfilePicture,
                Description = userProfile.Description,
                DateOfBirth = userProfile.DateOfBirth,
                Friends = userProfile.Friendships
                    .Where(f => f.FriendUserProfile != null) // Ensure no null FriendUserProfile
                    .Select(f => new FriendDto
                    {
                        Username = f.FriendUserProfile.Username
                    }).ToList(),
                LikedRecipes = userProfile.LikedRecipes
                    .Where(lr => lr.Recipe != null) // Ensure no null Recipe
                    .Select(lr => new LikedRecipeDto
                    {
                        RecipeId = lr.RecipeId,
                    }).ToList()
            };

            _logger.LogInformation("UserDetailsDto created for username: {Username}", username);
            return userDetails;
        }

        public async Task<bool> AddFriendAsync(FriendshipDto friendshipDto)
        {
            var user = await _context.UserProfiles.SingleOrDefaultAsync(u => u.Username == friendshipDto.Username);
            var friend = await _context.UserProfiles.SingleOrDefaultAsync(u => u.Username == friendshipDto.FriendUsername);

            if (user == null || friend == null)
            {
                return false;
            }

            var existingFriendship = await _context.Friendships
                .SingleOrDefaultAsync(f => f.Username == friendshipDto.Username && f.FriendUsername == friendshipDto.FriendUsername);

            if (existingFriendship != null)
            {
                return false; // Friendship already exists
            }

            var friendship = new Friendship
            {
                Username = user.Username,
                FriendUsername = friend.Username
            };

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> RemoveFriendAsync(FriendshipDto friendshipDto)
        {
            var friendship = await _context.Friendships.SingleOrDefaultAsync(f => f.Username == friendshipDto.Username && f.FriendUsername == friendshipDto.FriendUsername);

            if (friendship == null)
            {
                return false;
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}