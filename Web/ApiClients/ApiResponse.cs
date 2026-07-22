namespace Web.ApiClients
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Data { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }

    public class ApiResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
