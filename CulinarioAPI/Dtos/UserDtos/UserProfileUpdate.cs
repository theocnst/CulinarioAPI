namespace CulinarioAPI.Dtos.UserDtos
{
    public class UserProfileUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
