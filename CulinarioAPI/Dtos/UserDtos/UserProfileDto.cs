namespace CulinarioAPI.Dtos.UserDtos
{
    public class UserProfileDto
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
