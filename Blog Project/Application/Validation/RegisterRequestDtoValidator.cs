using Blog_Project.Application.DTOs;
using FluentValidation;

namespace Blog_Project.Application.Validation
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.UserName)
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Must(password =>
                {
                    return password.Any(char.IsUpper) &&
                           password.Any(char.IsLower) &&
                           password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) &&
                           !password.Any(char.IsWhiteSpace);
                }).WithMessage("Password must contain at least one uppercase letter, " +
                "one lowercase letter, one number, and one special character.");
        }
    }
}
