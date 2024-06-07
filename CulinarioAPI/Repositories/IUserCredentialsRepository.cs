using CulinarioAPI.Models;

public interface IUserCredentialsRepository
{
    Task<UserCredentials> GetUserByEmailAsync(string email);
    Task AddUserAsync(UserCredentials user);
    Task<bool> UserExistsAsync(string email);
}