using System.ComponentModel.DataAnnotations;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Country
    {
        [Key]
        public string CountryName { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
