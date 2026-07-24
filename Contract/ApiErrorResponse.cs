using System;
using System.Collections.Generic;
using System.Text;

namespace Contract
{
    public class ApiErrorResponse
    {
        public string Title { get; set; } = "An error occurred.";
        public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
    }
}
