using CulinarioAPI.Dtos;

namespace CulinarioAPI.Services
{
    public interface IUserCredentialsService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto userDto);
        Task<string> AuthenticateUserAsync(AuthRequestDto authRequest);
        Task<bool> LogoutUserAsync();
        Task<bool> IsTokenValidAsync(string token);
    }
}
