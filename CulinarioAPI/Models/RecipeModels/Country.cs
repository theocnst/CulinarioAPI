using System.ComponentModel.DataAnnotations;

namespace CulinarioAPI.Models.RecipeModels
{
    public class Country
    {
        [Key]
        public int CountryId { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
    }
}
