using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Rating
    {
        [Key]
        public int RatingId { get; set; }

        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public double Score { get; set; }  // Rating score

        public virtual UserProfile UserProfile { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
