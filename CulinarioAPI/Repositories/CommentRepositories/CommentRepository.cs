using CulinarioAPI.Data;
using CulinarioAPI.Models.RecipeModels;
using Microsoft.EntityFrameworkCore;

namespace CulinarioAPI.Repositories.RecipeRepositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllCommentsForRecipeAsync(int recipeId)
        {
            return await _context.Comments
                .Include(c => c.UserProfile)
                .Where(c => c.RecipeId == recipeId)
                .ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(string username, int recipeId)
        {
            return await _context.Comments
                .FirstOrDefaultAsync(c => c.Username == username && c.RecipeId == recipeId);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
