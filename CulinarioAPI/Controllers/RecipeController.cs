using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Services.RecipeServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CulinarioAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly IRecipeService _recipeService;
        private readonly ILogger<RecipeController> _logger;

        public RecipeController(IRecipeService recipeService, ILogger<RecipeController> logger)
        {
            _recipeService = recipeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            _logger.LogInformation("GetAllRecipes called");
            var recipes = await _recipeService.GetAllRecipesAsync();
            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(int id)
        {
            _logger.LogInformation("GetRecipeById called with id: {Id}", id);
            var recipe = await _recipeService.GetRecipeByIdAsync(id);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe not found with id: {Id}", id);
                return NotFound();
            }
            return Ok(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("AddRecipe called");

            // Add the recipe
            var createdRecipe = await _recipeService.AddRecipeAsync(recipeCreateDto);

            // Return the created recipe
            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.RecipeId }, createdRecipe);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("UpdateRecipe called with id: {Id}", id);
            await _recipeService.UpdateRecipeAsync(id, recipeCreateDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation("DeleteRecipe called with id: {Id}", id);
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }
    }
}
