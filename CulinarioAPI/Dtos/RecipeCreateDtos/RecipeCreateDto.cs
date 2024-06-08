namespace CulinarioAPI.Dtos.RecipeCreateDtos
{
    public class RecipeCreateDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public int PrepTime { get; set; }
        public int CookTime { get; set; }
        public int TotalTime { get; set; }
        public int Servings { get; set; }
        public string Description { get; set; }
        public int AdminId { get; set; }
        public List<IngredientCreateDto> Ingredients { get; set; }
        public List<InstructionCreateDto> Instructions { get; set; }
        public NutritionInfoCreateDto NutritionInfo { get; set; }
    }
}