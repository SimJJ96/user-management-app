using FluentValidation;
using UserManagement.API.DTOs;

namespace UserManagement.API.Validators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
                .Matches(@"^[\p{L} \-'\.]+$").WithMessage("First name contains invalid characters."); ;

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
                .Matches(@"^[\p{L} \-'\.]+$").WithMessage("Last name contains invalid characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\+?\d{7,15}$").WithMessage("Phone number must be 7–15 digits with optional '+' prefix.")
                .When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber));

            RuleFor(x => x.ZipCode)
                .Matches(@"^[A-Za-z0-9\- ]{3,10}$").WithMessage("Zip code must be 3–10 alphanumeric characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.ZipCode));
        }
    }
}
