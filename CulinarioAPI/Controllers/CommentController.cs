using System.Collections.Generic;
using System.Threading.Tasks;
using CulinarioAPI.Dtos.RecipeDtos;
using CulinarioAPI.Services.RecipeServices;
using Microsoft.AspNetCore.Mvc;

namespace CulinarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{recipeId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAllCommentsForRecipe(int recipeId)
        {
            var comments = await _commentService.GetAllCommentsForRecipeAsync(recipeId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult> PostComment([FromBody] CreateCommentDto createCommentDto)
        {
            await _commentService.PostCommentAsync(createCommentDto);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteComment([FromBody] DeleteCommentDto deleteCommentDto)
        {
            await _commentService.DeleteCommentAsync(deleteCommentDto);
            return Ok();
        }
    }
}
