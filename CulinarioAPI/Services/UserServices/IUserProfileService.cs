using CulinarioAPI.Dtos.UserDtos;

namespace CulinarioAPI.Services.UserServices
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(string username);
        Task<UserDetailsDto> GetUserDetailsAsync(string username);
        Task<bool> UpdateUserProfileAsync(string username, UserProfileUpdateDto profileDto);
        Task<bool> AddFriendAsync(FriendshipDto friendshipDto);
        Task<bool> RemoveFriendAsync(FriendshipDto friendshipDto);
        Task<bool> AddLikedRecipeAsync(LikedRecipeOperationDto likedRecipeDto);
        Task<bool> RemoveLikedRecipeAsync(LikedRecipeOperationDto likedRecipeDto);
        Task<string> GetUserProfilePicAsync(string username);

    }
}
