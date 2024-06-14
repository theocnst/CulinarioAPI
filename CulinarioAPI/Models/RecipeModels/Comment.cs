using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CulinarioAPI.Models.UserModels;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [ForeignKey("UserProfile")]
        public string Username { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        [Required]
        public string Note { get; set; }

        public virtual UserProfile UserProfile { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
