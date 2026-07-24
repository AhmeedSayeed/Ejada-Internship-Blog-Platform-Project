using System;
using System.Collections.Generic;
using System.Text;

namespace API
{
    public class ApiErrorResponse
    {
        public string Title { get; set; } = "An error occurred.";
        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
    }

    public static class ApiErrors
    {
        public static ApiErrorResponse From(Error error) =>
            new() { Title = "Request failed.", Errors = new[] { error.Description } };

        public static ApiErrorResponse From(IEnumerable<string>? errors, string title = "Request failed.") =>
            new() { Title = title, Errors = errors ?? Array.Empty<string>() };
    }
}
