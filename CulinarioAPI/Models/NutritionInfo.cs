using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models
{
    public class NutritionInfo
    {
        [Key]
        public int NutritionInfoId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public double ServingSizeGrams { get; set; }
        public double CaloriesPer100g { get; set; }
        public double Carbs { get; set; }
        public double Fats { get; set; }
        public double Protein { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
