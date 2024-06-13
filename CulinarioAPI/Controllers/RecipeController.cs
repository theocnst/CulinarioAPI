using CulinarioAPI.Dtos;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Services.RecipeServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CulinarioAPI.Controllers
{
    [ApiController]
    [Authorize]
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

        [HttpGet("all")]
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("AddRecipe called with payload: {payload}", JsonConvert.SerializeObject(recipeCreateDto));
            var createdRecipe = await _recipeService.AddRecipeAsync(recipeCreateDto);
            return CreatedAtAction(nameof(GetRecipeById), new { id = createdRecipe.RecipeId }, createdRecipe);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("UpdateRecipe called with id: {Id}", id);
            await _recipeService.UpdateRecipeAsync(id, recipeCreateDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation("DeleteRecipe called with id: {Id}", id);
            await _recipeService.DeleteRecipeAsync(id);
            return NoContent();
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            _logger.LogInformation("GetAllCountries called");
            var countries = await _recipeService.GetAllCountriesAsync();
            return Ok(countries);
        }

        [HttpGet("recipetypes")]
        public IActionResult GetRecipeTypes()
        {
            _logger.LogInformation("GetRecipeTypes called");
            var recipeTypes = _recipeService.GetRecipeTypes();
            return Ok(recipeTypes);
        }

        // New API endpoint for rating a recipe
        [HttpPost("{id}/rate")]
        public async Task<IActionResult> RateRecipe(int id, [FromBody] RatingDto ratingDto)
        {
            _logger.LogInformation("RateRecipe called for RecipeId: {RecipeId} by User: {Username}", id, ratingDto.Username);

            if (id != ratingDto.RecipeId)
            {
                _logger.LogWarning("Recipe ID mismatch: {Id} vs {RecipeId}", id, ratingDto.RecipeId);
                return BadRequest("Recipe ID mismatch");
            }

            try
            {
                var updatedRecipe = await _recipeService.RateRecipeAsync(ratingDto);
                return Ok(updatedRecipe);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid rating score: {Score}", ratingDto.Score);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rating recipe");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
