

using eCommerce.BusinessLogicLayer.DTO;
using FluentValidation;

namespace eCommerce.BusinessLogicLayer.Validators
{
    public class ProductAddRequestValidator:AbstractValidator<ProductAddRequest>
    {
        public ProductAddRequestValidator()
        {
            RuleFor(temp => temp.ProductName).NotEmpty().WithMessage("Product name cannot be empty");
            RuleFor(temp => temp.Category).IsInEnum().WithMessage("Not valid enum");
            RuleFor(temp => temp.UnitPrice).InclusiveBetween(0,double.MaxValue).WithMessage($"Unit price should be between  0 to {double.MaxValue}");
            RuleFor(temp => temp.QuantityInStock).InclusiveBetween(0,int.MaxValue).WithMessage($"Quantity in stock  should be between  0 to {int.MaxValue}");

        }
    }
}
