namespace Blog_Project.Infrastructure.FileStrorage
{
    public class FileStorageSettings
    {
        public string UploadsFolder { get; set; }
        public int MaxFileSizeMB { get; set; }
        public List<string> AllowedExtensions { get; set; }
    }
}
