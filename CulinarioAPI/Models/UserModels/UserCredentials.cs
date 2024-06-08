using System.ComponentModel.DataAnnotations;
using CulinarioAPI.Models.RecipeModels;

namespace CulinarioAPI.Models.UserModels
{
    public class UserCredentials
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string PasswordHash { get; set; }

        public bool IsAdmin { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
