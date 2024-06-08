using System.ComponentModel.DataAnnotations;

namespace CulinarioAPI.Dtos.UserDtos
{
    public class UserRegistrationDto
    {
        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
