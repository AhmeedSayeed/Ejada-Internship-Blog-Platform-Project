namespace Blog_Project.Domain.Constants
{
    public static class AppRoles
    {
        public const string Admin = "Admin";    
        public const string Author = "Author";
        public const string Reader = "Reader";

        public static readonly string[] AllRoles = { Admin, Author, Reader };
    }
}
