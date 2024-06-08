using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models
{
    public class UserProfile
    {
        [Key, ForeignKey("UserCredentials")]
        public int UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePicture { get; set; }
        public string Description { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual UserCredentials UserCredentials { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<LikedRecipe> LikedRecipes { get; set; }
    }
}
