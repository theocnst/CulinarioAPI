using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models.RecipeModels
{
    public class NutritionInfo
    {
        [Key, ForeignKey("Recipe")]
        public int RecipeId { get; set; }

        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Protein { get; set; }
        public int Fiber { get; set; }
        public int Sugar { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
