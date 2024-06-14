namespace CulinarioAPI.Dtos.RecipeDtos
{
    public class CommentDto
    {
        public string Username { get; set; }
        public int RecipeId { get; set; }
        public string Note { get; set; }
        public string ProfilePicture { get; set; }
        public string Name { get; set; }
    }

    public class CreateCommentDto
    {
        public string Username { get; set; }
        public int RecipeId { get; set; }
        public string Note { get; set; }
    }

    public class DeleteCommentDto
    {
        public string Username { get; set; }
        public int RecipeId { get; set; }
    }
}
