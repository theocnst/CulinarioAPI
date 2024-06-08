using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Repositories.UserRepositories
{
    public interface IUserProfileRepository
    {
        Task AddUserProfileAsync(UserProfile userProfile);
        Task<UserProfile> GetUserProfileByIdAsync(int id);
        Task UpdateUserProfileAsync(UserProfile profile);
        Task<bool> UserProfileExistsAsync(int id);
    }
}
