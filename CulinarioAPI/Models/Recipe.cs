using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models
{
    public class Recipe
    {
        [Key]
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public TimeSpan PrepTime { get; set; }
        public TimeSpan CookTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public int Servings { get; set; }
        public string Description { get; set; }

        [ForeignKey("UserCredentials")]
        public int AdminId { get; set; }
        public virtual UserCredentials Admin { get; set; }

        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<Ingredient> Ingredients { get; set; }
        public virtual NutritionInfo NutritionInfo { get; set; }
        public virtual ICollection<Rating> Ratings { get; set; }

        [NotMapped]
        public double StarRating => Ratings.Any() ? Ratings.Average(r => r.Score) : 0;

        [NotMapped]
        public int NumberOfVotes => Ratings.Count;
    }
}
