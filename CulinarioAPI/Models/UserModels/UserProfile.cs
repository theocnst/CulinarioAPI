using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinarioAPI.Models.RecipeModels;

namespace CulinarioAPI.Models.UserModels
{
    public class UserProfile
    {
        [Key, ForeignKey("UserCredentials")]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public virtual UserCredentials UserCredentials { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Friendship> Friendships { get; set; }
        public virtual ICollection<LikedRecipe> LikedRecipes { get; set; }
    }
}
