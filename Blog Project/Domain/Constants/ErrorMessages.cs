namespace Blog_Project.Domain.Constants
{
    public static class ErrorMessages
    {
        public const string PostNotFound = "Post not found.";
        public const string AuthorNotFound = "Author not found.";
        public const string CategoryNotFound = "Category not found.";

        public const string UnauthorizedPostUpdate =
            "You are not authorized to update this post.";

        public const string UnauthorizedPostDelete =
            "You are not authorized to delete this post.";

        public const string DraftOnly =
            "Only draft posts can be submitted.";
        public const string InvalidImg =
       "Invalid image.";
        
    }
}
