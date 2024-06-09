using AutoMapper;
using CulinarioAPI.Dtos.UserDtos;
using CulinarioAPI.Repositories.UserRepositories;

namespace CulinarioAPI.Services.UserServices
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserProfileService> _logger;

        public UserProfileService(IUserProfileRepository userProfileRepository, IMapper mapper, ILogger<UserProfileService> logger)
        {
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(string username)
        {
            _logger.LogInformation("GetUserProfileAsync called with username: {Username}", username);

            try
            {
                var profile = await _userProfileRepository.GetUserProfileByUsernameAsync(username);
                if (profile == null)
                {
                    _logger.LogWarning("UserProfile not found for username: {Username}", username);
                    return null;
                }

                var profileDto = _mapper.Map<UserProfileDto>(profile);
                _logger.LogInformation("UserProfile found and mapped for username: {Username}", username);
                return profileDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting UserProfile for username: {Username}", username);
                return null;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(string username, UserProfileUpdateDto profileDto)
        {
            _logger.LogInformation("UpdateUserProfileAsync called with username: {Username}", username);

            try
            {
                if (!await _userProfileRepository.UserProfileExistsAsync(username))
                {
                    _logger.LogWarning("Update failed, UserProfile not found for username: {Username}", username);
                    return false;
                }

                var profile = await _userProfileRepository.GetUserProfileByUsernameAsync(username);
                _mapper.Map(profileDto, profile);

                await _userProfileRepository.UpdateUserProfileAsync(profile);
                _logger.LogInformation("UserProfile updated for username: {Username}", username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating UserProfile for username: {Username}", username);
                return false;
            }
        }
    }
}
