using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Blog_Project.Application.DTOs
{
    public record Error(string Code, string Description)
    {
        public static readonly Error None = new(string.Empty, string.Empty);
    }

    public class Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public T? Value { get; }
        public Error Error { get; }

        private Result(bool isSuccess, Error error, T? value)
        {
            if(isSuccess && error != Error.None)
                throw new InvalidOperationException("A successful result cannot contain an error.");

            if(IsFailure && error == Error.None)
                throw new InvalidOperationException("A failure result must contain an error.");

            IsSuccess = isSuccess;
            Error = error;
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, Error.None, value);

        public static Result<T> Failure(Error error) => new(false, error, default);
    }
}
