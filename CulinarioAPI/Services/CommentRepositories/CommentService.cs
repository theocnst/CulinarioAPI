using AutoMapper;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Models.RecipeModels;
using CulinarioAPI.Repositories.RecipeRepositories;

namespace CulinarioAPI.Services.RecipeServices
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetAllCommentsForRecipeAsync(int recipeId)
        {
            var comments = await _commentRepository.GetAllCommentsForRecipeAsync(recipeId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task PostCommentAsync(CreateCommentDto createCommentDto)
        {
            var comment = new Comment
            {
                Username = createCommentDto.Username,
                RecipeId = createCommentDto.RecipeId,
                Note = createCommentDto.Note
            };
            await _commentRepository.AddCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(DeleteCommentDto deleteCommentDto)
        {
            var comment = await _commentRepository.GetCommentAsync(deleteCommentDto.Username, deleteCommentDto.RecipeId);
            if (comment != null)
            {
                await _commentRepository.DeleteCommentAsync(comment);
            }
        }
    }
}
