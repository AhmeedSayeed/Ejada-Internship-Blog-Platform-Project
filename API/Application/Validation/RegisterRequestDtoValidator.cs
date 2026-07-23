using API.Application.DTOs;
using FluentValidation;
using API.Domain.Constants;

namespace API.Application.Validation
{
    public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
    {
        public RegisterRequestDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.UserName)
                .MinimumLength(ValidationConstants.UsernameMinLength).WithMessage($"Username must be at least {ValidationConstants.UsernameMinLength} characters long.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(ValidationConstants.PasswordMinLength).WithMessage($"Password must be at least {ValidationConstants.PasswordMinLength} characters long.")
                .Must(password =>
                {
                    return password.Any(char.IsUpper) &&
                           password.Any(char.IsLower) &&
                           password.Any(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)) &&
                           !password.Any(char.IsWhiteSpace);
                }).WithMessage("Password must contain at least one uppercase letter, " +
                "one lowercase letter, one number, and one special character.");

            RuleFor(x => x.Role)
                .Must(role => role == "Reader" || role == "Author")
                .WithMessage("Role must be either 'Reader' or 'Author'.");
        }
    }
}
