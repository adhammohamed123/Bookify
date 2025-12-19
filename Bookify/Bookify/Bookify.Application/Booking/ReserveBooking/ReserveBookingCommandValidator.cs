using FluentValidation;

namespace Bookify.Application.Booking.ReserveBooking
{
    internal sealed class ReserveBookingCommandValidator : AbstractValidator<ReserveBookingCommand>
    {
        public ReserveBookingCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
            RuleFor(x => x.ApartmentId).NotEmpty().WithMessage("ApartmentId is required.");
            RuleFor(x => x.StartDateUtc).LessThan(x => x.EndDateUtc).WithMessage("Start Date must be earlier than End Date.");
        }
    }

}
