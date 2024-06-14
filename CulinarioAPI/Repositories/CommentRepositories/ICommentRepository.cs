using CulinarioAPI.Models.RecipeModels;

namespace CulinarioAPI.Repositories.RecipeRepositories
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetAllCommentsForRecipeAsync(int recipeId);
        Task<Comment> GetCommentAsync(string username, int recipeId);
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(Comment comment);
    }
}
