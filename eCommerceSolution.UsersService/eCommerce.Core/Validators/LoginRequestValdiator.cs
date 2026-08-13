

using eCommerce.Core.DTO;
using FluentValidation;

namespace eCommerce.Core.Validators
{
    public class LoginRequestValdiator:AbstractValidator<LoginRequest>
    {
        public LoginRequestValdiator()
        {
            // Email
            RuleFor(temp => temp.Email)
                .NotEmpty().WithMessage("Email must not be empty")
                .EmailAddress().WithMessage("Email is invalid");
            // Password
            RuleFor(temp => temp.Password)
                .NotEmpty().WithMessage("password cannot be empty");

        }
    }
}
