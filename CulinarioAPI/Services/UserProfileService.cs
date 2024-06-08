using AutoMapper;
using CulinarioAPI.Dtos;
using CulinarioAPI.Repositories;

namespace CulinarioAPI.Services
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

        public async Task<UserProfileDto> GetUserProfileAsync(int id)
        {
            _logger.LogInformation("GetUserProfileAsync called with id: {Id}", id);

            try
            {
                var profile = await _userProfileRepository.GetUserProfileByIdAsync(id);
                if (profile == null)
                {
                    _logger.LogWarning("UserProfile not found for id: {Id}", id);
                    return null;
                }

                var profileDto = _mapper.Map<UserProfileDto>(profile);
                _logger.LogInformation("UserProfile found and mapped for id: {Id}", id);
                return profileDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting UserProfile for id: {Id}", id);
                return null;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(int id, UserProfileUpdateDto profileDto)
        {
            _logger.LogInformation("UpdateUserProfileAsync called with id: {Id}", id);

            try
            {
                if (!await _userProfileRepository.UserProfileExistsAsync(id))
                {
                    _logger.LogWarning("Update failed, UserProfile not found for id: {Id}", id);
                    return false;
                }

                var profile = await _userProfileRepository.GetUserProfileByIdAsync(id);
                _mapper.Map(profileDto, profile);

                await _userProfileRepository.UpdateUserProfileAsync(profile);
                _logger.LogInformation("UserProfile updated for id: {Id}", id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating UserProfile for id: {Id}", id);
                return false;
            }
        }
    }
}
