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
        public async Task<UserDetailsDto> GetUserDetailsAsync(string username)
        {
            _logger.LogInformation("GetUserDetailsAsync called with username: {Username}", username);

            var userDetails = await _userProfileRepository.GetUserDetailsByUsernameAsync(username);
            if (userDetails == null)
            {
                _logger.LogWarning("User details not found for username: {Username}", username);
                return null;
            }

            _logger.LogInformation("User details found for username: {Username}", username);
            return userDetails;
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
        public async Task<bool> AddFriendAsync(FriendshipDto friendshipDto)
        {
            _logger.LogInformation("AddFriendAsync called with username: {Username} and friendUsername: {FriendUsername}", friendshipDto.Username, friendshipDto.FriendUsername);

            try
            {
                return await _userProfileRepository.AddFriendAsync(friendshipDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding friend for username: {Username}", friendshipDto.Username);
                return false;
            }
        }


        public async Task<bool> RemoveFriendAsync(FriendshipDto friendshipDto)
        {
            _logger.LogInformation("RemoveFriendAsync called with username: {Username} and friendUsername: {FriendUsername}", friendshipDto.Username, friendshipDto.FriendUsername);

            try
            {
                return await _userProfileRepository.RemoveFriendAsync(friendshipDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while removing friend for username: {Username}", friendshipDto.Username);
                return false;
            }
        }
    }
}
