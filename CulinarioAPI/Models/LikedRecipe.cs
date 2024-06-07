using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models
{
    public class LikedRecipe
    {
        [Key]
        public int LikedRecipeId { get; set; }

        [ForeignKey("UserProfile")]
        public int UserId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
