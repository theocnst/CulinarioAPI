using System.Collections.Generic;

namespace CulinarioAPI.Dtos.UserDtos
{
    public class UserDetailsDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public List<FriendDto> Friends { get; set; }
        public List<LikedRecipeDto> LikedRecipes { get; set; }
    }

    public class FriendDto
    {
        public string Username { get; set; }
    }

    public class LikedRecipeDto
    {
        public string Name { get; set; }
    }
}
