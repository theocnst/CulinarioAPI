using AutoMapper;
using CulinarioAPI.Dtos;
using CulinarioAPI.Dtos.RecipeCreateDtos;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Models.RecipeModels;
using CulinarioAPI.Repositories.RecipeRepositories;

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

            // Ensure the country exists or create it
            var country = await _recipeRepository.GetCountryByNameAsync(recipeCreateDto.CountryName);
            if (country == null)
            {
                country = new Country { CountryName = recipeCreateDto.CountryName };
                await _recipeRepository.AddCountryAsync(country);
                await _recipeRepository.SaveChangesAsync(); // Ensure changes are saved before proceeding
            }

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
                // Ensure the country exists or create it
                var country = await _recipeRepository.GetCountryByNameAsync(recipeCreateDto.CountryName);
                if (country == null)
                {
                    country = new Country { CountryName = recipeCreateDto.CountryName };
                    await _recipeRepository.AddCountryAsync(country);
                }

                _mapper.Map(recipeCreateDto, recipe);
                await _recipeRepository.UpdateRecipeAsync(recipe);
            }
        }

        public async Task DeleteRecipeAsync(int id)
        {
            _logger.LogInformation("DeleteRecipeAsync called with id: {Id}", id);
            await _recipeRepository.DeleteRecipeAsync(id);
        }

        public async Task<IEnumerable<CountryDto>> GetAllCountriesAsync()
        {
            _logger.LogInformation("GetAllCountriesAsync called");
            var countries = await _recipeRepository.GetAllCountriesAsync();
            return _mapper.Map<IEnumerable<CountryDto>>(countries);
        }

        public IEnumerable<RecipeTypeDto> GetRecipeTypes()
        {
            _logger.LogInformation("GetRecipeTypes called");
            return Enum.GetValues(typeof(RecipeType))
                       .Cast<RecipeType>()
                       .Select(rt => new RecipeTypeDto { Name = rt.ToString() })
                       .ToList();
        }
        public async Task<RecipeDto> RateRecipeAsync(RatingDto ratingDto)
        {
            _logger.LogInformation("RateRecipeAsync called for RecipeId: {RecipeId} by User: {Username}", ratingDto.RecipeId, ratingDto.Username);

            if (ratingDto.Score < 1 || ratingDto.Score > 10)
            {
                _logger.LogWarning("Invalid rating score: {Score} by User: {Username} for RecipeId: {RecipeId}", ratingDto.Score, ratingDto.Username, ratingDto.RecipeId);
                throw new ArgumentException("Rating score must be between 1 and 10.");
            }

            var rating = await _recipeRepository.GetRatingByUserAndRecipeAsync(ratingDto.Username, ratingDto.RecipeId);
            if (rating == null)
            {
                rating = new Rating
                {
                    Username = ratingDto.Username,
                    RecipeId = ratingDto.RecipeId,
                    Score = ratingDto.Score
                };
                await _recipeRepository.AddRatingAsync(rating);
            }
            else
            {
                rating.Score = ratingDto.Score;
                await _recipeRepository.UpdateRatingAsync(rating);
            }

            var recipe = await _recipeRepository.GetRecipeByIdAsync(ratingDto.RecipeId);
            _logger.LogInformation("RecipeId: {RecipeId} has now {NumberOfRatings} ratings", recipe.RecipeId, recipe.Ratings.Count);

            await _recipeRepository.UpdateRecipeAsync(recipe);

            return _mapper.Map<RecipeDto>(recipe);
        }

    }
}
