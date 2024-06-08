using CulinarioAPI.Dtos.UserDtos;

namespace CulinarioAPI.Services.UserServices
{
    public interface IUserCredentialsService
    {
        Task<bool> RegisterUserAsync(UserRegistrationDto userDto);
        Task<string> AuthenticateUserAsync(AuthRequestDto authRequest);
        Task<bool> LogoutUserAsync();
        Task<bool> IsTokenValidAsync(string token);
    }
}
