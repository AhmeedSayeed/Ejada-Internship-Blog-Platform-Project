using FluentValidation;
using Web.ViewModel;

namespace Web.Validation
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Must(p => p.Any(char.IsUpper) && p.Any(char.IsLower)
                && p.Any(char.IsDigit) && p.Any(c => !char.IsWhiteSpace(c) && !char.IsLetterOrDigit(c)))
                .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("Passwords do not match.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => role == "Reader" || role == "Author").WithMessage("Invalid role selected.");
        }
    }
}
