using AutoMapper;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Models.RecipeModels;
using CulinarioAPI.Repositories.RecipeRepositories;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CulinarioAPI.Services.RecipeServices
{
    public class RecipeService : IRecipeService
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RecipeService> _logger;

        public RecipeService(IRecipeRepository recipeRepository, IMapper mapper, ILogger<RecipeService> logger)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RecipeDto>> GetAllRecipesAsync()
        {
            _logger.LogInformation("GetAllRecipesAsync called");
            var recipes = await _recipeRepository.GetAllRecipesAsync();
            return _mapper.Map<IEnumerable<RecipeDto>>(recipes);
        }

        public async Task<RecipeDto> GetRecipeByIdAsync(int id)
        {
            _logger.LogInformation("GetRecipeByIdAsync called with id: {Id}", id);
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task<RecipeDto> AddRecipeAsync(RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("AddRecipeAsync called");
            var recipe = _mapper.Map<Recipe>(recipeCreateDto);
            await _recipeRepository.AddRecipeAsync(recipe);

            // Return the created recipe with the generated RecipeId
            return _mapper.Map<RecipeDto>(recipe);
        }

        public async Task UpdateRecipeAsync(int id, RecipeCreateDto recipeCreateDto)
        {
            _logger.LogInformation("UpdateRecipeAsync called with id: {Id}", id);
            var recipe = await _recipeRepository.GetRecipeByIdAsync(id);
            if (recipe != null)
            {
                _mapper.Map(recipeCreateDto, recipe);
                await _recipeRepository.UpdateRecipeAsync(recipe);
            }
        }

        public async Task DeleteRecipeAsync(int id)
        {
            _logger.LogInformation("DeleteRecipeAsync called with id: {Id}", id);
            await _recipeRepository.DeleteRecipeAsync(id);
        }
    }
}
