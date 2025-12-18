using Bookify.Domain.Shared;
using FluentValidation;

namespace Bookify.Application.Apartment.AddApartment
{
    internal sealed class AddApartmentCommandValidator:AbstractValidator<AddApartmentCommad>
    {
        IEnumerable<Currency> allowedCurrencies => Currency.All;
       
        public AddApartmentCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Apartment name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Apartment description is required.");
            RuleFor(x => x.State).NotEmpty().WithMessage("State is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("ZipCode is required.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
            RuleFor(x=>x.Price.Amount).GreaterThan(0).WithMessage("Price must be greater than zero.");
            RuleFor(x => x.Price.Currency).Must(c=> allowedCurrencies.Contains(c)).WithMessage($"Price currency is not supported,allowed Currencies {string.Join(",",allowedCurrencies)}");
            RuleFor(x => x.CleanningFee.Amount).GreaterThanOrEqualTo(0).WithMessage("Cleanning fee must be greater than or equal to zero.");
            RuleFor(x => x.CleanningFee.Currency).Must(c => allowedCurrencies.Contains(c)).WithMessage($"CleanningFee currency is not supported,allowed Currencies {string.Join(",", allowedCurrencies)}");
            RuleFor(x => x.Amenities).NotEmpty().WithMessage("At least one amenity is required.");
        }
    }
}
