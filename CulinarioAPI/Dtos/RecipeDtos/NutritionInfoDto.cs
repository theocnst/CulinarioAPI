namespace CulinarioAPI.Dtos.RecipeDtos
{
    public class NutritionInfoDto
    {
        public int RecipeId { get; set; }
        public int Calories { get; set; }
        public int Fats { get; set; }
        public int Carbs { get; set; }
        public int Protein { get; set; }
        public int Fiber { get; set; }
        public int Sugar { get; set; }
    }
}
