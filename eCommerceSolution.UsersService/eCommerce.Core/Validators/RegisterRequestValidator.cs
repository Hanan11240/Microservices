using eCommerce.Core.DTO;
using FluentValidation;


namespace eCommerce.Core.Validators
{
    public class RegisterRequestValidator:AbstractValidator<RegisterRequest>
    {
       public RegisterRequestValidator() {
            RuleFor(temp => temp.Email).NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invaldi email format");
            RuleFor(temp => temp.PersonName).NotEmpty().WithMessage("Person Name is required")
                .MinimumLength(3).WithMessage("Person name must be atleast of 3 characters");
            RuleFor(temp => temp.Password).NotEmpty().WithMessage("Password  is required");

            RuleFor(temp => temp.Gender).IsInEnum().WithMessage("Not correct enum value");
               

        }
    }
}
