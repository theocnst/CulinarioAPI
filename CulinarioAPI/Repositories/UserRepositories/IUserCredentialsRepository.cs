using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Repositories.UserRepositories
{
    public interface IUserCredentialsRepository
    {
        Task<UserCredentials> GetUserByEmailAsync(string email);
        Task AddUserAsync(UserCredentials user);
        Task<bool> UserExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
    }
}