using CulinarioAPI.Dtos.RecipeDtos;

namespace CulinarioAPI.Services.RecipeServices
{
    public interface ICommentService
    {
        Task<IEnumerable<CommentDto>> GetAllCommentsForRecipeAsync(int recipeId);
        Task PostCommentAsync(CreateCommentDto createCommentDto);
        Task DeleteCommentAsync(DeleteCommentDto deleteCommentDto);
    }
}
