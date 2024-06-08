using System.ComponentModel.DataAnnotations;

namespace CulinarioAPI.Dtos.UserDtos
{
    public class UserProfileDto
    {
        public int UserProfileId { get; set; }
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
