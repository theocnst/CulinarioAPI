namespace CulinarioAPI.Dtos.RecipeDtos
{
    public class RecipeDto
    {
        public int RecipeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }
        public int Servings { get; set; }
        public string Description { get; set; }
        public string AdminUsername { get; set; }
        public string CountryName { get; set; }
        public RecipeTypeDto RecipeType { get; set; }
        public NutritionInfoDto NutritionInfo { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<InstructionDto> Instructions { get; set; }

        // New fields for ratings
        public double AverageRating { get; set; }
        public int NumberOfRatings { get; set; }
    }
}
