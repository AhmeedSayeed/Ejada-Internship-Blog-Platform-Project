using Blog_Project.Application.DTOs;
using FluentValidation;
using Blog_Project.Domain.Constants;

namespace Blog_Project.Application.Validation
{
    public class UpdateProfileDtoValidator : AbstractValidator<UpdateProfileDto>
    {
        public UpdateProfileDtoValidator()
        {
            RuleFor(x => x.UserName)
                .MinimumLength(ValidationConstants.UsernameMinLength).When(x => !string.IsNullOrEmpty(x.UserName))
                .WithMessage($"UserName must be at least {ValidationConstants.UsernameMinLength} characters long.")
                .MaximumLength(ValidationConstants.UsernameMaxLength).When(x => !string.IsNullOrEmpty(x.UserName))
                .WithMessage($"UserName cannot exceed {ValidationConstants.UsernameMaxLength} characters.");

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email format.");

            RuleFor(x => x.Bio)
                .MaximumLength(ValidationConstants.BioMaxLength).When(x => !string.IsNullOrEmpty(x.Bio))
                .WithMessage($"Bio cannot exceed {ValidationConstants.BioMaxLength} characters.");

            RuleFor(x => x.ProfileImageUrl)
                .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .When(x => !string.IsNullOrEmpty(x.ProfileImageUrl)).WithMessage("ProfileImageUrl must be a valid URL.");

            RuleFor(x => x.NewPassword)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.CurrentPassword))
                .WithMessage("NewPassword is required when CurrentPassword is provided.")
                .MinimumLength(ValidationConstants.PasswordMinLength).When(x => !string.IsNullOrEmpty(x.NewPassword))
                .WithMessage($"NewPassword must be at least {ValidationConstants.PasswordMinLength} characters long.")
                .Must(p => p.Any(char.IsUpper) && p.Any(char.IsLower) && p.Any(char.IsDigit) && p.Any(c => !char.IsWhiteSpace(c) && !char.IsLetterOrDigit(c)))
                .When(x => !string.IsNullOrEmpty(x.NewPassword))
                .WithMessage("NewPassword must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

            RuleFor(x => x.CurrentPassword)
                .NotEmpty().When(x => !string.IsNullOrEmpty(x.NewPassword))
                .WithMessage("CurrentPassword is required when changing the password.");
        }
    }
}
