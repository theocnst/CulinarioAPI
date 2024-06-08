using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }

        [ForeignKey("Recipe")]
        public int RecipeId { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string Name { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
